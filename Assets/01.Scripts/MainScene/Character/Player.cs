using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    // Use this for initialization
    void Start()
    {
	}
	
	// Update is called once per frame
	void Update()
    {
        if (false == _isLive)
            return;
        eMoveDirection moveDirection = _state.Update();
        //moveState로 변환 예정
        //if (eMoveDirection.NONE != moveDirection)
        //{
        //    Move(moveDirection);
        //}
    }
    void Move(eMoveDirection moveDirection)
    {
        string animationTrigger = "down";
        int moveX = _tileX;
        int moveY = _tileY;
        switch(moveDirection)
        {
            case eMoveDirection.LEFT: animationTrigger = "left"; moveX--; break;
            case eMoveDirection.RIGHT: animationTrigger = "right"; moveX++; break;
            case eMoveDirection.UP: animationTrigger = "up"; moveY--; break;
            case eMoveDirection.DOWN: animationTrigger = "down"; moveY++; break;
        }

        _characterView.GetComponent<Animator>().SetTrigger(animationTrigger);

        TileMap map = GameManager.Instance.GetMap();

        List<MapObject> collisionList = map.GetCollisionList(moveX, moveY);
        if(0==collisionList.Count)  //이동 가능할때
        {
            map.ResetObject(_tileX, _tileY, this);
            _tileX = moveX;
            _tileY = moveY;
            map.SetObject(_tileX, _tileY, this, eTileLayer.MIDDLE);
        }
        else
        {
            for (int i = 0; i < collisionList.Count; i++)
            {
                switch(collisionList[i].GetObjectType())
                {
                    case eMapObjectType.MONSTER:
                        Attack(collisionList[i]);
                        break;
                }
            }
        }
    }
}