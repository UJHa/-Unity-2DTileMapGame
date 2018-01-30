using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingTestMove : State
{
    override public void Start()
    {
        base.Start();
        Debug.Log("움직양!");
    }
    override public void Stop()
    {
        base.Stop();
        _character.ClearPathfindingTileCell();
        Debug.Log("멈춰양!");
    }
    override public void Update()
    {
        base.Update();
        if(_character.IsMovePossible())
        {
            UpdateMove();
        }
    }
    private void UpdateMove()
    {
        if(false == _character.IsEmptyPathfindingTileCell())
        {
            TileCell nextTileCell = _character.PopPathTileCell();

            sPosition curPosition;
            curPosition.x = _character.GetTileX();
            curPosition.y = _character.GetTileY();

            sPosition toPosition;
            toPosition.x = nextTileCell.GetTileX();
            toPosition.y = nextTileCell.GetTileY();

            eMoveDirection direction = _character.GetDirection(curPosition, toPosition);
            _character.SetNextDirection(direction);
            if (false == _character.MoveStart(nextTileCell.GetTileX(), nextTileCell.GetTileY()))
            {
                _nextState = eStateType.BATTLE;
            }
        }
        else
        {
            _nextState = eStateType.IDLE;
        }
    }
}