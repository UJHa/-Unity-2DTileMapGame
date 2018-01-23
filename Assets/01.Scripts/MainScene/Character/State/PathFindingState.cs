using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingState : State
{
    struct sPathCommand
    {
        public TileCell tileCell;
        public TileCell prevTileCell;
    }
    Queue<sPathCommand> _pathfindingQueue = new Queue<sPathCommand>();
	// Use this for initialization
	override public void Start () {
        base.Start();
        TileMap map = GameManager.Instance.GetMap();
        map.GetTileCell(_character.GetTileX(), _character.GetTileY());
        for (int y = 0; y < map.GetHeight(); y++)
        {
            for (int x = 0; x < map.GetWidth(); x++)
            {
                map.GetTileCell(x, y).SetVisit(false);
            }
        }
        sPathCommand command;
        command.tileCell = map.GetTileCell(_character.GetTileX(), _character.GetTileY());
        command.prevTileCell = null;
        _pathfindingQueue.Enqueue(command);
    }

    // Update is called once per frame
    override public void Update () {
        base.Update();
		if(0 != _pathfindingQueue.Count)
        {
            //큐의 첫번째 커맨드 가져옴
            sPathCommand command = _pathfindingQueue.Dequeue();
            //가져온 커맨드의 현재 타일셀 방문 표시
            command.tileCell.SetVisit(true);
            //가져온 커맨드의 현재 타일셀이 목표 타일일 경우 nextState 변경
            if(_character.GetTargetTileCell()==command.tileCell)
            {
                Debug.Log("찾았어양");
                _nextState = eStateType.MOVE;
                return;
            }

            int tileX = command.tileCell.GetTileX();
            int tileY = command.tileCell.GetTileY();
            //4방향 next타일들 검사
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0: //left
                        tileX--;
                        break;
                    case 1: //right
                        tileX++;
                        break;
                    case 2: //up
                        tileY--;
                        break;
                    case 3: //down
                        tileY++;
                        break;
                }
                TileMap map = GameManager.Instance.GetMap();
                if (tileX < 0) tileX = 0;
                if (tileY < 0) tileY = 0;
                if (tileX >= map.GetWidth()) tileX = map.GetWidth() - 1;
                if (tileY >= map.GetHeight()) tileY = map.GetHeight() - 1;

                TileCell nextTileCell = map.GetTileCell(tileX, tileY);
                tileX = command.tileCell.GetTileX();
                tileY = command.tileCell.GetTileY();

                // nextTileCell 방문 안했고, 움직일수 있는 타일일때
                if (false == nextTileCell.IsVisited() && true == nextTileCell.CanMove())
                {
                    sPathCommand nextCommand;
                    nextCommand.tileCell = nextTileCell;
                    nextTileCell.Draw();
                    nextCommand.prevTileCell = command.tileCell;
                    _pathfindingQueue.Enqueue(nextCommand);
                }
            }
        }
	}
}
