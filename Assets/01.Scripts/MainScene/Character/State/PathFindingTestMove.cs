using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingTestMove : State
{
    Stack<TileCell> _pathfindingStack;
    override public void Start()
    {
        base.Start();
        Debug.Log("움직양!");
        _pathfindingStack = _character.GetPathFindingStack();
    }
    override public void Update()
    {
        base.Update();
        if(_pathfindingStack.Count > 0)
        {
            TileCell moveTileCell = _pathfindingStack.Pop();
            _character.SetNextDirection(GetMoveDirection(moveTileCell));
            if (_character.MoveStart(moveTileCell.GetTileX(), moveTileCell.GetTileY()))
            {
                _character.SetNextDirection(eMoveDirection.NONE);
            }
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