using Godot;
using System;

public partial class GateStartFinish : Node3D
{
    #region Exports
    [Export] private Area3D area3D = null;    
    #endregion    
    #region Method
    public Area3D GetArea3D()
    { 
        return area3D; 
    }
    #endregion
}
