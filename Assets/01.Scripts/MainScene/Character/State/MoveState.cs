﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    override public void Update()
    {
        base.Update();
    }

    override public void Start()
    {
        base.Start();

        int moveX = _character.GetTileX();
        int moveY = _character.GetTileY();
        switch (_character.GetNextDirection())
        {
            case eMoveDirection.LEFT: moveX--; break;
            case eMoveDirection.RIGHT: moveX++; break;
            case eMoveDirection.UP: moveY--; break;
            case eMoveDirection.DOWN: moveY++; break;
        }

        if (false == _character.MoveStart(moveX, moveY))
        {
            if (_character.IsAttackPossible())
            {
                _nextState = eStateType.ATTACK;
            }
            else
            {
                _character.SetNextDirection(eMoveDirection.NONE);
                _nextState = eStateType.IDLE;
            }
        }
        else
        {
            _character.SetNextDirection(eMoveDirection.NONE);
            _nextState = eStateType.IDLE;
        }
    }
}