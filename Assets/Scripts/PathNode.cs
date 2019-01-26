using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SettlersEngine;

public class PathNode : IPathNode<Character>
{
    public Int32 X { get; set; }
    public Int32 Y { get; set; }
    public Boolean IsWall {get; set;}

    public PathNode(Int32 X, Int32 Y, Boolean IsWall) {
        this.X = X;
        this.Y = Y;
        this.IsWall = IsWall;
    }

    public bool IsWalkable(Character unused)
    {
        return !IsWall;
    }

}
