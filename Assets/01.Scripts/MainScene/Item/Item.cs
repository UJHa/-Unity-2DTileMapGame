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
                
                //_damagePoint = msgParam.attackPoint;
                //SetAttacker(msgParam.sender);

                //sPosition curPosition;
                //curPosition.x = _tileX;
                //curPosition.y = _tileY;
                //sPosition attackedPosition;
                //attackedPosition.x = msgParam.sender.GetTileX();
                //attackedPosition.y = msgParam.sender.GetTileY();
                //eMoveDirection direction = GetDirection(curPosition, attackedPosition);
                //SetNextDirection(direction);
                //MoveStart(attackedPosition.x, attackedPosition.y);
                //_state.NextState(eStateType.DAMAGE);
                break;
            //case "IsDead":
            //    Debug.Log("I'm dead!");
            //    Character msgSender = (Character)msgParam.sender;
            //    Debug.Log(msgSender.GetEXP());
            //    IncreaseEXP(msgSender.GetEXP());
            //    break;
        }
    }
}
