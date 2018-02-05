using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : State
{
    override public void Update()
    {
        base.Update();
        if (false == _character.IsMovePossible())
            return;
        eMoveDirection moveDirection = eMoveDirection.NONE;
        moveDirection = (eMoveDirection)Random.Range(0, (int)eMoveDirection.DOWN + 1);

        if (eMoveDirection.NONE != moveDirection)
        {
            _character.SetNextDirection(moveDirection);
            _nextState = eStateType.MOVE;
        }
    }
}