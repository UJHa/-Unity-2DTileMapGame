using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    Stack<TileCell> _pathfindingStack = new Stack<TileCell>();
    override public void Start()
    {
        base.Start();
        TileCell pathTileCell = _character.GetTargetTileCell();
        while (null != pathTileCell.GetPrevTileCell())
        {
            _pathfindingStack.Push(pathTileCell);

            pathTileCell = pathTileCell.GetPrevTileCell();
        }
    }
    override public void Update()
    {
        base.Update();
        if (_character.IsMovePossible())
        {
            UpdateMove();
        }
    }
    void UpdateMove()
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

            _character.MoveStart(nextTileCell.GetTileX(), nextTileCell.GetTileY());
        }
        else
        {
            //int selectState = Random.Range(0, 2);
            //if (0 == selectState)
            //{
            //    _nextState = eStateType.IDLE;
            //}
            //else if (0 == selectState)
            {
                _nextState = eStateType.ATTACK;
            }

        }
    }
    public override void Stop()
    {
        base.Stop();
        _character.SetTargetTileCell(null);
        _pathfindingStack.Clear();
    }
}