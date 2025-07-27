using Godot;
using System;

public partial class Supporter : Node3D
{
    #region Exports
    [Export] private Color[] colorsList;    
    #endregion
    #region MeshInstance3D
    MeshInstance3D bodyMeshInstance3D = null;
    #endregion

    #region Behavior
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    #endregion
    #region Get Set
    public int GetColorCount 
    { 
        get { return colorsList.Length; }
    }
    #endregion
    #region Method
    public void GenerateBodyColor(int colorId)
    {
        bodyMeshInstance3D = GetNode<MeshInstance3D>("Body");
        var materialNew = new StandardMaterial3D();
        materialNew.AlbedoColor = colorsList[colorId];
        materialNew.ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded;
        materialNew.DiffuseMode = BaseMaterial3D.DiffuseModeEnum.Lambert;
        materialNew.SpecularMode = BaseMaterial3D.SpecularModeEnum.Disabled;
        bodyMeshInstance3D.SetSurfaceOverrideMaterial(0, materialNew);
    }
    #endregion
}
