using Godot;
using System;
using WinterSports.Scripts.Controller;

public partial class GamesView : Control
{    
    #region Variables
    private GamesController gamesController = null;
    #endregion
    #region Behaviors
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Init();        
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        
    }
    #endregion
    #region Methods
    private void Init()
    {
        gamesController = new GamesController();
        AssignControls();
        AssignInput();
        gamesController.ShowScreen(3);
    }
         
    private void AssignControls()
    {
        gamesController.SetControls(
            GetNode<Control>("QuitControl"),
            GetNode<Control>("SaveControl"),
            GetNode<Control>("SaveWarningControl"),
            GetNode<Control>("SportsControl"),
            GetNode<Control>("MedalTableControl")
            );
        gamesController.SetButtons(
            GetNode<Button>("ExitButton"),
            GetNode<Button>("SaveButton"),
            GetNode<Button>("SportsButton"),
            GetNode<Button>("MedalTable")
            );
        gamesController.SetQuitButtons(
            GetNode<Button>("QuitControl/QuitButton"),
            GetNode<Button>("QuitControl/BackButton")
            );
    }
    private void AssignInput()
    {
        gamesController.GetExitButton.Pressed += () => { QuitToMainMenuScreen(); };
        gamesController.GetSaveButton.Pressed += () => { SaveMenuScreen(); };
        gamesController.GetSportsButton.Pressed += () => { SportsMenuScreen(); };
        gamesController.GetMedalTable.Pressed += () => { MedalTableMenuScreen(); };

        gamesController.GetQuitButton.Pressed += () => { QuitMenuScreen(); };
        gamesController.GetBackButton.Pressed += () => { QuitBackMenuScreen(); };
    }
    #endregion
    #region Event
    private void QuitToMainMenuScreen()
    {
        gamesController.ShowScreen(0);
    }
    private void SaveMenuScreen()
    {
        gamesController.ShowScreen(1);
    }
    private void SportsMenuScreen()
    {
        gamesController.ShowScreen(3);
    }
    private void MedalTableMenuScreen()
    {
        gamesController.ShowScreen(4);
    }
    private void QuitMenuScreen()
    {
        gamesController.ShowScreen(3);
    }
    private void QuitBackMenuScreen()
    {
        GetTree().ChangeSceneToFile("res://Scenes/MainScene.tscn");
    }
    #endregion
}
