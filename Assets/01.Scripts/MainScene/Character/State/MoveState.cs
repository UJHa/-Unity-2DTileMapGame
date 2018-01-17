﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    override public eMoveDirection Update()
    {
        base.Update();
        return eMoveDirection.NONE;
    }

    override public void Start()
    {
        base.Start();
        int moveX = _character.GetTileX();
        int moveY = _character.GetTileY();
        switch (_character.GetNextDirection())
        {
            case eMoveDirection.LEFT:  moveX--; break;
            case eMoveDirection.RIGHT: moveX++; break;
            case eMoveDirection.UP: moveY--; break;
            case eMoveDirection.DOWN: moveY++; break;
        }

        if (false == _character.MoveStart(moveX, moveY))
        {
            TileMap map = GameManager.Instance.GetMap();

            List<MapObject> collisionList = map.GetCollisionList(moveX, moveY);
            for (int i = 0; i < collisionList.Count; i++)
            {
                switch (collisionList[i].GetObjectType())
                {
                    case eMapObjectType.MONSTER:
                        _character.Attack(collisionList[i]);
                        break;
                }
            }
        }
        _character.SetNextDirection(eMoveDirection.NONE);
        _nextState = eStateType.IDLE;
        //TileMap map = GameManager.Instance.GetMap();

        //List<MapObject> collisionList = map.GetCollisionList(moveX, moveY);
        //if (0 == collisionList.Count)  //이동 가능할때
        //{
        //    map.ResetObject(_tileX, _tileY, this);
        //    _tileX = moveX;
        //    _tileY = moveY;
        //    map.SetObject(_tileX, _tileY, this, eTileLayer.MIDDLE);
        //}
        //else
        //{
        //    for (int i = 0; i < collisionList.Count; i++)
        //    {
        //        switch (collisionList[i].GetObjectType())
        //        {
        //            case eMapObjectType.MONSTER:
        //                Attack(collisionList[i]);
        //                break;
        //        }
        //    }
        //}
    }
}
