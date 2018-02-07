using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    void Start()
    {
        _attackCoolTime = 0.1f;
        _moveCoolTime = 0.1f;
    }
    override protected void InitState()
    {
        base.InitState();
        {
            State state = new PathFindingIdle();
            state.Init(this);
            _stateMap[eStateType.IDLE] = state;
        }
        {
            State state = new PathFindingImmidiateState();
            state.Init(this);
            _stateMap[eStateType.PATHFINDING] = state;
        }
        {
            State state = new PathfindingBuildState();
            state.Init(this);
            _stateMap[eStateType.BUILD_PATH] = state;
        }
        {
            State state = new PathFindingTestMove();
            state.Init(this);
            _stateMap[eStateType.MOVE] = state;
        }
        {
            State state = new BattleState();
            state.Init(this);
            _stateMap[eStateType.BATTLE] = state;
        }
        _state = _stateMap[eStateType.IDLE];
        _state.Start();
    }
}