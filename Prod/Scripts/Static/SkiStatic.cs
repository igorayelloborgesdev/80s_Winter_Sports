using Godot;
using System;

public static class SkiStatic
{
    #region variables
    public static bool isCollided = false;
    #endregion
    #region Method
    public static void Reset()
    { 
        isCollided = false; 
    }
    #endregion
}
