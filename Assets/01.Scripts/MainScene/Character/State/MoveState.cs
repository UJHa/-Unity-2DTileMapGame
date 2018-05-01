using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected Stack<TileCell> _pathfindingStack = new Stack<TileCell>();
    Vector3 _characterVector;
    eMoveDirection _direction = eMoveDirection.NONE;
    override public void Start()
    {
        base.Start();
        TileCell pathTileCell = _character.GetTargetTileCell();
        while (null != pathTileCell.GetPrevTileCell())
        {
            _pathfindingStack.Push(pathTileCell);

            pathTileCell = pathTileCell.GetPrevTileCell();
        }
        if (0 != _pathfindingStack.Count)
        {
            _nextTileCell = _pathfindingStack.Pop();
            sPosition curPosition;
            curPosition.x = _character.GetTileX();
            curPosition.y = _character.GetTileY();

            sPosition toPosition;
            toPosition.x = _nextTileCell.GetTileX();
            toPosition.y = _nextTileCell.GetTileY();

            _direction = _character.GetDirection(curPosition, toPosition);
            _character.SetAnimation(_direction.ToString().ToLower());
        }
        _characterVector = _character.GetTransform().position;
        Debug.Log(_character + " : move");
    }
    override public void Update()
    {
        base.Update();
        UpdateMove();
    }
    void UpdateMove()
    {
        //_character.UpdateMoveCooltime();
        //if (0 != _pathfindingStack.Count)
        //{
        //    _nextTileCell = _pathfindingStack.Pop();
        //    sPosition curPosition;
        //    curPosition.x = _character.GetTileX();
        //    curPosition.y = _character.GetTileY();

        //    sPosition toPosition;
        //    toPosition.x = _nextTileCell.GetTileX();
        //    toPosition.y = _nextTileCell.GetTileY();

        //    eMoveDirection direction = _character.GetDirection(curPosition, toPosition);
        //    //_character.SetNextDirection(direction);
        //    _character.SetAnimation(direction.ToString().ToLower());
        //}
        //else
        //{
        //    //상태 변경
        //    MoveFinish();
        //    return;
        //}
        //if (_character.IsMovePossible())
        //{
        //    MoveNextTile();
        //}
        //else
        //{
        //    MoveInterpolation();  //보간
        //}

        _character.UpdateMoveCooltime();
        if (_character.IsMovePossible())
        {
            //MoveNextTile();
            _characterVector = _character.GetTransform().position;
            if (!_character.MoveStart(_nextTileCell.GetTileX(), _nextTileCell.GetTileY()))
            {
                MoveFinish();
            }
            if (0 != _pathfindingStack.Count)
            {
                _nextTileCell = _pathfindingStack.Pop();
                sPosition curPosition;
                curPosition.x = _character.GetTileX();
                curPosition.y = _character.GetTileY();

                sPosition toPosition;
                toPosition.x = _nextTileCell.GetTileX();
                toPosition.y = _nextTileCell.GetTileY();

                _direction = _character.GetDirection(curPosition, toPosition);
                _character.SetAnimation(_direction.ToString().ToLower());
            }
            else
            {
                //상태 변경
                MoveFinish();
                return;
            }
        }
        else
        {
            MoveInterpolation();  //보간
        }
    }
    override public void Stop()
    {
        base.Stop();
        _character.SetTargetTileCell(null);
        _pathfindingStack.Clear();
    }
    TileCell _nextTileCell = null;
    void MoveNextTile()
    {
        if (!_character.MoveStart(_nextTileCell.GetTileX(), _nextTileCell.GetTileY()))
        {
            MoveFinish();
        }
        _characterVector = _character.GetTransform().position;
        _character.SetPosition(_nextTileCell.GetPosition());
    }
    void MoveInterpolation()
    {
        TileMap map = GameManager.Instance.GetMap();
        float deltaMoveSpeed = map.GetTileSize() * _character.GetDeltaMoveRate();
        Vector3 deltaMoveSpeedVector = Vector3.zero;
        switch (_direction)
        {
            case eMoveDirection.LEFT:
                _character.SetPosition(_characterVector + new Vector3(-deltaMoveSpeed, 0.0f, 0.0f));
                break;
            case eMoveDirection.RIGHT:
                _character.SetPosition(_characterVector + new Vector3(deltaMoveSpeed, 0.0f, 0.0f));
                break;
            case eMoveDirection.UP:
                _character.SetPosition(_characterVector + new Vector3(0.0f, deltaMoveSpeed, 0.0f));
                break;
            case eMoveDirection.DOWN:
                _character.SetPosition(_characterVector + new Vector3(0.0f, -deltaMoveSpeed, 0.0f));
                break;
            default:
                break;
        }
    }
    virtual protected void MoveFinish()
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