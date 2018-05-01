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
        if (eStateType.NONE != _state.GetNextState() && _state.GetNextState() != _state.GetNowState())
        {
            ChangeState(_state.GetNextState());
        }
        if(_hp <= 0)
        {
            _state.NextState(eStateType.DEATH);
        }


        _state.Update();
        UpdateActionCooltime();
        //UpdateCooltime();

        UpdateGuage();
    }
    void UpdateActionCooltime()
    {
        if (_actionCooltime <= _deltaActionCooltime)
            _deltaActionCooltime = _actionCooltime;
        else
            _deltaActionCooltime += Time.deltaTime;
    }
    public void UpdateMoveCooltime()
    {
        if (_moveCooltime <= _deltaMoveCooltime)
        {

        }
        else
        {
            _deltaMoveCooltime += Time.deltaTime;
        }
    }
    void UpdateGuage()
    {
        if (null == _hpGuage || null == _cooltimeGuage)
            return;
        _hpGuage.value = _hp / 100.0f;
        _cooltimeGuage.value = _deltaActionCooltime / _actionCooltime;
    }
    GameObject _bulletDirection;
    virtual public void Init(string viewName)
    {
        //View를 붙인다.
        string filePath = "Prefabs/CharacterView/" + viewName;
        GameObject characterViewPrefabs = Resources.Load<GameObject>(filePath);
        _characterView = GameObject.Instantiate(characterViewPrefabs);
        _characterView.transform.SetParent(transform);
        _characterView.transform.localPosition = Vector3.zero;
        _characterView.transform.localScale = GameManager.Instance.GetMap().GetLocalScale();
        SetAnimation("idle");

        GameObject bulletDirectionPrefabs = Resources.Load<GameObject>("Prefabs/DirectionUI/BulletDirection");
        _bulletDirection = GameObject.Instantiate(bulletDirectionPrefabs);
        _bulletDirection.transform.SetParent(transform);
        _bulletDirection.transform.localPosition = Vector3.zero;
        _bulletDirection.transform.localScale = GameManager.Instance.GetMap().GetLocalScale();
        _bulletDirection.SetActive(false);

        InitPosition();
        InitState();
    }

    virtual protected void InitPosition()
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



    //eMoveDirection _nextDirection = eMoveDirection.NONE;
    //public eMoveDirection GetNextDirection() { return _nextDirection; }
    //public void SetNextDirection(eMoveDirection direction) { _nextDirection = direction; }

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
        _state.Start();
    }
    void ChangeState(eStateType nextState)
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
                //SetNextDirection(direction);
                MoveStart(attackedPosition.x, attackedPosition.y);
                _state.NextState(eStateType.DAMAGE);
                break;
            case "IsDead":
                Debug.Log("I'm dead!");
                Character msgSender = (Character)msgParam.sender;
                IncreaseEXP(msgSender.GetEXP());
                break;
        }
    }

    // Actions
    public bool MoveStart(int moveX, int moveY)
    {
        ResetMoveCooltime();

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
        //ResetAttackCooltime();
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
        //_textExp.text = "exp " + _myExp;
    }

    //cooltime
    protected float _actionCooltime = 1.0f;
    float _deltaActionCooltime = 0.0f;
    public bool IsActionPossible()
    {
        if (_actionCooltime <= _deltaActionCooltime)
        {
            return true;
        }
        return false;
    }
    public void ResetActionCooltime()
    {
        _deltaActionCooltime = 0.0f;
    }
    public void SetActionCooltime(float second)
    {
        _actionCooltime = second;
        _deltaActionCooltime = second;
    }

    protected float _moveCooltime = 1.0f;
    float _deltaMoveCooltime = 0.0f;
    public bool IsMovePossible()
    {
        //Debug.Log(_moveCooltime + ", " + _deltaMoveCooltime + ":" + (_moveCooltime <= _deltaMoveCooltime));
        if (_moveCooltime <= _deltaMoveCooltime)
        {
            return true;
        }
        return false;
    }
    void ResetMoveCooltime()
    {
        _deltaMoveCooltime = 0.0f;
    }
    public float GetDeltaMoveRate()
    {
        return _deltaMoveCooltime / _moveCooltime;
    }

    //pathfinding
    private TileCell _targetTileCell;
    public void SetTargetTileCell(TileCell tileCell)
    {
        _targetTileCell = tileCell;
    }
    public TileCell GetTargetTileCell() { return _targetTileCell; }

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
    Slider _cooltimeGuage = null;
    public void LinkActionCooltimeGuage(Slider cooltimeGuage)
    {
        GameObject canvasObject = transform.Find("Canvas").gameObject;
        cooltimeGuage.transform.SetParent(canvasObject.transform);
        cooltimeGuage.transform.localPosition = new Vector3(0.0f, -0.4f, 0.0f);
        cooltimeGuage.transform.localScale = Vector3.one;

        _cooltimeGuage = cooltimeGuage;
        _cooltimeGuage.value = _deltaActionCooltime / _actionCooltime;
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

    Button _atkButton = null;
    public void LinkAtkButton(Button button)
    {
        GameObject canvasObject = transform.Find("Canvas").gameObject;
        button.transform.SetParent(canvasObject.transform);
        button.transform.localPosition = new Vector3(-1.6f, -2.0f, 0.0f);
        button.transform.localScale = Vector3.one;

        _atkButton = button;
        _atkButton.onClick.AddListener(IsAtkClick);
    }

    Button _waitButton = null;
    public void LinkWaitButton(Button button)
    {
        GameObject canvasObject = transform.Find("Canvas").gameObject;
        button.transform.SetParent(canvasObject.transform);
        button.transform.localPosition = new Vector3(1.6f, -2.0f, 0.0f);
        button.transform.localScale = Vector3.one;

        _waitButton = button;
        _waitButton.onClick.AddListener(IsWaitClick);
    }
    public void SetCanvasLayer(eTileLayer layer)
    {
        GameObject canvasObject = transform.Find("Canvas").gameObject;
        int sortingID = SortingLayer.NameToID(layer.ToString());
        canvasObject.GetComponent<Canvas>().sortingLayerID = sortingID;
    }
    public void SetSelectUI(bool isActive)
    {
        _atkButton.gameObject.SetActive(isActive);
        _waitButton.gameObject.SetActive(isActive);
    }
    bool _atkClicked = false;
    bool _waitClicked = false;
    public bool IsClickAtkButton()
    {
        if (_atkClicked)
        {
            _atkClicked = false;
            return true;
        }
        return false;
    }

    public bool IsClickWaitButton()
    {
        if (_waitClicked)
        {
            _waitClicked = false;
            return true;
        }
        return false;
    }
    void IsAtkClick()
    {
        _atkClicked = true;
    }
    void IsWaitClick()
    {
        _waitClicked = true;
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
    public sPosition GetPositionByDirection(sPosition curPosition, int direction)
    {
        sPosition position = curPosition;
        eMoveDirection moveDirection = (eMoveDirection)direction;
        switch (moveDirection)
        {
            case eMoveDirection.LEFT:
                position.x--;
                if (position.x < 0)
                    position.x++;
                break;
            case eMoveDirection.RIGHT:
                position.x++;
                if (position.x == GameManager.Instance.GetMap().GetWidth())
                    position.x--;
                break;
            case eMoveDirection.UP:
                position.y--;
                if (position.y < 0)
                    position.y++;
                break;
            case eMoveDirection.DOWN:
                position.y++;
                if (position.y == GameManager.Instance.GetMap().GetHeight())
                    position.y--;
                break;
        }
        return position;
    }
    public Transform GetTransform()
    {
        return gameObject.transform;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if(bullet!=null)
        {
            if (gameObject.name != bullet.GetShooterName())
            {   
                Destroy(collision.gameObject);
                _hp -= 50;
            }
        }
    }
    //animation
    public void SetAnimation(string animationTrigger)
    {
        _characterView.GetComponent<Animator>().SetTrigger(animationTrigger);
    }

    //direction
    public GameObject GetDirectionUI()
    {
        return _bulletDirection;
    }
}