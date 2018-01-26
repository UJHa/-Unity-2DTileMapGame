﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPathState : State
{
    TileCell _reverseTileCell;
    override public void Start()
    {
        base.Start();
        _reverseTileCell = _character.GetTargetTileCell();

    }
    override public void Update()
    {
        base.Update();

        if (null != _reverseTileCell.GetPrevTileCell())
        {
            _character.PushPathTileCell(_reverseTileCell);
            _reverseTileCell.Draw(Color.white);
            _reverseTileCell = _reverseTileCell.GetPrevTileCell();
        }
        else
        {
            _nextState = eStateType.MOVE;
        }
    }
    public override void Stop()
    {
        base.Stop();
        _character.SetTargetTileCell(null);
    }
}