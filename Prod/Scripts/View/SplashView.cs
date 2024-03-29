using Godot;
using System;

public partial class SplashView : Control
{
    #region Variables
    private SplashController splashController = new SplashController();
    #endregion
    #region Behavior
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        splashController.Init();
        splashController.LoadConfig();
        splashController.LoadCountryData();
        splashController.LoadLevelData();
    }
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{        
        if (splashController.TimerCheck((float)delta))
            GetTree().ChangeSceneToFile("res://Scenes/MainScene.tscn");        
    }
    #endregion    
}
