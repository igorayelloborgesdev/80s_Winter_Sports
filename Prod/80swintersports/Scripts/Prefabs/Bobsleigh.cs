using Godot;
using System;

public partial class Bobsleigh : Node3D
{
    #region Exports
    [Export] MeshInstance3D bodyMeshInstance3D = null;
    #endregion    
    #region Methods
    public void GenerateColor(Godot.Color aColor)
    {        
        var materialNew = new StandardMaterial3D();
        materialNew.AlbedoColor = aColor;
        materialNew.ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded;
        materialNew.DiffuseMode = BaseMaterial3D.DiffuseModeEnum.Lambert;
        materialNew.SpecularMode = BaseMaterial3D.SpecularModeEnum.Disabled;
        bodyMeshInstance3D.SetSurfaceOverrideMaterial(0, materialNew);        
    }
    #endregion
}
