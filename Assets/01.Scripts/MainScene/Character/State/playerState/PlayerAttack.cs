using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : State
{
    override public void Start()
    {
        base.Start();
        _character.GetDirectionUI().SetActive(true);
    }
    override public void Update()
    {
        base.Update();
        Vector3 bulletDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _character.GetTransform().position;
        bulletDirection.z = 0.0f;
        bulletDirection.Normalize();

        UpdateBulletDirectionUI(bulletDirection);

        if (Input.GetMouseButtonDown(0))
        {
            //Vector3 bulletDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _character.GetTransform().position;
            //bulletDirection.z = 0.0f;
            //bulletDirection.Normalize();

            // 탄 오브젝트 생성
            GameObject gameobject = Resources.Load<GameObject>("Prefabs/Bullet/DefaultBullet");
            GameObject bulletObject = GameObject.Instantiate(gameobject);
            //bulletObject.transform.SetParent(Camera.main.gameObject.transform);
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
        _character.GetDirectionUI().SetActive(false);
    }
    void UpdateBulletDirectionUI(Vector3 direction)
    {
        Vector3 defaultVector = new Vector3(1.0f, 0.0f, 0.0f);
        float dot = Vector3.Dot(defaultVector, direction);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        if (direction.y < 0)
        {
            dot = Vector3.Dot(defaultVector, -direction);
            angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
            angle -= 180.0f;
        }
        _character.GetDirectionUI().transform.localPosition = direction / 3.0f;
        _character.GetDirectionUI().transform.eulerAngles = new Vector3(0.0f, 0.0f, angle);
    }
}
