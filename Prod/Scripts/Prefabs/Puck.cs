using Godot;
using System;

public partial class Puck : RigidBody3D
{
    private float initY = 5.0f;
    private float initYLimit = 0.0f;
    public override void _Ready()
    {
        
    }    
    public override void _Process(double delta)
    {        
        if (this.GlobalPosition.Y < initYLimit)
        {
            Vector3 vector = new Vector3(this.GlobalPosition.X, initY, this.GlobalPosition.Z);
            Transform3D transform = new Transform3D(Basis.Identity, vector);
            Transform3D newTransform = new Transform3D(transform.Basis, this.GlobalTransform.Origin);
            this.Transform = newTransform;
            this.LinearVelocity = Vector3.Zero;
            this.Position = new Vector3(this.GlobalPosition.X, initY, this.GlobalPosition.Z);
            GD.Print("TESTE");
        }        
    }
}
