using Godot;
using System;
using WinterSports.Scripts.Static;

public partial class LugePathCollision : Area3D
{
    #region Behaviors
    private void OnBodyEntered(Node body)
    {
        try
        {            
            if (body.Name.ToString().Trim().ToLower() == "speednormal")
            {                
                LugeStatic.isSpeedNormal = true;
            }            
        }
        catch (Exception ex) { }
    }
    private void OnBodyExited(Node body)
    {
        try
        {
            if (body.Name.ToString().Trim().ToLower() == "speednormal")
            {             
                LugeStatic.isSpeedNormal = false;
            }
        }
        catch (Exception ex) { }
    }
    public override void _Ready()
    {
        Connect("area_entered", new Callable(this, nameof(OnBodyEntered)));
        Connect("area_exited", new Callable(this, nameof(OnBodyExited)));
    }
    #endregion
}
