using Godot;
using System;

public partial class Puck : RigidBody3D
{
    private float initY = 1.0f;
    private float initYLimit = 0.0f;
    public override void _Ready()
    {
        
    }    
    public override void _Process(double delta)
    {    
        try
        {
            if (this.GlobalPosition.Y < initYLimit)
            {
                Vector3 vector = new Vector3(this.GlobalPosition.X, initY, this.GlobalPosition.Z);
                Transform3D transform = new Transform3D(Basis.Identity, vector);
                Transform3D newTransform = new Transform3D(transform.Basis, this.GlobalTransform.Origin);
                this.Transform = newTransform;
                this.LinearVelocity = Vector3.Zero;
                if (this.GlobalPosition.X > 4.0f)
                {
                    this.Position = new Vector3(4.5f, initY, this.GlobalPosition.Z);
                }
                else if (this.GlobalPosition.X < -4.0f)
                {
                    this.Position = new Vector3(-4.5f, initY, this.GlobalPosition.Z);
                }
                else if (this.GlobalPosition.Z > 11.0f)
                {
                    this.Position = new Vector3(this.GlobalPosition.X, initY, 7.5f);
                }
                else if (this.GlobalPosition.Z < -11.0f)
                {
                    this.Position = new Vector3(this.GlobalPosition.X, initY, -7.5f);
                }
            }
        } catch (Exception e) {
            GD.Print(e);
        }
        
    }
}
