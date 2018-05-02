using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTileLayer
{
    GROUND,
    MIDDLE,
    UI,
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
    public Vector2 GetPosition()
    {
        return _position;
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
    public List<MapObject> GetMapObjectList(eTileLayer layer)
    {
        if (0 == _mapObjectMap[(int)layer].Count)
            return null;
        return _mapObjectMap[(int)layer];
    }
    //visit
    protected struct sTilePathFindInfo
    {
        public bool isVisit;
        public float distanceStart;
        public TileCell prevTileCell;
    }
    Dictionary<Character, sTilePathFindInfo> _pathFindInfo = new Dictionary<Character, sTilePathFindInfo>();
    //Dictionary<Character, bool> _isCharacterVisit;
    public void ResetPathFinding(Character character)
    {
        sTilePathFindInfo tilePathFindInfo;
        tilePathFindInfo.isVisit = false;
        tilePathFindInfo.distanceStart = 0.0f;
        tilePathFindInfo.prevTileCell = null;
        _pathFindInfo[character] = tilePathFindInfo;

        //SetVisit(character, false);
        //_distanceStart = 0.0f;
        //_prevTileCell = null;
    }
    //bool _isVisit;
    public void SetVisit(Character character, bool isVisited)
    {
        //_isVisit = isVisited;
        sTilePathFindInfo tilePathFindInfo = _pathFindInfo[character];
        tilePathFindInfo.isVisit = isVisited;
        _pathFindInfo[character] = tilePathFindInfo;
    }
    public bool IsVisited(Character character)
    {
        //return _isVisit;
        return _pathFindInfo[character].isVisit;
    }

    public bool IsPathfindable()
    {
        for (int layer = 0; layer < (int)eTileLayer.MAXCOUNT; layer++)
        {
            List<MapObject> objectList = _mapObjectMap[layer];
            for (int i = 0; i < objectList.Count; i++)
            {
                if  (eMapObjectType.MONSTER != objectList[i].GetObjectType() &&
                    false == objectList[i].CanMove())
                    return false;
            }
        }
        return true;
    }

    public void Draw(Color color)
    {
        List<MapObject> objectList = _mapObjectMap[(int)eTileLayer.GROUND];
        objectList[0].GetComponent<SpriteRenderer>().color = color;
    }
    //float _distanceStart = 0.0f;
    public float GetDistanceFromStart(Character character)
    {
        //return _distanceStart;
        return _pathFindInfo[character].distanceStart;
    }
    float _distanceWeight = 1.0f;
    public float GetDistanceFromWeight()
    {
        return _distanceWeight;
    }
    public void SetDistanceFromStart(Character character, float distance)
    {
        //_distanceStart = distance;
        sTilePathFindInfo tilePathFindInfo = _pathFindInfo[character];
        tilePathFindInfo.distanceStart = distance;
        _pathFindInfo[character] = tilePathFindInfo;
    }
    //TileCell _prevTileCell;
    public void SetPrevTileCell(Character character, TileCell prevTileCell)
    {
        //_prevTileCell = prevTileCell;
        sTilePathFindInfo tilePathFindInfo = _pathFindInfo[character];
        tilePathFindInfo.prevTileCell = prevTileCell;
        _pathFindInfo[character] = tilePathFindInfo;
    }
    public TileCell GetPrevTileCell(Character character)
    {
        return _pathFindInfo[character].prevTileCell;
    }
}