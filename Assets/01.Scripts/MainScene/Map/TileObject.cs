using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MapObject
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    //Init
    public void Init(Sprite sprite, int tileX, int tileY)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        _tileX = tileX;
        _tileY = tileY;
    }
}