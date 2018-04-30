using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    void Start()
    {
        _type = eMapObjectType.MONSTER;
        //_attackCoolTime = 2.0f;
        _moveCoolTime = 2.0f;
    }
    protected override void InitPosition()
    {
        TileMap map = GameManager.Instance.GetMap();
        _tileX = 6;
        _tileY = 6;
        map.SetObject(_tileX, _tileY, this, eTileLayer.MIDDLE);

        SetCanMove(false);
    }
}