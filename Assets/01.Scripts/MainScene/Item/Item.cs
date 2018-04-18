using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MapObject
{
    public void Init(int tileX, int tileY)
    {
        TileMap map = GameManager.Instance.GetMap();

        map.SetObject(tileX, tileY, this, eTileLayer.MIDDLE);

        SetCanMove(true);
    }
    override public void ReceiveObjectMessage(MessageParam msgParam)
    {
        switch (msgParam.message)
        {
            case "pick":
                Debug.Log("I'm picked!");
                this.gameObject.SetActive(false);
                break;
        }
    }
}
