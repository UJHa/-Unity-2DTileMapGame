using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eMoveDirection
{
    NONE,
    LEFT,
    RIGHT,
    UP,
    DOWN,
}
public class Character : MapObject
{
    protected GameObject _characterView;

    protected int _hp = 100;
    public int GetHP() { return _hp; }
    protected bool _isLive = true;
    protected int _attackPoint = 10;
    protected int _moveRange = 5;
    public int GetMoveRange()
    {
        return _moveRange;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (eStateType.NONE != _state.GetNextState())
        {
            ChangeState(_state.GetNextState());
        }

        _state.Update();
        UpdateCoolTime();

        UpdateGuage();
    }
    void UpdateCoolTime()
    {
        if (_attackCoolTime <= _deltaAttackCoolTime)
            _deltaAttackCoolTime = _attackCoolTime;
        else
            _deltaAttackCoolTime += Time.deltaTime;

        if (_moveCoolTime <= _deltaMoveCoolTime)
            _deltaMoveCoolTime = _moveCoolTime;
        else
            _deltaMoveCoolTime += Time.deltaTime;
    }
    void UpdateGuage()
    {
        if (null == _hpGuage || null == _coolTimeGuage)
            return;
        _hpGuage.value = _hp / 100.0f;
        _coolTimeGuage.value = _deltaAttackCoolTime / _attackCoolTime;
    }
    public void Init(string viewName)
    {
        //View를 붙인다.
        string filePath = "Prefabs/CharacterView/" + viewName;
        GameObject characterViewPrefabs = Resources.Load<GameObject>(filePath);
        _characterView = GameObject.Instantiate(characterViewPrefabs);
        _characterView.transform.SetParent(transform);
        _characterView.transform.localPosition = Vector3.zero;
        _characterView.transform.localScale = GameManager.Instance.GetMap().GetLocalScale();

        InitPosition();
        InitState();
    }

    void InitPosition()
    {
        TileMap map = GameManager.Instance.GetMap();
        _tileX = Random.Range(1, map.GetWidth() - 2);
        _tileY = Random.Range(1, map.GetHeight() - 2);

        while(false == map.GetTileCell(_tileX, _tileY).CanMove())
        {
            _tileX = Random.Range(1, map.GetWidth() - 2);
            _tileY = Random.Range(1, map.GetHeight() - 2);
        }
        map.SetObject(_tileX, _tileY, this, eTileLayer.MIDDLE);

        SetCanMove(false);
    }



    eMoveDirection _nextDirection = eMoveDirection.NONE;
    public eMoveDirection GetNextDirection() { return _nextDirection; }
    public void SetNextDirection(eMoveDirection direction) { _nextDirection = direction; }

    override public void SetSortingOrder(eTileLayer layer, int sortingOrder)
    {
        _tileLayer = layer;

        int sortingID = SortingLayer.NameToID(layer.ToString());

        _characterView.GetComponent<SpriteRenderer>().sortingLayerID = sortingID;
        _characterView.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }

    // State
    protected Dictionary<eStateType, State> _stateMap = new Dictionary<eStateType, State>();
    protected State _state;
    virtual protected void InitState()
    {
        {
            State state = new IdleState();
            state.Init(this);
            _stateMap[eStateType.IDLE] = state;
        }
        {
            State state = new MoveState();
            state.Init(this);
            _stateMap[eStateType.MOVE] = state;
        }
        {
            State state = new AttackState();
            state.Init(this);
            _stateMap[eStateType.ATTACK] = state;
        }
        {
            State state = new DamageState();
            state.Init(this);
            _stateMap[eStateType.DAMAGE] = state;
        }
        {
            State state = new DeathState();
            state.Init(this);
            _stateMap[eStateType.DEATH] = state;
        }
        _state = _stateMap[eStateType.IDLE];
    }
    private void ChangeState(eStateType nextState)
    {
        if (null != _state)
            _state.Stop();

        _state = _stateMap[nextState];
        _state.Start();
    }

