using Godot;
using System;

public partial class DirectionArrow : Node3D
{
    #region Import    
    [Export] private Color[] colorsList;
    [Export] private MeshInstance3D[] bodyMeshInstance3D = null;
    #endregion
    #region Variables
    public int id = 0;
    public int direction = 0;
    public bool enable = true;
    public bool playerScore = false;
    #endregion
    #region Methods
    public void GenerateBodyColor()
    {
        bodyMeshInstance3D[direction - 1].Show();
    }
    public void SetBodyColor(int colorId)
    {        
        var dir = direction - 1;
        var newMaterial = bodyMeshInstance3D[dir].GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
        var albedoTexture = newMaterial.AlbedoTexture.ResourcePath;
        Texture textureResource = GD.Load<Texture>(albedoTexture);
        Texture2D texture2D = textureResource as Texture2D;
        var materialNew = new StandardMaterial3D();
        materialNew.AlbedoTexture = texture2D;
        materialNew.AlbedoColor = colorsList[colorId];
        materialNew.ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded;
        materialNew.DiffuseMode = BaseMaterial3D.DiffuseModeEnum.Lambert;
        materialNew.SpecularMode = BaseMaterial3D.SpecularModeEnum.Disabled;
        materialNew.Transparency = BaseMaterial3D.TransparencyEnum.Alpha;
        materialNew.BlendMode = BaseMaterial3D.BlendModeEnum.Mix;
        bodyMeshInstance3D[dir].SetSurfaceOverrideMaterial(0, materialNew);
    }
    #endregion
}
