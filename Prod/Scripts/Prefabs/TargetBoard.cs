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
    public void EnableAllTargetById()
    {
        foreach (var target in targetEnable) 
        {
            var targetObj = target as Target;
            targetObj.GetSetEnable = true;
        }
        foreach (var target1 in targetDisable)
        {
            target1.Hide();            
        }
        foreach (var target2 in targetEnable)
        {
            target2.Show();
        }        
    }
    #endregion
}