    //Message
    override public void ReceiveObjectMessage(MessageParam msgParam)
    {
        switch (msgParam.message)
        {
            case "Attack":
                Debug.Log("recevie Attack!");
                _damagePoint = msgParam.attackPoint;
                SetAttacker(msgParam.sender);

                sPosition curPosition;
                curPosition.x = _tileX;
                curPosition.y = _tileY;
                sPosition attackedPosition;
                attackedPosition.x = msgParam.sender.GetTileX();
                attackedPosition.y = msgParam.sender.GetTileY();
                eMoveDirection direction = GetDirection(curPosition, attackedPosition);
                SetNextDirection(direction);
                MoveStart(attackedPosition.x, attackedPosition.y);
                _state.NextState(eStateType.DAMAGE);
                break;
            case "IsDead":
                Debug.Log("I'm dead!");
                Character msgSender = (Character)msgParam.sender;
                Debug.Log(msgSender.GetEXP());
                IncreaseEXP(msgSender.GetEXP());
                break;
        }
    }

    // Actions
    public bool MoveStart(int moveX, int moveY)
    {
        ResetMoveCooltime();
        string animationTrigger = "down";
        switch (_nextDirection)
        {
            case eMoveDirection.LEFT: animationTrigger = "left"; break;
            case eMoveDirection.RIGHT: animationTrigger = "right"; break;
            case eMoveDirection.UP: animationTrigger = "up"; break;
            case eMoveDirection.DOWN: animationTrigger = "down"; break;
        }

        _characterView.GetComponent<Animator>().SetTrigger(animationTrigger);

        TileMap map = GameManager.Instance.GetMap();

        List<MapObject> collisionList = map.GetCollisionList(moveX, moveY);
        if (null != collisionList && 0 == collisionList.Count) //이동 가능할때
        {
            map.ResetObject(_tileX, _tileY, this);
            _tileX = moveX;
            _tileY = moveY;
            map.SetObject(_tileX, _tileY, this, eTileLayer.MIDDLE);
            //pick message 주기
            List<MapObject> mapObejctList = map.GetTileCell(_tileX, _tileY).GetMapObjectList(eTileLayer.MIDDLE);
            for (int i = 0; i < mapObejctList.Count; i++)
            {
                MessageParam msgParam = new MessageParam();
                msgParam.sender = this;
                msgParam.receiver = mapObejctList[i];
                msgParam.message = "pick";

                MessageSystem.Instance.Send(msgParam);
            }
            return true;
        }
        return false;
    }
    public void Attack(MapObject enemy)
    {
        ResetAttackCooltime();
        MessageParam msgParam = new MessageParam();
        msgParam.sender = this;
        msgParam.receiver = enemy;
        msgParam.attackPoint = _attackPoint;
        msgParam.message = "Attack";

        MessageSystem.Instance.Send(msgParam);
    }

    public bool IsLive()
    {
        return _isLive;
    }
    public void DecreaseHP(int damagePoint)
    {
        string filePath = "Prefabs/Effects/DamageEffect";
        GameObject effectPrefabs = Resources.Load<GameObject>(filePath);
        GameObject effectObject = GameObject.Instantiate(effectPrefabs, transform.position, Quaternion.identity);
        GameObject.Destroy(effectObject, 1.0f);

        _characterView.GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor", 0.1f);
        _hp -= damagePoint;
        Debug.Log("remain hp : " + _hp);
        if (_hp <= 0)
        {
            _isLive = false;
            _hp = 0;
        }
    }

    int _damagePoint;
    public int GetDamagePoint()
    {
        return _damagePoint;
    }
    MapObject _attacker;
    public MapObject GetAttacker()
    {
        return _attacker;
    }
    public void SetAttacker(MapObject attacker)
    {
        _attacker = attacker;
    }

    int _itemIndex = 30;
    public void DropItem()
    {
        //_itemIndex = Random.Range(0, 3);
        ItemCreator.Instance.CreateItem(_itemIndex , _tileX, _tileY);
    }

    void ResetColor()
    {
        _characterView.GetComponent<SpriteRenderer>().color = Color.white;
    }
    //level
    int _level = 1;
    int _expPoint = 5;
    int _myExp = 0;
    public int GetEXP()
    {
        return _expPoint;
    }
    public void IncreaseEXP(int expPoint)
    {
        _myExp += expPoint;
        Debug.Log("exp Up!");
        if(10 <= _myExp)
        {
            _myExp = 0;
            //레벨 업 기능
            _hp = 100;
            _attackPoint *= 2;
            _level++;
            _textLevel.text = "level "+ _level;
        }
        _textExp.text = "exp " + _myExp;
    }

