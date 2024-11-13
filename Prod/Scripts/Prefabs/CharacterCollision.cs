using Godot;
using System;
using WinterSports.Scripts.Static;

public partial class CharacterCollision : Area3D
{
    #region Behaviors
    private void OnBodyEntered(Node body)
    {        
        try
        {            
            if (body.Name.ToString().Trim().ToLower() == "area3dfinish")
            {
                SpeedSkatingStatic.isLapFinished = true;
                BiathlonStatic.isLapFinished = true;
            }                
        }
        catch (Exception ex) { }
    }
    public override void _Ready()
    {
        Connect("area_entered", new Callable(this, nameof(OnBodyEntered)));
    }
    #endregion    
}
