using Godot;
using System;

public partial class TargetBoard : Node
{
    #region Exports
    [Export] private Node3D[] targetDisable = null;
    [Export] private Node3D[] targetEnable = null;
    #endregion
    #region Methods
    public void DisableTargetById(int id)
    {
        targetDisable[id].Show();
        targetEnable[id].Hide();                
    }
    #endregion
}
