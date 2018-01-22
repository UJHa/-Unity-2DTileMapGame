using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    // Use this for initialization
    void Start()
    {
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
            State state = new PathFindingTestMove();
            state.Init(this);
            _stateMap[eStateType.MOVE] = state;
        }
        _state = _stateMap[eStateType.IDLE];
    }
}