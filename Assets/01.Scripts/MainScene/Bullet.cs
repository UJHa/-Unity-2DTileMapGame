using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MapObject
{
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(isMove)
        {
            gameObject.transform.position += _bulletSpeed;
        }
	}
    Vector3 _bulletSpeed;
    bool isMove = false;
    public void MoveStart(Vector3 speed)
    {
        _bulletSpeed = speed * 0.1f;
        isMove = true;
    }
}
