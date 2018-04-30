using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
    override public void Start()
    {
        base.Start();
        _nowState = eStateType.DEATH;
        _character.SetCanMove(true);
        _character.gameObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
        _character.gameObject.GetComponent<CircleCollider2D>().enabled = false;

        MessageParam msgParam = new MessageParam();
        msgParam.sender = _character;
        msgParam.receiver = _character.GetAttacker();
        msgParam.message = "IsDead";

        //MessageSystem.Instance.Send(msgParam);


        _character.DropItem();
    }
    override public void Update()
    {
        base.Update();
    }
}
