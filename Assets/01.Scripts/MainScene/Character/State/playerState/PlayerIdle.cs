using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : State
{
    List<sTileHeuristicInfo> _tileInfoQueue = new List<sTileHeuristicInfo>();
    List<TileCell> _movePossibleTiles = new List<TileCell>();
    protected struct sTileHeuristicInfo
    {
        public TileCell tileCell;
        public float heuristic;
    }
    override public void Start()
    {
        base.Start();
        _character.ResetActionCooltime();
        SettingMovePossibleTiles();
    }
    override public void Update()
    {
        base.Update();
        if (_character.IsActionPossible())
        {
            DrawMovePath(null);
        }
        else
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            int targetTileX = hit.transform.GetComponent<MapObject>().GetTileX();
            int targetTileY = hit.transform.GetComponent<MapObject>().GetTileY();

            TileCell target = GameManager.Instance.GetMap().GetTileCell(targetTileX, targetTileY);
            if (_movePossibleTiles.Contains(target))
            {
                _character.SetTargetTileCell(target);
                SettingTilePath();
                DrawMovePath(target);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!(_character.GetTileX() == targetTileX && _character.GetTileY() == targetTileY))    //player위치 제외한 타일셀 클릭 시
                {
                    _character.ShowMoveCursor(hit.transform.GetComponent<MapObject>().transform.position);
                    if (_movePossibleTiles.Contains(target))
                    {
                        _character.SetTargetTileCell(target);
                        _nextState = eStateType.MOVE;
                    }
                }
            }
        }
    }
    override public void Stop()
    {
        for (int i = 0; i < _movePossibleTiles.Count; i++)
        {
            _movePossibleTiles[i].Draw(Color.white);
        }

        _tileInfoQueue.Clear();
        _movePossibleTiles.Clear();
    }
    void SettingMovePossibleTiles()
    {
        TileMap map = GameManager.Instance.GetMap();
        map.ResetVisit();

        TileCell startTileCell = map.GetTileCell(_character.GetTileX(), _character.GetTileY());
        startTileCell.SetPrevTileCell(null);
        sTileHeuristicInfo startCmd;
        startCmd.tileCell = startTileCell;
        startCmd.heuristic = 0.0f;
        _tileInfoQueue.Add(startCmd);

        while (0 != _tileInfoQueue.Count)
        {
            sTileHeuristicInfo command = _tileInfoQueue[0];
            _tileInfoQueue.RemoveAt(0);
            //가져온 커맨드의 현재 타일셀 방문 표시
            if (false == command.tileCell.IsVisited())
            {
                if (_character.GetMoveRange() == command.tileCell.GetDistanceFromStart())
                {
                    _tileInfoQueue.Clear();
                    return;
                }
                command.tileCell.SetVisit(true);
                _movePossibleTiles.Add(command.tileCell);

                //4방향 next타일들 검사
                for (int direction = (int)eMoveDirection.LEFT; direction < (int)eMoveDirection.DOWN + 1; direction++)
                {
                    sPosition curPosition;
                    curPosition.x = command.tileCell.GetTileX();
                    curPosition.y = command.tileCell.GetTileY();
                    sPosition nextPosition = _character.GetPositionByDirection(curPosition, direction);

                    TileCell nextTileCell = map.GetTileCell(nextPosition.x, nextPosition.y);
                    // nextTileCell 방문 안했고, 움직일수 있는 타일일때
                    if (null != nextTileCell && true == nextTileCell.IsPathfindable() && false == nextTileCell.IsVisited())
                    {
                        float distanceFromStart = command.tileCell.GetDistanceFromStart() + nextTileCell.GetDistanceFromWeight();
                        float heuristic = distanceFromStart;

                        if (null == nextTileCell.GetPrevTileCell() || distanceFromStart < nextTileCell.GetDistanceFromStart())
                        {
                            nextTileCell.SetDistanceFromStart(distanceFromStart);
                            nextTileCell.SetPrevTileCell(command.tileCell);

                            sTileHeuristicInfo nextCommand;
                            nextCommand.tileCell = nextTileCell;
                            nextCommand.heuristic = heuristic;
                            PushSortmoveRangeQueue(nextCommand);
                        }
                    }
                }
            }
        }
    }
    void SettingTilePath()
    {
        TileMap map = GameManager.Instance.GetMap();
        map.ResetVisit();

        TileCell startTileCell = map.GetTileCell(_character.GetTileX(), _character.GetTileY());
        startTileCell.SetPrevTileCell(null);
        sTileHeuristicInfo startCmd;
        startCmd.tileCell = startTileCell;
        startCmd.heuristic = 0.0f;
        _tileInfoQueue.Add(startCmd);

        while (0 != _tileInfoQueue.Count)
        {
            sTileHeuristicInfo command = _tileInfoQueue[0];
            _tileInfoQueue.RemoveAt(0);
            //가져온 커맨드의 현재 타일셀 방문 표시
            if (false == command.tileCell.IsVisited())
            {
                if (_character.GetMoveRange() == command.tileCell.GetDistanceFromStart())
                {
                    _tileInfoQueue.Clear();
                    return;
                }
                command.tileCell.SetVisit(true);

                //4방향 next타일들 검사
                for (int direction = (int)eMoveDirection.LEFT; direction < (int)eMoveDirection.DOWN + 1; direction++)
                {
                    sPosition curPosition;
                    curPosition.x = command.tileCell.GetTileX();
                    curPosition.y = command.tileCell.GetTileY();
                    sPosition nextPosition = _character.GetPositionByDirection(curPosition, direction);

                    TileCell nextTileCell = map.GetTileCell(nextPosition.x, nextPosition.y);
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
                            PushSortmoveRangeQueue(nextCommand);
                        }
                    }
                }
            }
        }
    }
    void DrawMovePath(TileCell targetTileCell)
    {
        for (int i = 0; i < _movePossibleTiles.Count; i++)
        {
            _movePossibleTiles[i].Draw(Color.blue);
        }
        TileCell pathTileCell = targetTileCell;
        while (null != pathTileCell)
        {
            pathTileCell.Draw(Color.red);
            pathTileCell = pathTileCell.GetPrevTileCell();
        }
    }
    void PushSortmoveRangeQueue(sTileHeuristicInfo command)
    {
        _tileInfoQueue.Add(command);
        _tileInfoQueue.Sort(delegate (sTileHeuristicInfo cmd, sTileHeuristicInfo nextCmd)
        {
            return cmd.heuristic.CompareTo(nextCmd.heuristic);
        });
    }
    float CalcComplexHeuristic(TileCell nextTileCell, TileCell targetTileCell)
    {
        int distanceW = nextTileCell.GetTileX() - targetTileCell.GetTileX();
        int distanceH = nextTileCell.GetTileY() - targetTileCell.GetTileY();
        distanceW *= distanceW;
        distanceH *= distanceH;
        float distance = distanceW + distanceH;
        return distance;
    }
    float CalcAStarHeuristic(float distanceFromStart, TileCell nextTileCell, TileCell targetTileCell)
    {
        return distanceFromStart + CalcComplexHeuristic(nextTileCell, targetTileCell);
    }
}
