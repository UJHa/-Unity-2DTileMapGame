using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingTestMove : State
{
    override public void Start()
    {
        base.Start();
        Debug.Log("Move!");
        _character.GetTargetTileCell();
        _nextState = eStateType.IDLE;
    }
    override public void Update()
    {
        base.Update();
    }
}