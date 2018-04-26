using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MapObject
{
    float _mainCameraWidth;
    float _mainCameraHeight;
    // Use this for initialization
    void Start () {
        Camera cam = Camera.main;
        _mainCameraHeight = 2.0f * cam.orthographicSize;
        _mainCameraWidth = _mainCameraHeight * cam.aspect;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(isMove)
        {
            gameObject.transform.position += _bulletSpeed;
            if(transform.localPosition.x > _mainCameraWidth / 2 || transform.localPosition.x < -_mainCameraWidth / 2
                || transform.localPosition.y > _mainCameraHeight / 2 || transform.localPosition.y < -_mainCameraHeight / 2)
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
        Debug.Log(_shooterName);
    }
    public string GetShooterName()
    {
        return _shooterName;
    }
}
