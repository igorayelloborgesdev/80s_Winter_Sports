using Godot;
using System;
using WinterSports.Scripts.Static;

public partial class SpeedContact : Area3D
{
    #region Behaviors
    private void OnBodyEntered(Node body)
    {
        try
        {            
            if (body.Name.ToString().Trim().ToLower() == "area3d")
            {
                var directionArrow = body.GetParent<DirectionArrow>();                
                if (directionArrow.enable)
                {                    
                    SpeedSkatingStatic.isCollided = true;
                    SpeedSkatingStatic.direction = directionArrow.direction;
                    SpeedSkatingStatic.id = directionArrow.id;
                }
                
            }            
        }catch(Exception ex) { }        
    }
    private void OnBodyExited(Node body)
    {
        try
        {
            if (body.Name.ToString().Trim().ToLower() == "area3d")
            {
                var directionArrow = body.GetParent<DirectionArrow>();
                SpeedSkatingStatic.currentIndex++;
                SpeedSkatingStatic.id++;
                if (directionArrow.enable)
                {                                        
                    if (!directionArrow.playerScore)
                    {                        
                        SpeedSkatingStatic.isNotScore = true;
                    }

                    directionArrow.enable = false;
                    SpeedSkatingStatic.isCollided = false;
                    SpeedSkatingStatic.direction = 0;                                        
                }
                if (SpeedSkatingStatic.arrowCount == SpeedSkatingStatic.id)
                {
                    SpeedSkatingStatic.currentIndex = 0;
                    SpeedSkatingStatic.id = 0;
                    SpeedSkatingStatic.resetArrowCount = true;
                }
            }
        }
        catch (Exception ex) { }
    }
    // This method connects the body_entered signal
    public override void _Ready()
    {        
        Connect("area_entered", new Callable(this, nameof(OnBodyEntered)));
        Connect("area_exited", new Callable(this, nameof(OnBodyExited)));
    }
    #endregion
}
