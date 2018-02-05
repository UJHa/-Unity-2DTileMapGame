using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    void Start()
    {
        _type = eMapObjectType.MONSTER;
        _attackCoolTime = 2.0f;
        _moveCoolTime = 2.0f;
    }
    override protected void InitState()
    {
        base.InitState();
        {
            State state = new MonsterIdleState();
            state.Init(this);
            _stateMap[eStateType.IDLE] = state;
        }
        {
            State state = new BattleState();
            state.Init(this);
            _stateMap[eStateType.BATTLE] = state;
        }
        _state = _stateMap[eStateType.IDLE];
    }
}