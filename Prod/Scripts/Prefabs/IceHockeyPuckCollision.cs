using Godot;
using System;

public partial class IceHockeyPuckCollision : Area3D
{
    public override void _Ready()
    {
        Connect("area_entered", new Callable(this, nameof(OnBodyEntered)));
        //Connect("area_exited", new Callable(this, nameof(OnBodyExited)));
    }
    private void OnBodyEntered(Node body)
    {
        try
        {
            GD.Print("-----------------------------");
            GD.Print(body.GetParent().GetParent().Name);            
            
            var character = body.GetParent().GetParent<Character>();            
            body.GetParent().GetParent<Character>().GetPlayerInput().ResetControlAndSelected();
            body.GetParent().GetParent<Character>().isSelected = true;
            body.GetParent().GetParent<Character>().isPuckControl = true;
            if (body.GetParent().GetParent<Character>().isPlayerTeam)
            {
                body.GetParent().GetParent<Character>().GetPlayerInput().SetisSelected(ref body.GetParent().GetParent<Character>().isSelected);
                body.GetParent().GetParent<Character>().GetPlayerInput().SetisPuckControl(ref body.GetParent().GetParent<Character>().isPuckControl);
            }            
            body.GetParent().GetParent<Character>().SetPuckOriginalTransform();
            if (body.GetParent().GetParent<Character>().isPlayerTeam)
                body.GetParent().GetParent<Character>().ShowHideIceHockeySeletion(true);            
        }
        catch (Exception ex) {
            var msg = ex.Message;
        }
    }
}
