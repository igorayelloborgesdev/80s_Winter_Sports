using Godot;
using System;

public partial class IceHockeyPlayerCollision : Area3D
{
    private bool isCollided = false;
    public bool GetIsCollided
    {
        get { return isCollided; }
    }
    public override void _Ready()
    {
        Connect("area_entered", new Callable(this, nameof(OnBodyEntered)));
        Connect("area_exited", new Callable(this, nameof(OnBodyExited)));
    }
    private void OnBodyEntered(Node body)
    {
        try
        {
            isCollided = true;
        }
        catch (Exception ex) { }
    }
    private void OnBodyExited(Node body)
    {
        try
        {
            isCollided = false;
        }
        catch (Exception ex) { }
    }

}
