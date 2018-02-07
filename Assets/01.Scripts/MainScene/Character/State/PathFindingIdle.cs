using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingIdle : State
{
    override public void Start()
    {
        base.Start(); 
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
            if(Physics.Raycast(ray, out hit))
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
}