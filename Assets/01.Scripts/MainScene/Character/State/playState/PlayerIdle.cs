using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : State
{
    protected List<sPathCommand> _pathfindingQueue = new List<sPathCommand>();
    protected struct sPathCommand
    {
        public TileCell tileCell;
        public float heuristic;
    }
    override public void Start()
    {
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
        _pathfindingQueue.Add(startCmd);

        while (0 != _pathfindingQueue.Count)
        {
            sPathCommand command = _pathfindingQueue[0];
            _pathfindingQueue.RemoveAt(0);
            //가져온 커맨드의 현재 타일셀 방문 표시
            if (false == command.tileCell.IsVisited())
            {
                if (_character.GetMoveRange() == command.tileCell.GetDistanceFromStart())
                    return;
                command.tileCell.SetVisit(true);
                command.tileCell.Draw(Color.blue);

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
                        {
                            nextTileCell.SetDistanceFromStart(distanceFromStart);
                            nextTileCell.SetPrevTileCell(command.tileCell);

                            sPathCommand nextCommand;
                            nextCommand.tileCell = nextTileCell;
                            nextCommand.heuristic = heuristic;
                            _pathfindingQueue.Add(command);
                        }
                    }
                }
            }
        }
    }
    override public void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0))
        {
            //Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Ray ray = new Ray(position, Vector3.zero);
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
                    if (target.IsPathfindable())
                    {
                        target.Draw(Color.blue);
                        _character.SetTargetTileCell(target);
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
}
