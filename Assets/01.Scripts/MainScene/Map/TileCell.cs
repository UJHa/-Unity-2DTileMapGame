﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTileLayer
{
    GROUND,
    MIDDLE,
    MAXCOUNT,
}

public class TileCell
{
    Vector2 _position;
    List<List<MapObject>> _mapObjectMap = new List<List<MapObject>>();

    public void Init()
    {
        for (int i = 0; i < (int)eTileLayer.MAXCOUNT; i++)
        {
            List<MapObject> mapObjectList = new List<MapObject>();
            _mapObjectMap.Add(mapObjectList);
        }
    }
    public void SetPosition(float x, float y)
    {
        _position.x = x;
        _position.y = y;
    }
    //tile position
    private int _tileX;
    private int _tileY;

    public void SetTilePosition(int tileX, int tileY)
    {
        _tileX = tileX;
        _tileY = tileY;
    }
    public int GetTileX() { return _tileX; }
    public int GetTileY() { return _tileY; }

    public void AddObject(eTileLayer layer, MapObject mapObject)
    {
        List<MapObject> mapObjectList = _mapObjectMap[(int)layer];

        int sortingOrder = mapObjectList.Count;
        mapObject.SetSortingOrder(layer, sortingOrder);
        mapObject.SetPosition(_position);

        mapObjectList.Add(mapObject);
    }
    public void RemoveObject(MapObject mapObject)
    {
        List<MapObject> mapObjectList = _mapObjectMap[(int)mapObject.GetCurrentLayer()];
        mapObjectList.Remove(mapObject);
    }
    public bool CanMove()
    {
        for (int layer = 0; layer < (int)eTileLayer.MAXCOUNT; layer++)
        {
            List<MapObject> objectList = _mapObjectMap[layer];
            for (int i = 0; i < objectList.Count; i++)
            {
                if (false == objectList[i].CanMove())
                    return false;
            }
        }
        return true;
    }
    public List<MapObject> GetCollisionList()
    {
        List<MapObject> collisionList = new List<MapObject>();
        for (int layer = 0; layer < (int)eTileLayer.MAXCOUNT; layer++)
        {
            List<MapObject> objectList = _mapObjectMap[layer];
            for (int i = 0; i < objectList.Count; i++)
            {
                if (false == objectList[i].CanMove())
                {
                    collisionList.Add(objectList[i]);
                }
            }
        }
        return collisionList;
    }
    //visit
    private bool _isVisit;
    public void SetVisit(bool isVisited)
    {
        _isVisit = isVisited;
    }
    public bool IsVisited() { return _isVisit; }
    public void Draw()
    {
        List<MapObject> objectList = _mapObjectMap[(int)eTileLayer.GROUND];
        objectList[0].GetComponent<SpriteRenderer>().color = Color.blue;
    }
}