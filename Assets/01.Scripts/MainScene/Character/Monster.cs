using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    override public void Init(string viewName)
    {
        base.Init(viewName);
        _type = eMapObjectType.MONSTER;
        _moveCooltime = 2.0f;
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