using Godot;
using System;
using System.Linq;
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

            if((!body.GetParent().GetParent<Character>().isPlayerTeam && !this.GetParent().GetParent<Character>().isPlayerTeam) &&
                (body.GetParent().GetParent<Character>().playerNumber != this.GetParent().GetParent<Character>().playerNumber))
            {                
                //this.GetParent().GetParent<Character>().GetPlayerInput().SetisCollidedWithTeamMate(true, true);
            }

        }
        catch (Exception ex) { }
    }
    private void OnBodyExited(Node body)
    {
        try
        {
            
        }
        catch (Exception ex) { }
    }
    public override void _Ready()
    {
        Connect("area_entered", new Callable(this, nameof(OnBodyEntered)));
        Connect("area_exited", new Callable(this, nameof(OnBodyExited)));
    }
    #endregion    
}
