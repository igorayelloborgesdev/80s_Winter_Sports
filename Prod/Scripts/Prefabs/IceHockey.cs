using Godot;
using System;

public partial class IceHockey : Node
{
    #region Exports    
    [Export] private MeshInstance3D initPoint = null;
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
    #endregion
}
