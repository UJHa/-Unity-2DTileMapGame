using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    override public void Update()
    {
        base.Update();
        eMoveDirection moveDirection = eMoveDirection.NONE;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDirection = eMoveDirection.LEFT;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDirection = eMoveDirection.RIGHT;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDirection = eMoveDirection.UP;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDirection = eMoveDirection.DOWN;
        }

        if (eMoveDirection.NONE != moveDirection)
        {
            _character.SetNextDirection(moveDirection);
            _nextState = eStateType.MOVE;
        }
    }
}