    //coolTime
    protected float _attackCoolTime = 1.5f;
    float _deltaAttackCoolTime = 0.0f;
    public bool IsAttackPossible()
    {
        if (_attackCoolTime <= _deltaAttackCoolTime)
        {
            return true;
        }
        return false;
    }
    void ResetAttackCooltime()
    {
        _deltaAttackCoolTime = 0.0f;
    }

    protected float _moveCoolTime = 0.3f;
    float _deltaMoveCoolTime = 0.0f;
    public bool IsMovePossible()
    {
        if (_moveCoolTime <= _deltaMoveCoolTime)
        {
            return true;
        }
        return false;
    }
    void ResetMoveCooltime()
    {
        _deltaMoveCoolTime = 0.0f;
    }

    //pathfinding
    private TileCell _targetTileCell;
    public void SetTargetTileCell(TileCell tileCell)
    {
        _targetTileCell = tileCell;
    }
    public TileCell GetTargetTileCell() { return _targetTileCell; }

    private Stack<TileCell> _pathfindingStack = new Stack<TileCell>();
    public void PushPathTileCell(TileCell tileCell)
    {
        if (null != tileCell) 
            _pathfindingStack.Push(tileCell);
    }
    public TileCell PopPathTileCell()
    {
        if(_pathfindingStack.Count>0)
            return _pathfindingStack.Pop();
        return null;
    }
    public bool IsEmptyPathfindingTileCell()
    {
        if (_pathfindingStack.Count > 0)
            return false;
        return true;
    }
    public void ClearPathfindingTileCell()
    {
        _pathfindingStack.Clear();
    }
    public Stack<TileCell> GetPathFindingStack() { return _pathfindingStack; }

    // UI
    Slider _hpGuage = null;
    public void LinkHPGuage(Slider hpGuage)
    {
        GameObject canvasObject = transform.Find("Canvas").gameObject;
        hpGuage.transform.SetParent(canvasObject.transform);
        hpGuage.transform.localPosition = Vector3.zero;
        hpGuage.transform.localScale = Vector3.one;

        _hpGuage = hpGuage;
        _hpGuage.value = _hp / 100.0f;
    }
    Slider _coolTimeGuage = null;
    public void LinkCoolTimeGuage(Slider coolTimeGuage)
    {
        GameObject canvasObject = transform.Find("Canvas").gameObject;
        coolTimeGuage.transform.SetParent(canvasObject.transform);
        coolTimeGuage.transform.localPosition = new Vector3(0.0f, -0.4f, 0.0f);
        coolTimeGuage.transform.localScale = Vector3.one;

        _coolTimeGuage = coolTimeGuage;
        _coolTimeGuage.value = _deltaAttackCoolTime / _attackCoolTime;
    }
    Text _textLevel = null;
    public void LinkTextLevel(Text text)
    {
        GameObject canvasObject = transform.Find("Canvas").gameObject;
        text.transform.SetParent(canvasObject.transform);
        text.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
        text.transform.localScale = Vector3.one;

        _textLevel = text;
    }
    Text _textExp = null;
    public void LinkTextEXP(Text text)
    {
        GameObject canvasObject = transform.Find("Canvas").gameObject;
        text.transform.SetParent(canvasObject.transform);
        text.transform.localPosition = new Vector3(0.0f, -0.8f, 0.0f);
        text.transform.localScale = Vector3.one;

        _textExp = text;
    }

    public void ShowMoveCursor(Vector3 vector3)
    {
        string filePath = "Prefabs/Effects/DamageEffect";
        GameObject effectPrefabs = Resources.Load<GameObject>(filePath);
        GameObject effectObject = GameObject.Instantiate(effectPrefabs, vector3, Quaternion.identity);
        GameObject.Destroy(effectObject, 0.5f);
    }

    //position
    public eMoveDirection GetDirection(sPosition curPosition, sPosition toPosition)
    {
        if (toPosition.x < curPosition.x) return eMoveDirection.LEFT;
        if (toPosition.x > curPosition.x) return eMoveDirection.RIGHT;
        if (toPosition.y < curPosition.y) return eMoveDirection.UP;
        if (toPosition.y > curPosition.y) return eMoveDirection.DOWN;
        return eMoveDirection.DOWN;
    }
}