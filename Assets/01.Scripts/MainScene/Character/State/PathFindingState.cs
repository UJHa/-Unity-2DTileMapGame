using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingState : State
{
    struct sPosition
    {
        public int x;
        public int y;
    }
    List<sPathCommand> _pathfindingQueue = new List<sPathCommand>();
    struct sPathCommand
    {
        public TileCell tileCell;
        public float heuristic;
    }
    override public void Start() {
        base.Start();
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
        sPathCommand startCmd;
        startCmd.tileCell = startTileCell;
        startCmd.heuristic = 0.0f;
        //PushPathfindingQueue(startCmd);
        _pathfindingQueue.Add(startCmd);

    }
    // Update is called once per frame
    override public void Update() {
        base.Update();
        UpdatePathFinding();
    }

    public override void Stop()
    {
        base.Stop();
        _pathfindingQueue.Clear();
    }
    private sPosition GetPositionByDirection(sPosition curPosition, int direction)
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
    private void UpdatePathFinding()
    {
        if (0 != _pathfindingQueue.Count)
        {
            //큐의 첫번째 커맨드 가져옴
            sPathCommand command = _pathfindingQueue[0];
            _pathfindingQueue.RemoveAt(0);
            //가져온 커맨드의 현재 타일셀 방문 표시
            if (false == command.tileCell.IsVisited())
            {
                command.tileCell.SetVisit(true);
                command.tileCell.Draw(Color.blue);

                //가져온 커맨드의 현재 타일셀이 목표 타일일 경우 nextState 변경
                if (_character.GetTargetTileCell() == command.tileCell)
                {
                    Debug.Log("찾았어양");
                    _character.SetTargetTileCell(command.tileCell);
                    //_findState = eFindState.BUILD_PATH;
                    _nextState = eStateType.BUILD_PATH;
                    return;
                }
                int tileX = command.tileCell.GetTileX();
                int tileY = command.tileCell.GetTileY();
                //4방향 next타일들 검사
                for (int direction = (int)eMoveDirection.LEFT; direction < (int)eMoveDirection.DOWN + 1; direction++)
                {
                    sPosition curPosition;
                    curPosition.x = command.tileCell.GetTileX();
                    curPosition.y = command.tileCell.GetTileY();
                    sPosition nextPosition = GetPositionByDirection(curPosition, direction);

                    TileMap map = GameManager.Instance.GetMap();
                    TileCell nextTileCell = map.GetTileCell(nextPosition.x, nextPosition.y);
                    tileX = command.tileCell.GetTileX();
                    tileY = command.tileCell.GetTileY();

                    // nextTileCell 방문 안했고, 움직일수 있는 타일일때
                    if (false == nextTileCell.IsVisited() && true == nextTileCell.CanMove())
                    {
                        float distanceFromStart = command.tileCell.GetDistanceFromStart() + nextTileCell.GetDistanceFromWeight();
                        //float heuristic = distanceFromStart;
                        float heuristic = CalcAStarHeuristic(distanceFromStart, nextTileCell, _character.GetTargetTileCell());

                        if (null == nextTileCell.GetPrevTileCell())
                        {
                            nextTileCell.SetDistanceFromStart(distanceFromStart);
                            nextTileCell.SetPrevTileCell(command.tileCell);

                            sPathCommand nextCommand;
                            nextCommand.tileCell = nextTileCell;
                            nextCommand.heuristic = heuristic;
                            PushPathfindingQueue(nextCommand);
                        }
                        else
                        {
                            if (distanceFromStart < nextTileCell.GetDistanceFromStart())
                            {
                                nextTileCell.SetDistanceFromStart(distanceFromStart);
                                nextTileCell.SetPrevTileCell(command.tileCell);

                                sPathCommand nextCommand;
                                nextCommand.tileCell = nextTileCell;
                                nextCommand.heuristic = heuristic;
                                PushPathfindingQueue(nextCommand);
                            }
                        }
                    }
                }
            }
        }
    }
    private void PushPathfindingQueue(sPathCommand command)
    {
        _pathfindingQueue.Add(command);
        _pathfindingQueue.Sort(delegate (sPathCommand cmd, sPathCommand nextCmd)
        {
            if (cmd.heuristic < nextCmd.heuristic) return -1;
            else if (cmd.heuristic > nextCmd.heuristic) return 1;
            return 0;
        }
        );
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
    private float CalcAStarHeuristic(float distanceFromStart, TileCell nextTileCell, TileCell targetTileCell)
    {
        return distanceFromStart + CalcComplexHeuristic(nextTileCell, targetTileCell);
    }
}