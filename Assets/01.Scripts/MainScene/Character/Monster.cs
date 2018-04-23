using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    void Start()
    {
        _type = eMapObjectType.MONSTER;
        //_attackCoolTime = 2.0f;
        _moveCoolTime = 2.0f;
    }
    override protected void InitState()
    {
        base.InitState();
    }
}