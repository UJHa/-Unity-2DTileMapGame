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
        if(false == _character.IsEmptyPathfindingTileCell())
        {
            TileCell nextTileCell = _character.PopPathTileCell();

            sPosition curPosition;
            curPosition.x = _character.GetTileX();
            curPosition.y = _character.GetTileY();

            sPosition toPosition;
            toPosition.x = nextTileCell.GetTileX();
            toPosition.y = nextTileCell.GetTileY();

            eMoveDirection direction = GetDirection(curPosition, toPosition);
            _character.SetNextDirection(direction);
            _character.MoveStart(nextTileCell.GetTileX(), nextTileCell.GetTileY());
            //if ()
            //{
            //    _character.SetNextDirection(eMoveDirection.NONE);
            //}
        }
        else
        {
            _nextState = eStateType.IDLE;
        }
    }
    private eMoveDirection GetDirection(sPosition curPosition, sPosition toPosition)
    {
        if (toPosition.x < curPosition.x) return eMoveDirection.LEFT;
        if (toPosition.x > curPosition.x) return eMoveDirection.RIGHT;
        if (toPosition.y < curPosition.y) return eMoveDirection.UP;
        if (toPosition.y > curPosition.y) return eMoveDirection.DOWN;
        return eMoveDirection.DOWN;
    }
    //private eMoveDirection GetMoveDirection(TileCell nextTileCell)
    //{
    //    if (_character.GetTileX() == nextTileCell.GetTileX())
    //    {
    //        if (_character.GetTileY() < nextTileCell.GetTileY())
    //        {
    //            return eMoveDirection.DOWN;
    //        }
    //        else if (_character.GetTileY() > nextTileCell.GetTileY())
    //        {
    //            return eMoveDirection.UP;
    //        }
    //    }
    //    if (_character.GetTileY() == nextTileCell.GetTileY())
    //    {
    //        if (_character.GetTileX() < nextTileCell.GetTileX())
    //        {
    //            return eMoveDirection.RIGHT;
    //        }
    //        else if (_character.GetTileX() > nextTileCell.GetTileX())
    //        {
    //            return eMoveDirection.LEFT;
    //        }
    //    }
    //    return eMoveDirection.NONE;
    //}
}