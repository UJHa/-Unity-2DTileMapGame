using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    //override public void Start()
    //{
    //    base.Start();

    //    int moveX = _character.GetTileX();
    //    int moveY = _character.GetTileY();
    //    switch (_character.GetNextDirection())
    //    {
    //        case eMoveDirection.LEFT: moveX--; break;
    //        case eMoveDirection.RIGHT: moveX++; break;
    //        case eMoveDirection.UP: moveY--; break;
    //        case eMoveDirection.DOWN: moveY++; break;
    //    }

    //    TileMap map = GameManager.Instance.GetMap();
    //    List<MapObject> collisionList = map.GetCollisionList(moveX, moveY);
    //    if(null != collisionList && 0 == collisionList.Count)
    //    {
    //        for (int i = 0; i < collisionList.Count; i++)
    //        {
    //            switch (collisionList[i].GetObjectType())
    //            {
    //                case eMapObjectType.MONSTER:
    //                    _character.Attack(collisionList[i]);
    //                    break;
    //            }
    //        }
    //    }
    //    _character.SetNextDirection(eMoveDirection.NONE);
    //    _nextState = eStateType.IDLE;
    //}
    override public void Start()
    {
        base.Start();
        Vector3 bulletDirection = new Vector3(1.0f, 0.0f, 0.0f);
        {
            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, 0.0f);
            bulletDirection = bulletRotation * bulletDirection;
            bulletDirection.Normalize();

            // 탄 오브젝트 생성
            GameObject gameobject = Resources.Load<GameObject>("Prefabs/Bullet/DefaultBullet");
            GameObject bulletObject = GameObject.Instantiate(gameobject);
            bulletObject.transform.SetParent(Camera.main.gameObject.transform);
            bulletObject.transform.position = new Vector3(_character.GetTransform().position.x, _character.GetTransform().position.y, 0.0f);
            bulletObject.transform.localScale = GameManager.Instance.GetMap().GetLocalScale();
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.SetShooter(_character.gameObject.name);
            bullet.MoveStart(bulletDirection);
        }
        {
            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, 30.0f);
            bulletDirection = bulletRotation * bulletDirection;
            bulletDirection.Normalize();

            // 탄 오브젝트 생성
            GameObject gameobject = Resources.Load<GameObject>("Prefabs/Bullet/DefaultBullet");
            GameObject bulletObject = GameObject.Instantiate(gameobject);
            bulletObject.transform.SetParent(Camera.main.gameObject.transform);
            bulletObject.transform.position = new Vector3(_character.GetTransform().position.x, _character.GetTransform().position.y, 0.0f);
            bulletObject.transform.localScale = GameManager.Instance.GetMap().GetLocalScale();
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.SetShooter(_character.gameObject.name);
            bullet.MoveStart(bulletDirection);
        }
        _nextState = eStateType.IDLE;
    }
    override public void Update()
    {
        base.Update();
    }
}
