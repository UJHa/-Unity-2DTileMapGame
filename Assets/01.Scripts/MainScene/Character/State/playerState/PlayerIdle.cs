using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : IdleState
{
    override public void Start()
    {
        base.Start();
        SettingMovePossibleTiles();
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
    override protected void UpdateState()
    {
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
}
