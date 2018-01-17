using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eStateType
{
    NONE,
    IDLE,
    MOVE,
}
public class State
{
    protected eStateType _nextState = eStateType.NONE;
    protected Character _character;

    public void Init(Character character)
    {
        _character = character;
    }

    virtual public void Start()
    {
        _nextState = eStateType.NONE;
    }
    virtual public void Stop()
    {

    }
    virtual public eMoveDirection Update()
    {
        if (eStateType.NONE != _nextState)
        {
            _character.ChangeState(_nextState);
            return eMoveDirection.NONE;
        }
        return eMoveDirection.NONE;
    }
}