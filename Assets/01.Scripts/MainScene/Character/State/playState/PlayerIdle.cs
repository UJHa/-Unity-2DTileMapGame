using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : State
{
    List<sTileHeuristicInfo> _moveRangeQueue = new List<sTileHeuristicInfo>();
    List<TileCell> _movePossibleTileCellList = new List<TileCell>();
    protected struct sTileHeuristicInfo
    {
        public TileCell tileCell;
        public float heuristic;
    }
    override public void Start()
    {
        base.Start();
        Debug.Log("idle start!");
        DrawMoveRange();
        Debug.Log(_movePossibleTileCellList.Count);
    }
    override public void Update()
    {
        base.Update();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            int targetTileX = hit.transform.GetComponent<MapObject>().GetTileX();
            int targetTileY = hit.transform.GetComponent<MapObject>().GetTileY();

            TileCell target = GameManager.Instance.GetMap().GetTileCell(targetTileX, targetTileY);
            if (_movePossibleTileCellList.Contains(target))
            {
                _character.SetTargetTileCell(target);
                DrawMoveRange();
                DrawMovePath(target);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!(_character.GetTileX() == targetTileX && _character.GetTileY() == targetTileY))    //player위치 제외한 타일셀 클릭 시
                {
                    _character.ShowMoveCursor(hit.transform.GetComponent<MapObject>().transform.position);
                    if (_movePossibleTileCellList.Contains(target))
                    //if (target.IsPathfindable())
                    {
                        _character.SetTargetTileCell(target);
                        Debug.Log("이동가능 타일!");
                        _nextState = eStateType.BUILD_PATH;
                    }
                }
            }
        }
    }
    override public void Stop()
    {
        for (int i = 0; i < _movePossibleTileCellList.Count; i++)
        {
            _movePossibleTileCellList[i].Draw(Color.white);
        }

        _moveRangeQueue.Clear();
        _movePossibleTileCellList.Clear();
    }
    void UpdateInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                int targetTileX = hit.transform.GetComponent<MapObject>().GetTileX();
                int targetTileY = hit.transform.GetComponent<MapObject>().GetTileY();

                TileCell target = GameManager.Instance.GetMap().GetTileCell(targetTileX, targetTileY);
                if (!(_character.GetTileX() == targetTileX && _character.GetTileY() == targetTileY))    //player위치 제외한 타일셀 클릭 시
                {
                    _character.ShowMoveCursor(hit.transform.GetComponent<MapObject>().transform.position);
                    if (_movePossibleTileCellList.Contains(target))
                    //if (target.IsPathfindable())
                    {
                        _character.SetTargetTileCell(target);
                        Debug.Log("이동가능 타일!");
                        _nextState = eStateType.PATHFINDING;
                    }
                }
            }
        }
    }
    sPosition GetPositionByDirection(sPosition curPosition, int direction)
    {
        sPosition position = curPosition;
        eMoveDirection moveDirection = (eMoveDirection)direction;
        switch (moveDirection)
        {
            case eMoveDirection.LEFT:
                position.x--;
                if (position.x < 0)
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
    void DrawMoveRange()
    {
        TileMap map = GameManager.Instance.GetMap();
        map.ResetVisit();

        TileCell startTileCell = map.GetTileCell(_character.GetTileX(), _character.GetTileY());
        startTileCell.SetPrevTileCell(null);
        sTileHeuristicInfo startCmd;
        startCmd.tileCell = startTileCell;
        startCmd.heuristic = 0.0f;
        _moveRangeQueue.Add(startCmd);

        while (0 != _moveRangeQueue.Count)
        {
            sTileHeuristicInfo command = _moveRangeQueue[0];
            _moveRangeQueue.RemoveAt(0);
            //가져온 커맨드의 현재 타일셀 방문 표시
            if (false == command.tileCell.IsVisited())
            {
                if (_character.GetMoveRange() == command.tileCell.GetDistanceFromStart())
                {
                    _moveRangeQueue.Clear();
                    return;
                }
                command.tileCell.SetVisit(true);
                command.tileCell.Draw(Color.blue);
                _movePossibleTileCellList.Add(command.tileCell);

                int tileX = command.tileCell.GetTileX();
                int tileY = command.tileCell.GetTileY();
                //4방향 next타일들 검사
                for (int direction = (int)eMoveDirection.LEFT; direction < (int)eMoveDirection.DOWN + 1; direction++)
                {
                    sPosition curPosition;
                    curPosition.x = command.tileCell.GetTileX();
                    curPosition.y = command.tileCell.GetTileY();
                    sPosition nextPosition = GetPositionByDirection(curPosition, direction);

                    TileCell nextTileCell = map.GetTileCell(nextPosition.x, nextPosition.y);
                    tileX = command.tileCell.GetTileX();
                    tileY = command.tileCell.GetTileY();

                    // nextTileCell 방문 안했고, 움직일수 있는 타일일때
                    if (null != nextTileCell && true == nextTileCell.IsPathfindable() && false == nextTileCell.IsVisited())
                    {
                        float distanceFromStart = command.tileCell.GetDistanceFromStart() + nextTileCell.GetDistanceFromWeight();
                        float heuristic = distanceFromStart;
                        if (null != _character.GetTargetTileCell())
                        {
                            heuristic = CalcAStarHeuristic(distanceFromStart, nextTileCell, _character.GetTargetTileCell());
                        }

                        if (null == nextTileCell.GetPrevTileCell() || distanceFromStart < nextTileCell.GetDistanceFromStart())
                        {
                            nextTileCell.SetDistanceFromStart(distanceFromStart);
                            nextTileCell.SetPrevTileCell(command.tileCell);

                            sTileHeuristicInfo nextCommand;
                            nextCommand.tileCell = nextTileCell;
                            nextCommand.heuristic = heuristic;
                            _moveRangeQueue.Add(nextCommand);
                            _moveRangeQueue.Sort(delegate (sTileHeuristicInfo cmd, sTileHeuristicInfo nextCmd)
                            {
                                return cmd.heuristic.CompareTo(nextCmd.heuristic);
                            });
                        }
                    }
                }
            }
        }
    }
    void DrawMovePath(TileCell targetTileCell)
    {
        for (int i = 0; i < _movePossibleTileCellList.Count; i++)
        {
            _movePossibleTileCellList[i].Draw(Color.blue);
        }
        TileCell pathTileCell = targetTileCell;
        while (null != pathTileCell)
        {
            pathTileCell.Draw(Color.red);
            pathTileCell = pathTileCell.GetPrevTileCell();
        }
    }
    private float CalcComplexHeuristic(TileCell nextTileCell, TileCell targetTileCell)
    {
        int distanceW = nextTileCell.GetTileX() - targetTileCell.GetTileX();
        int distanceH = nextTileCell.GetTileY() - targetTileCell.GetTileY();
        distanceW *= distanceW;
        distanceH *= distanceH;
        float distance = distanceW + distanceH;
        return distance;
    }
    protected float CalcAStarHeuristic(float distanceFromStart, TileCell nextTileCell, TileCell targetTileCell)
    {
        return distanceFromStart + CalcComplexHeuristic(nextTileCell, targetTileCell);
    }
}
