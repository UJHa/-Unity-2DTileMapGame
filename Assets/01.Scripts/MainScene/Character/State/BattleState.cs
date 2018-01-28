using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : State
{
    override public void Start()
    {
        base.Start();
    }
    public override void Update()
    {
        base.Update();
        if (_character.IsAttackPossible())
        {
            int moveX = _character.GetTileX();
            int moveY = _character.GetTileY();
            switch (_character.GetNextDirection())
            {
                case eMoveDirection.LEFT: moveX--; break;
                case eMoveDirection.RIGHT: moveX++; break;
                case eMoveDirection.UP: moveY--; break;
                case eMoveDirection.DOWN: moveY++; break;
            }
            TileMap map = GameManager.Instance.GetMap();
            TileCell targetTileCell = map.GetTileCell(moveX, moveY);
            List<MapObject> collisionList = map.GetCollisionList(moveX, moveY);
            if(false==targetTileCell.CanMove())
            {
                for (int i = 0; i < collisionList.Count; i++)
                {
                    _character.Attack(collisionList[i]);
                }
            }
            else
            {
                _nextState = eStateType.IDLE;
            }
        }
    }
}