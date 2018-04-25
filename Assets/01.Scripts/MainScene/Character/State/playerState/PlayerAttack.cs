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
            Debug.Log("atk test");
            Debug.Log(_character.GetTransform().position);
            Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Debug.Log(bulletDirection);

            // 탄 오브젝트 생성
            GameObject gameobject = Resources.Load<GameObject>("Prefabs/Bullet/DefaultBullet");
            GameObject bulletObject = GameObject.Instantiate(gameobject);
            bulletObject.transform.SetParent(_character.GetTransform());
            bulletObject.transform.localPosition = Vector3.zero;
            bulletObject.transform.localScale = GameManager.Instance.GetMap().GetLocalScale();
            bulletObject.GetComponent<Bullet>().MoveStart(bulletDirection);

            // 마우스 위치의 각도 지정(다른 업데이트에서 처리)

            _nextState = eStateType.IDLE;
        }
        //if (_character.IsClickAtkButton())
        //{
        //    _nextState = eStateType.ATTACK;
        //    return;
        //}
        //if (_character.IsClickWaitButton())
        //{
        //    _nextState = eStateType.IDLE;
        //    return;
        //}
    }
    override public void Stop()
    {
    }
}
