using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    void Start()
    {
        //_attackCoolTime = 0.1f;
        _moveCoolTime = 0.1f;
    }
    override protected void InitState()
    {
        base.InitState();
        {
            //State state = new PathFindingIdle();
            State state = new PlayerIdle();
            state.Init(this);
            _stateMap[eStateType.IDLE] = state;
        }
        {
            State state = new PlayerMove();
            state.Init(this);
            _stateMap[eStateType.MOVE] = state;
        }
        {
            State state = new PlayerSelect();
            state.Init(this);
            _stateMap[eStateType.SELECT] = state;
        }
        {
            State state = new PlayerAttack();
            state.Init(this);
            _stateMap[eStateType.ATTACK] = state;
        }
        _state = _stateMap[eStateType.IDLE];
        _state.Start();
    }
    protected override void InitPosition()
    {
        TileMap map = GameManager.Instance.GetMap();
        _tileX = 5;
        _tileY = 5;
        map.SetObject(_tileX, _tileY, this, eTileLayer.MIDDLE);

        SetCanMove(false);
    }
}