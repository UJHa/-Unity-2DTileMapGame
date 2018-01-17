using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    // Use this for initialization
    void Start()
    {
        _type = eMapObjectType.MONSTER;
    }

    // Update is called once per frame
    void Update()
    {
        if (false == _isLive)
            return;
    }
}