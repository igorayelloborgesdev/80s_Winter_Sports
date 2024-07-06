using Godot;
using System;

public partial class Target : Node3D
{
    #region exports
    [Export] private int id;
    #endregion
    #region variables
    private bool enable = true;
    #endregion
    #region Get Set
    public int GetId
    { 
        get 
        { 
            return id; 
        }
    }
    public bool GetSetEnable
    {
        get
        {
            return enable;
        }
        set
        {
            enable = value;
        }
    }
    #endregion
}
