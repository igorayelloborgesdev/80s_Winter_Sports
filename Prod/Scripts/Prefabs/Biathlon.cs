using Godot;
using System;

public partial class Biathlon : Node
{
    [Export] private RayCast3D rayCast3D = null;
    [Export] private MeshInstance3D gunTarget = null;
    private bool isTriggerPulled = false;
    private float increment = 0.01f;

    [Export] private MeshInstance3D[] gunTargetArray = null;

    public override void _Process(double delta)
    {        
        
        if (Input.IsKeyPressed(Key.Left))
        {
            var inc = gunTarget.Position.X - increment;
            gunTarget.Position = new Vector3(inc, gunTarget.Position.Y, gunTarget.Position.Z);
        }
        if (Input.IsKeyPressed(Key.Right))
        {
            var inc = gunTarget.Position.X + increment;
            gunTarget.Position = new Vector3(inc, gunTarget.Position.Y, gunTarget.Position.Z);
        }
        if (Input.IsKeyPressed(Key.Up))
        {
            var inc = gunTarget.Position.Y + increment;
            gunTarget.Position = new Vector3(gunTarget.Position.X, inc, gunTarget.Position.Z);
        }
        if (Input.IsKeyPressed(Key.Down))
        {
            var inc = gunTarget.Position.Y - increment;
            gunTarget.Position = new Vector3(gunTarget.Position.X, inc, gunTarget.Position.Z);
        }
        if (Input.IsKeyPressed(Key.A))
        {
            if (!isTriggerPulled)
            {
                var collider = rayCast3D.GetCollider() as Node;
                if (rayCast3D.IsColliding())
                {
                    //GD.Print("Hit");
                    if (collider is not null)
                    {
                        //GD.Print(collider.Name);//<-
                        //GD.Print(rayCast3D.IsColliding());//<-
                        //GD.Print(rayCast3D.GetCollisionPoint());//<-
                    }
                }
                else
                {
                    //GD.Print("Miss");

                    //GD.Print(gunTarget.Position);//<-
                    var angle = Mathf.DegToRad(128.0f);
                    var hyp = 0.1f;

                    var cos = Math.Cos(angle);
                    var ca = cos * hyp;
                    var sin = Math.Sin(angle);
                    var co = sin * hyp;

                    gunTargetArray[1].Position = new Vector3(gunTarget.Position.X + (float)ca, gunTarget.Position.Y, gunTarget.Position.Z);

                    GD.Print(ca);
                    GD.Print(co);

                }
                isTriggerPulled = true;
            }
        }
        if (!Input.IsKeyPressed(Key.A) && isTriggerPulled)
        {
            isTriggerPulled = false;
            //GD.Print("Recharge");
        }
    }
}
