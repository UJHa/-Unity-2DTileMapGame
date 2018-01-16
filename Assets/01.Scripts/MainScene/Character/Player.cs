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
        eMoveDirection moveDirection = eMoveDirection.NONE;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDirection = eMoveDirection.LEFT;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDirection = eMoveDirection.RIGHT;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDirection = eMoveDirection.UP;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDirection = eMoveDirection.DOWN;
        }
        if (eMoveDirection.NONE != moveDirection)
            Move(moveDirection);
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
        if (map.CanMoveTile(moveX, moveY))
        {
            map.ResetObject(_tileX, _tileY, this);
            _tileX = moveX;
            _tileY = moveY;
            map.SetObject(_tileX, _tileY, this, eTileLayer.MIDDLE);
        }
    }
}