using System.Collections;
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

    public void AddObject(eTileLayer layer, MapObject mapObject)
    {
        List<MapObject> mapObjectList = _mapObjectMap[(int)layer];

        int sortingOrder = mapObjectList.Count;
        //mapObject.SetSortingOrder(sortingID, sortingOrder);
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
}