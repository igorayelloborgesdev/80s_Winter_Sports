using Godot;
using System;

public partial class CrossCountryOvertake : Area3D
{
    #region Exports
    [Export] public ColliderTypes colliderTypes = ColliderTypes.FrontMidle;
    #endregion
    #region Variables
    private bool isOvertake = false;
    public bool GetSetIsOvertake 
    {
        get 
        { 
            return isOvertake;
        }
        set 
        {
            isOvertake = value;
        }
    }
    private Node3D crossCountryCollisionML = null;
    public Node3D GetCrossCountryCollisionML
    {
        get
        {
            return crossCountryCollisionML;
        }        
    }
    private Node3D crossCountryCollisionMR = null;
    public Node3D GetCrossCountryCollisionMR
    {
        get
        {
            return crossCountryCollisionMR;
        }
    }
    private Node3D crossCountryCollisionFL = null;
    public Node3D GetCrossCountryCollisionFL
    {
        get
        {
            return crossCountryCollisionFL;
        }
    }
    private Node3D crossCountryCollisionFR = null;
    public Node3D GetCrossCountryCollisionFR
    {
        get
        {
            return crossCountryCollisionFR;
        }
    }
    #endregion
    #region Enum
    public enum ColliderTypes
    {
        FrontMidle,
        FrontLeft,
        FrontRight,
        RearMidle,
        RearLeft,
        RearRight,
    };    
    #endregion
    #region Behavior    
    public override void _Ready()
    {        
        Connect("area_entered", new Callable(this, nameof(AreaEnter)));
        Connect("area_exited", new Callable(this, nameof(AreaExited)));
    }
    #endregion
    #region Method
    private void AreaEnter(Node body)
    {
        try
        {
            DefineOvertake(body, true);
        }
        catch (Exception ex) { }
    }
    private void AreaExited(Node body)
    {
        try
        {
            DefineOvertake(body, false);
        }
        catch (Exception ex) { }
    }
    private void DefineOvertake(Node body, bool isInside)
    {
        if (body is CrossCountryOvertake)
        {
            var obj = body as CrossCountryOvertake;
            if (obj.GetColliderType() == ColliderTypes.RearMidle && colliderTypes == ColliderTypes.FrontMidle && isInside)
            {
                crossCountryCollisionML = body.GetParent().GetParent().GetNode<Node3D>("CrossCountryCollisionML");
                crossCountryCollisionMR = body.GetParent().GetParent().GetNode<Node3D>("CrossCountryCollisionMR");
                crossCountryCollisionFL = body.GetParent().GetParent().GetNode<Node3D>("CrossCountryCollisionFL");
                crossCountryCollisionFR = body.GetParent().GetParent().GetNode<Node3D>("CrossCountryCollisionFR");
                isOvertake = true;
            }
            if (obj.GetColliderType() == ColliderTypes.RearMidle && colliderTypes == ColliderTypes.FrontMidle && !isInside)
            {
                isOvertake = false;
            }
        }        
    }

    public ColliderTypes GetColliderType()
    {
        return colliderTypes;        
    }
    #endregion
}
