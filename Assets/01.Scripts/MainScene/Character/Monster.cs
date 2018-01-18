using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    // Use this for initialization
    void Start()
    {
        _type = eMapObjectType.MONSTER;
    }

    //void Update()
    //{
    //    if (false == _isLive)
    //        return;
    //}
    override protected void InitState()
    {
        base.InitState();
        {
            State state = new MonsterIdleState();
            state.Init(this);
            _stateMap[eStateType.IDLE] = state;
        }
        _state = _stateMap[eStateType.IDLE];
    }
}