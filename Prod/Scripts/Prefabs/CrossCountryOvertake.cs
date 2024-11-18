using Godot;
using System;

public partial class CrossCountryOvertake : Area3D
{
    #region Exports
    [Export] public ColliderTypes colliderTypes = ColliderTypes.FrontMidle;
    #endregion
    #region Variables
    public bool isOvertake = false;
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
