using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected Stack<TileCell> _pathfindingStack = new Stack<TileCell>();
    Vector3 _characterVector;
    eMoveDirection _direction = eMoveDirection.NONE;
    TileCell _nextTileCell = null;
    enum eMoveState
    {
        START,
        MOVE,
        END,
    }
    eMoveState _eMoveState;
    override public void Start()
    {
        base.Start();
        _eMoveState = eMoveState.START;
        TileCell pathTileCell = _character.GetTargetTileCell();
        while (null != pathTileCell.GetPrevTileCell(_character))
        {
            _pathfindingStack.Push(pathTileCell);

            pathTileCell = pathTileCell.GetPrevTileCell(_character);
        }
        _characterVector = _character.GetPosition();
        Debug.Log(_character + " : move");
    }
    override public void Update()
    {
        base.Update();
        _character.UpdateMoveCooltime();
        UpdateMove();
    }
    void UpdateMove()
    {
        if (_character.IsMovePossible())
        {
            _eMoveState = eMoveState.END;
        }
        switch (_eMoveState)
        {
            case eMoveState.START:
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
                _eMoveState = eMoveState.MOVE;
                break;
            case eMoveState.MOVE:
                MoveInterpolation();  //보간
                break;
            case eMoveState.END:
                _character.MoveTileCell(_nextTileCell);
                _characterVector = _character.GetPosition();
                _eMoveState = eMoveState.START;
                break;
            default:
                break;
        }
    }
    override public void Stop()
    {
        base.Stop();
        _character.SetTargetTileCell(null);
        _pathfindingStack.Clear();
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