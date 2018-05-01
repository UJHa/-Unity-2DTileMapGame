using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected Stack<TileCell> _pathfindingStack = new Stack<TileCell>();
    override public void Start()
    {
        base.Start();
        TileCell pathTileCell = _character.GetTargetTileCell();
        while (null != pathTileCell.GetPrevTileCell())
        {
            _pathfindingStack.Push(pathTileCell);

            pathTileCell = pathTileCell.GetPrevTileCell();
        }
        Debug.Log(_character + " : move");

        Debug.Log(_character.IsMovePossible());
        Debug.Log(_character.GetMoveDuration());
    }
    override public void Update()
    {
        base.Update();
        UpdateMove();
    }
    void UpdateMove()
    {
        _character.UpdateMoveCooltime();
        if (null == _nextTileCell)
        {
            if (0 != _pathfindingStack.Count)
            {
                _nextTileCell = _pathfindingStack.Pop();
                sPosition curPosition;
                curPosition.x = _character.GetTileX();
                curPosition.y = _character.GetTileY();

                sPosition toPosition;
                toPosition.x = _nextTileCell.GetTileX();
                toPosition.y = _nextTileCell.GetTileY();

                eMoveDirection direction = _character.GetDirection(curPosition, toPosition);
                _character.SetNextDirection(direction);
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
                return;
            }
        }
        if (_character.IsMovePossible())
        {
            MoveNextTile();
        }
        else
        {
            MoveInterpolation();  //보간
        }
    }
    public override void Stop()
    {
        base.Stop();
        _character.SetTargetTileCell(null);
        _pathfindingStack.Clear();
        _nextTileCell = null;
    }
    TileCell _nextTileCell = null;
    virtual protected void MoveNextTile()
    {
        if (!_character.MoveStart(_nextTileCell.GetTileX(), _nextTileCell.GetTileY()))
        {
            _nextState = eStateType.IDLE;
        }
    }
    void MoveInterpolation()
    {
        eMoveDirection moveDirection = _character.GetNextDirection();
        switch (moveDirection)
        {
            case eMoveDirection.LEFT:
                _character.SetPosition(_character.GetTransform().position -  new Vector3(-0.1f, 0.0f,0.0f));
                break;
            case eMoveDirection.RIGHT:
                _character.SetPosition(_character.GetTransform().position - new Vector3(0.1f, 0.0f, 0.0f));
                break;
            case eMoveDirection.UP:
                _character.SetPosition(_character.GetTransform().position - new Vector3(0.0f, -0.1f, 0.0f));
                break;
            case eMoveDirection.DOWN:
                _character.SetPosition(_character.GetTransform().position - new Vector3(0.0f, -0.1f, 0.0f));
                break;
            default:
                break;
        }
    }
}