using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelect : State
{
    override public void Start()
    {
        base.Start();
        _character.ResetActionCooltime();
        _character.SetSelectUI(true);
    }
    override public void Update()
    {
        base.Update();

        if(_character.IsClickAtkButton())
        {
            _nextState = eStateType.ATTACK;
            return;
        }
        if(_character.IsClickWaitButton())
        {
            _nextState = eStateType.IDLE;
            return;
        }
    }
    override public void Stop()
    {
        _character.SetSelectUI(false);
    }
}
