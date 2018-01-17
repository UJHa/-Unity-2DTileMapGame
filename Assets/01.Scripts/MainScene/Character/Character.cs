using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eMoveDirection
{
    NONE,
    LEFT,
    RIGHT,
    UP,
    DOWN,
}
public class Character : MapObject {


    protected GameObject _characterView;
    protected int _tileX = 0;
    protected int _tileY = 0;

    protected int _hp = 100;
    protected bool _isLive = true;
    protected int _attackPoint = 10;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Init(string viewName)
    {
        //View를 붙인다.
        string filePath = "Prefabs/CharacterView/" + viewName;
        GameObject characterViewPrefabs = Resources.Load<GameObject>(filePath);
        _characterView = GameObject.Instantiate(characterViewPrefabs);
        _characterView.transform.SetParent(transform);
        _characterView.transform.localPosition = Vector3.zero;
        _characterView.transform.localScale = Vector3.one;

        TileMap map = GameManager.Instance.GetMap();
        _tileX = Random.Range(1, map.GetWidth() - 2);
        _tileY = Random.Range(1, map.GetHeight() - 2);
        //TileCell tileCell = map.GetTileCell(_tileX, _tileY);
        //tileCell.AddObject(eTileLayer.MIDDLE, this);
        map.SetObject(_tileX, _tileY, this, eTileLayer.MIDDLE);

        SetCanMove(false);

        InitState();
    }
    public int GetTileX() { return _tileX; }
    public int GetTileY() { return _tileY; }

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
    void InitState()
    {
        //_stateMap[eStateType.IDLE] = new IdleState();
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
        _state = _stateMap[eStateType.IDLE];
    }
    public void ChangeState(eStateType nextState)
    {
        if(null != _state)
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
                //Debug.Log("Attacked : " + msgParam.attackPoint);
                Damaged(msgParam.attackPoint);
                break;
        }
    }

    // Actions
    public bool MoveStart(int moveX, int moveY)
    {
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
        if (0 == collisionList.Count)  //이동 가능할때
        {
            map.ResetObject(_tileX, _tileY, this);
            _tileX = moveX;
            _tileY = moveY;
            map.SetObject(_tileX, _tileY, this, eTileLayer.MIDDLE);
            return true;
        }
        return false;
    }
    void Damaged(int attackEnemyPoint)
    {
        _hp -= attackEnemyPoint;
        if (_hp <= 0)
        {
            _isLive = false;
            _canMove = true;
            Debug.Log("Death!");
            _hp = 0;
        }
    }
    public void Attack(MapObject enemy)
    {
        MessageParam msgParam = new MessageParam();
        msgParam.sender = this;
        msgParam.receiver = enemy;
        msgParam.attackPoint = _attackPoint;
        msgParam.message = "Attack";

        MessageSystem.Instance.Send(msgParam);
    }
}
