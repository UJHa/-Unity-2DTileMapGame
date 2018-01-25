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
            Vector2 position2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(position2D, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                int targetTileX = hit.transform.GetComponent<MapObject>().GetTileX();
                int targetTileY = hit.transform.GetComponent<MapObject>().GetTileY();

                TileCell target = GameManager.Instance.GetMap().GetTileCell(targetTileX, targetTileY);
                if (!(_character.GetTileX() == targetTileX && _character.GetTileY() == targetTileY))
                {
                    if (target.CanMove())
                    {
                        target.Draw(Color.blue);
                        _character.SetTargetTileCell(target);
                        //hit.transform.GetComponent<SpriteRenderer>().color = Color.blue;
                        _nextState = eStateType.PATHFINDING;
                    }
                }
            }
            //if (null == _character.GetTargetTileCell()) 
            //{
            //    _nextState = eStateType.IDLE;
            //}
            //RaycastHit hit;
            //Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            //if (Physics.Raycast(ray, out hit))
            //{
            //    //hit.transform.GetComponent<SpriteRenderer>().color = Color.blue;

            //    int targetTileX = hit.transform.GetComponent<MapObject>().GetTileX();
            //    int targetTileY = hit.transform.GetComponent<MapObject>().GetTileY();

            //    if (!(_character.GetTileX() == targetTileX && _character.GetTileY() == targetTileY))
            //    {
            //        _character.SetTargetTileCell(targetTileX, targetTileY);
            //        _nextState = eStateType.MOVE;
            //    }
            //}
        }
    }
}