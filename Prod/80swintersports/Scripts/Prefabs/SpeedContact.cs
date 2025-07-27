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
                OnBodyEnteredSpeedSkating(directionArrow);
                OnBodyEnteredBiathlon(directionArrow);
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
                OnBodyExitedSpeedSkating(directionArrow);
                OnBodyExitedBiathlon(directionArrow);
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
    #region Speed Skating
    private void OnBodyEnteredSpeedSkating(DirectionArrow directionArrow)
    {
        if (GameModeSingleton.sport == 5 || GameModeSingleton.sport == 6)
        {
            if (directionArrow.enable)
            {
                SpeedSkatingStatic.isCollided = true;
                SpeedSkatingStatic.direction = directionArrow.direction;
                SpeedSkatingStatic.id = directionArrow.id;
            }
        }
        
    }
    private void OnBodyExitedSpeedSkating(DirectionArrow directionArrow)
    {
        if (GameModeSingleton.sport == 5 || GameModeSingleton.sport == 6)
        {
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
    #endregion
    #region Biathlon
    private void OnBodyEnteredBiathlon(DirectionArrow directionArrow)
    {
        if (GameModeSingleton.sport == 7)
        {
            if (directionArrow.enable)
            {
                BiathlonStatic.isCollided = true;
                BiathlonStatic.direction = directionArrow.direction;
                BiathlonStatic.idY = directionArrow.id;                
            }
        }

    }
    private void OnBodyExitedBiathlon(DirectionArrow directionArrow)
    {
        if (GameModeSingleton.sport == 7)
        {
            BiathlonStatic.currentIndex++;
            BiathlonStatic.idY++;
            if (directionArrow.enable)
            {
                if (!directionArrow.playerScore)
                {
                    BiathlonStatic.isNotScore = true;
                }
                directionArrow.enable = false;
                BiathlonStatic.isCollided = false;
                BiathlonStatic.direction = 0;
            }            
        }
    }
    #endregion
}
