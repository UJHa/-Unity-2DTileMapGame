using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MoveState
{
    override protected void MoveNextTile()
    {
        if (0 != _pathfindingStack.Count)
        {
            TileCell nextTileCell = _pathfindingStack.Pop();

            sPosition curPosition;
            curPosition.x = _character.GetTileX();
            curPosition.y = _character.GetTileY();

            sPosition toPosition;
            toPosition.x = nextTileCell.GetTileX();
            toPosition.y = nextTileCell.GetTileY();

            eMoveDirection direction = _character.GetDirection(curPosition, toPosition);
            _character.SetNextDirection(direction);
            if (!_character.MoveStart(nextTileCell.GetTileX(), nextTileCell.GetTileY()))
            {
                _nextState = eStateType.SELECT;
            }
        }
        else
        {
            _nextState = eStateType.SELECT;
        }
    }
}
