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
    private Node3D crossCountryCollisionM = null;
    public Node3D GetCrossCountryCollisionM
    {
        get
        {
            return crossCountryCollisionM;
        }
    }
    private Node3D crossCountryCollisionRR = null;
    public Node3D GetCrossCountryCollisionRR
    {
        get
        {
            return crossCountryCollisionRR;
        }
    }
    private Node3D crossCountryCollisionRL = null;
    public Node3D GetCrossCountryCollisionRL
    {
        get
        {
            return crossCountryCollisionRL;
        }
    }
    private bool isLeftFree = true;
    public bool GetisLeftFree
    {
        get
        {
            return isLeftFree;
        }        
    }
    private bool isRightFree = true;
    public bool GetisRightFree
    {
        get
        {
            return isRightFree;
        }
    }
    private int characterIdCountry = 0;
    public int GetCharacterIdCountry
    {
        get
        {
            return characterIdCountry;
        }
    }
    private bool isCollided = false;
    public bool GetIsCollided
    {
        get { return isCollided; }
    }
    private bool isFinished = false;
    public bool GetSetIsFinished
    {
        get
        {
            return isFinished;
        }   
        set 
        { 
            isFinished = value;
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
        Connect("body_entered", new Callable(this, nameof(HillFenceEnter)));
        Connect("body_exited", new Callable(this, nameof(HillFenceExit)));
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
        if (body.Name == "GateFinishArea3D")
        {
            isFinished = true;            
        }

        var character = this.GetParent().GetParent() as Character;
        if (character.statesSki == Character.StatesSki.Running)
        {
            if (body is CrossCountryOvertake)
            {
                var obj = body as CrossCountryOvertake;
                if (colliderTypes == ColliderTypes.FrontMidle && isInside)
                {
                    if (body.GetParent().GetParent().Name.ToString().Contains("Character"))
                    {
                        isOvertake = true;
                    }                                     
                }
                if (colliderTypes == ColliderTypes.FrontMidle && !isInside)
                {
                    if (body.GetParent().GetParent().Name.ToString().Contains("Character"))
                    {
                        isOvertake = false;
                    }
                }
                if (colliderTypes == ColliderTypes.RearLeft && isInside)
                {
                    if (body.GetParent().GetParent().Name.ToString().Contains("Character"))
                    {
                        isLeftFree = false;
                    }
                }
                if (colliderTypes == ColliderTypes.RearLeft && !isInside)
                {
                    if (body.GetParent().GetParent().Name.ToString().Contains("Character"))
                    {
                        isLeftFree = true;
                    }
                }
                if (colliderTypes == ColliderTypes.RearRight && isInside)
                {
                    if (body.GetParent().GetParent().Name.ToString().Contains("Character"))
                    {
                        isRightFree = false;
                    }
                }
                if (colliderTypes == ColliderTypes.RearRight && !isInside)
                {
                    if (body.GetParent().GetParent().Name.ToString().Contains("Character"))
                    {
                        isRightFree = true;
                    }
                }
            }
        }   
    }

    public ColliderTypes GetColliderType()
    {
        return colliderTypes;        
    }
    private void HillFenceEnter(Node body)
    {
        if (body.GetParent().Name == "HillFence")
        {            
            isCollided = true;
        }
    }
    private void HillFenceExit(Node body)
    {
        if (body.GetParent().Name == "HillFence")
        {         
            isCollided = false;
        }
    }
    #endregion
}
