using Godot;
using System;

public partial class IceHockeyPlayerCollision : Area3D
{
    public override void _Ready()
    {
        Connect("area_entered", new Callable(this, nameof(OnBodyEntered)));
        //Connect("area_exited", new Callable(this, nameof(OnBodyExited)));
    }
    private void OnBodyEntered(Node body)
    {
        try
        {
            
        }
        catch (Exception ex) { }
    }
}
