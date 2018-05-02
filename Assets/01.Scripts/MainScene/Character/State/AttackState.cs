using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    override public void Start()
    {
        base.Start();
        _character.SetAnimation("idle");
        Debug.Log(_character + " : attack");
    }
    override public void Update()
    {
        base.Update();

        for (int i = 0; i < 12; i++)
        {
            Vector3 bulletDirection = new Vector3(1.0f, 0.0f, 0.0f);
            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, i * 30.0f);
            bulletDirection = bulletRotation * bulletDirection;
            bulletDirection.Normalize();

            // 탄 오브젝트 생성
            GameObject gameobject = Resources.Load<GameObject>("Prefabs/Bullet/DefaultBullet");
            GameObject bulletObject = GameObject.Instantiate(gameobject);
            //bulletObject.transform.SetParent(Camera.main.gameObject.transform);
            bulletObject.transform.position = new Vector3(_character.GetPosition().x, _character.GetPosition().y, 0.0f);
            bulletObject.transform.localScale = GameManager.Instance.GetMap().GetLocalScale();
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.SetShooter(_character.gameObject.name);
            bullet.MoveBullet(bulletDirection);
        }
        _nextState = eStateType.IDLE;
    }
}
