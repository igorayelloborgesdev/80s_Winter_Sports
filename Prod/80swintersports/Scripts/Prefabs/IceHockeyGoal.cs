using Godot;
using System;

public partial class IceHockeyGoal : Node
{
    #region Exports        
    [Export] private Node3D goalRightDown = null;
    [Export] private Node3D goalRightUp = null;
    [Export] private Node3D goalLeftDown = null;
    [Export] private Node3D goalLeftUp = null;
    #endregion
    public Vector3 GetGoalPosition(int id)
    {
        if (id == 0)
            return goalRightDown.GlobalPosition;
        else if (id == 1)
            return goalRightUp.GlobalPosition;
        else if (id == 2)
            return goalLeftDown.GlobalPosition;
        else
            return goalLeftUp.GlobalPosition;
    }
    public Node3D GetGoalPositionNode3d(int id)
    {
        if (id == 0)
            return goalRightDown;
        else if (id == 1)
            return goalRightUp;
        else if (id == 2)
            return goalLeftDown;
        else
            return goalLeftUp;
    }
}
