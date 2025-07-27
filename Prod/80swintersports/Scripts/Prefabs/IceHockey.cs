using Godot;
using System;

public partial class IceHockey : Node
{
    #region Exports    
    [Export] private MeshInstance3D initPoint = null;
    [Export] private Node3D camera3D = null;
    [Export] private IceHockeyGoal Goal1 = null;
    [Export] private IceHockeyGoal Goal2 = null;
    #endregion
    #region Behaviors
    public override void _Ready()
    {        
    }
    #endregion
    #region Get Set
    public MeshInstance3D GetSetInitPoint()
    {
        return initPoint;
    }
    public Node3D GetSetCamera3D()
    {
        return camera3D;
    }
    public IceHockeyGoal GetIceHockeyGoal(int id)
    {
        return id == 0 ? Goal1 : Goal2;
    }
    #endregion
}
