using Godot;
using System;

public partial class SpeedSkating : Node
{
    #region Exports
    [Export] private MeshInstance3D[] initPoint = null;
    #endregion
    #region Methods
    public MeshInstance3D GetInitPoint(int id)
    {
        return initPoint[id];
    }
    #endregion
}
