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
            TileCell moveTileCell = _character.PopPathTileCell();
            _character.SetNextDirection(GetMoveDirection(moveTileCell));
            _character.MoveStart(moveTileCell.GetTileX(), moveTileCell.GetTileY());
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
    private eMoveDirection GetMoveDirection(TileCell nextTileCell)
    {
        if (_character.GetTileX() == nextTileCell.GetTileX())
        {
            if (_character.GetTileY() < nextTileCell.GetTileY())
            {
                return eMoveDirection.DOWN;
            }
            else if (_character.GetTileY() > nextTileCell.GetTileY())
            {
                return eMoveDirection.UP;
            }
        }
        if (_character.GetTileY() == nextTileCell.GetTileY())
        {
            if (_character.GetTileX() < nextTileCell.GetTileX())
            {
                return eMoveDirection.RIGHT;
            }
            else if (_character.GetTileX() > nextTileCell.GetTileX())
            {
                return eMoveDirection.LEFT;
            }
        }
        return eMoveDirection.NONE;
    }
}