using Godot;
using System;
using System.Linq;

public partial class GoalArea3D : Area3D
{

    #region Behaviors
    public override void _Ready()
    {
        Connect("area_entered", new Callable(this, nameof(OnBodyEntered)));
        Connect("area_exited", new Callable(this, nameof(OnBodyExited)));
    }
    private void OnBodyEntered(Node body)
    {
        try
        {            
            Vector3 collisionPoint = body.GetParent().GetParent<Character>().GlobalTransform.Origin;
            Vector3 areaCenter = GlobalTransform.Origin;
            Vector3 direction = (collisionPoint - areaCenter).Normalized();

            if (body.GetParent().GetParent<Character>().iceHockeyPosition != Character.IceHockeyPosition.GK)
            {                
                if (direction.Dot(Vector3.Forward) > 0.5f)
                {                 
                    body.GetParent().GetParent<Character>().iceHockeyMoveLimit["up"] = true;                    
                }
                else if (direction.Dot(Vector3.Back) > 0.5f)
                {                 
                    body.GetParent().GetParent<Character>().iceHockeyMoveLimit["down"] = true;                    
                }
                else if (direction.Dot(Vector3.Right) > 0.5f)
                {                 
                    body.GetParent().GetParent<Character>().iceHockeyMoveLimit["right"] = true;                    
                }
                else if (direction.Dot(Vector3.Left) > 0.5f)
                {                 
                    body.GetParent().GetParent<Character>().iceHockeyMoveLimit["left"] = true;                    
                }                
            }

        }
        catch (Exception ex) { }
    }
    private void OnBodyExited(Node body)
    {
        try
        {
            if (body.GetParent().GetParent<Character>().iceHockeyPosition != Character.IceHockeyPosition.GK)
            {
                foreach (var key in body.GetParent().GetParent<Character>().iceHockeyMoveLimit.Keys.ToList())
                {
                    body.GetParent().GetParent<Character>().iceHockeyMoveLimit[key] = false;
                }
            }
        }
        catch (Exception ex) { }
    }
    #endregion
}
