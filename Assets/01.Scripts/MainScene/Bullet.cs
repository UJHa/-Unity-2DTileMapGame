using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MapObject
{
    float _cameraWidth;
    float _cameraHeight;
    // Use this for initialization
    void Start () {
        Camera cam = Camera.main;
        _cameraHeight = 2.0f * cam.orthographicSize;
        _cameraWidth = _cameraHeight * cam.aspect;
        //Debug.Log(_cameraWidth);
        //Debug.Log(_cameraHeight);
        //Debug.Log(cam.transform.position);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(isMove)
        {
            gameObject.transform.position += _bulletSpeed;
            if(transform.position.x > _cameraWidth / 2 + Camera.main.transform.position.x || transform.position.x < -_cameraWidth / 2 + Camera.main.transform.position.x
                || transform.position.y > _cameraHeight / 2 + Camera.main.transform.position.y || transform.position.y < -_cameraHeight / 2 + Camera.main.transform.position.y)
            {
                Destroy(gameObject);
            }
        }
	}
    Vector3 _bulletSpeed;
    bool isMove = false;
    public void MoveStart(Vector3 speed)
    {
        _bulletSpeed = speed * 0.01f;
        isMove = true;
    }
    string _shooterName;
    public void SetShooter(string name)
    {
        _shooterName = name;
        //Debug.Log(_shooterName);
    }
    public string GetShooterName()
    {
        return _shooterName;
    }
}
