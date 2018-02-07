﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
    override public	void Start () {
        base.Start();

        _character.SetCanMove(true);
        _character.gameObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);

        MessageParam msgParam = new MessageParam();
        msgParam.sender = _character;
        msgParam.receiver = _character.GetAttacker();
        msgParam.message = "IsDead";

        MessageSystem.Instance.Send(msgParam);

        _character.DropItem();
    }
}
