using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : State
{
    override public void Start()
    {
        base.Start();
    }
    override public void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 bulletDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _character.GetTransform().position;
            bulletDirection.z = 0.0f;
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

            // 마우스 위치의 각도 지정(다른 업데이트에서 처리)

            _nextState = eStateType.IDLE;
        }
    }
    override public void Stop()
    {
    }
}
