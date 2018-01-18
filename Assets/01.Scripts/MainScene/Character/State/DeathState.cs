using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
    override public	void Start () {
        base.Start();

        _character.SetCanMove(true);
    }
}
