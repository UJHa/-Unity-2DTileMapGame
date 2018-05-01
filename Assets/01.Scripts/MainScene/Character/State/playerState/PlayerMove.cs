using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MoveState
{
    override protected void MoveFinish()
    {
        _nextState = eStateType.SELECT;
    }
}
