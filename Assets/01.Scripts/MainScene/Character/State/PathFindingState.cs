using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingState : State
{
    //enum eFindState
    //{
    //    PATHFINDING,
    //    BUILD_PATH,
    //}
    struct sPosition
    {
        public int x;
        public int y;
    }
    Queue<TileCell> _pathfindingQueue = new Queue<TileCell>();
    //eFindState _findState;
    //TileCell _reverseTileCell;
    override public void Start () {
        base.Start();
        //_findState = eFindState.PATHFINDING;
        //_reverseTileCell = null;
        TileMap map = GameManager.Instance.GetMap();
        if (null != _character.GetTargetTileCell())
        {
            map.ResetVisit();
        }
        else
        {
            _nextState = eStateType.IDLE;
        }

        //첫 큐에 넣는 출발 셀 넣기
        TileCell startTileCell = map.GetTileCell(_character.GetTileX(), _character.GetTileY());
        startTileCell.SetPrevTileCell(null);
        _pathfindingQueue.Enqueue(startTileCell);

    }
    // Update is called once per frame
    override public void Update () {
        base.Update();
        UpdatePathFinding();
        //switch (_findState)
        //{
        //    case eFindState.PATHFINDING:
        //        UpdatePathFinding();
        //        break;
        //    case eFindState.BUILD_PATH:
        //        UpdateBuildPath();
        //        break;
        //}
    }
    public override void Stop()
    {
        base.Stop();
        _pathfindingQueue.Clear();
        //_findState = eFindState.PATHFINDING;
        //_character.SetTargetTileCell(null);
    }
    private sPosition GetPositionByDirection(sPosition curPosition, int direction)
    {
        sPosition position = curPosition;
        eMoveDirection moveDirection = (eMoveDirection)direction;
        switch (moveDirection)
        {
            case eMoveDirection.LEFT:
                position.x--;
                if(position.x < 0)
                    position.x++;
                break;
            case eMoveDirection.RIGHT:
                position.x++;
                if (position.x == GameManager.Instance.GetMap().GetWidth())
                    position.x--;
                break;
            case eMoveDirection.UP:
                position.y--;
                if (position.y < 0)
                    position.y++;
                break;
            case eMoveDirection.DOWN:
                position.y++;
                if (position.y == GameManager.Instance.GetMap().GetHeight())
                    position.y--;
                break;
        }
        return position;
    }
    private void UpdatePathFinding()
    {
        if (0 != _pathfindingQueue.Count)
        {
            //큐의 첫번째 커맨드 가져옴
            TileCell tileCell = _pathfindingQueue.Dequeue();
            //가져온 커맨드의 현재 타일셀 방문 표시
            if (false == tileCell.IsVisited())
            {
                tileCell.SetVisit(true);
                tileCell.Draw(Color.blue);

                //가져온 커맨드의 현재 타일셀이 목표 타일일 경우 nextState 변경
                if (_character.GetTargetTileCell() == tileCell)
                {
                    Debug.Log("찾았어양");
                    _character.SetTargetTileCell(tileCell);
                    //_findState = eFindState.BUILD_PATH;
                    _nextState = eStateType.BUILD_PATH;
                    return;
                }
                int tileX = tileCell.GetTileX();
                int tileY = tileCell.GetTileY();
                //4방향 next타일들 검사
                for (int direction = (int)eMoveDirection.LEFT; direction < (int)eMoveDirection.DOWN + 1; direction++)
                {
                    sPosition curPosition;
                    curPosition.x = tileCell.GetTileX();
                    curPosition.y = tileCell.GetTileY();
                    sPosition nextPosition = GetPositionByDirection(curPosition, direction);

                    TileMap map = GameManager.Instance.GetMap();
                    TileCell nextTileCell = map.GetTileCell(nextPosition.x, nextPosition.y);
                    tileX = tileCell.GetTileX();
                    tileY = tileCell.GetTileY();

                    // nextTileCell 방문 안했고, 움직일수 있는 타일일때
                    if (false == nextTileCell.IsVisited() && true == nextTileCell.CanMove())
                    {
                        float distance = tileCell.GetDistanceFromStart() + nextTileCell.GetDistanceFromStart();
                        nextTileCell.SetDistanceFromStart(distance);

                        nextTileCell.SetPrevTileCell(tileCell);
                        _pathfindingQueue.Enqueue(nextTileCell);
                    }
                }
            }
        }
    }
    //private void UpdateBuildPath()
    //{
    //    if (null != _reverseTileCell.GetPrevTileCell())
    //    {
    //        _character.PushPathTileCell(_reverseTileCell);
    //        _reverseTileCell.Draw(Color.white);
    //        _reverseTileCell = _reverseTileCell.GetPrevTileCell();
    //    }
    //    else
    //    {
    //        _nextState = eStateType.MOVE;
    //    }
    //}
}