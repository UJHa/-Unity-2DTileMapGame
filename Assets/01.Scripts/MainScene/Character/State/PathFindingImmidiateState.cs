using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingImmidiateState : PathFindingState
{
    override public void Start()
    {
        base.Start();
        while (0 != _pathfindingQueue.Count)
        {
            UpdatePathFinding();
        }
    }
}