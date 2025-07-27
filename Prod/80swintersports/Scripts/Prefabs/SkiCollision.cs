using Godot;
using System;

public partial class SkiCollision : Area3D
{
    #region Variable
    private bool isCollided = false;
    private bool isFinished = false;
    #endregion
    #region Get Set
    public bool GetIsCollided 
    { 
        get { return isCollided; }
    }
    public bool GetIsFinished
    {
        get { return isFinished; }
    }
    #endregion
    #region Behavior    
    public override void _Ready()
    {
        Connect("body_entered", new Callable(this, nameof(HillFenceEnter)));
        Connect("body_exited", new Callable(this, nameof(HillFenceExit)));
        Connect("area_entered", new Callable(this, nameof(HillFenceAreaEnter)));
    }
    #endregion
    #region Methods
    private void HillFenceAreaEnter(Node body)
    {
        try
        {
            if (body.GetParent().GetParent().Name == "GateFinishCrossCountry")
            {
                isFinished = true;
            }            
        }catch (Exception ex) { }
        
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
    public void Reset()
    {
        isCollided = false;
        isFinished = false;
    }
    #endregion
}


