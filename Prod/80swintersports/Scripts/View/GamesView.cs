using Godot;
using System;
using System.Linq;
using WinterSports.Scripts.Controller;
using WinterSports.Scripts.Model;
using WinterSports.Scripts.Singleton;

public partial class GamesView : Control
{    
    #region Variables
    private GamesController gamesController = null;
    private GamesModel gamesModel = null;
    private TimerController timerController = new TimerController();
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
        LoadGameplay(delta);
    }
    #endregion
    #region Methods
    private void Init()
    {
        gamesController = new GamesController();
        gamesModel = new GamesModel();
        AssignControls();
        AssignInput();
        gamesController.ShowScreen(3);
        timerController.Init();
        gamesController.InitSaveGame();
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

        gamesController.GetSetsaveExitButton = GetNode<Button>("SaveControl/ExitButton");

        for (int i = 1; i < 13; i++)
        {
            gamesModel.buttonsGame.Add(GetNode<Button>("SportsControl/SportButton" + i.ToString()));
            gamesModel.flagGame.Add(new System.Collections.Generic.List<TextureRect>());
            gamesModel.flagGame[i - 1].Add(GetNode<TextureRect>("SportsControl/SportButton" + i.ToString() + "/CountryTextureRect1"));
            gamesModel.flagGame[i - 1].Add(GetNode<TextureRect>("SportsControl/SportButton" + i.ToString() + "/CountryTextureRect2"));
            gamesModel.flagGame[i - 1].Add(GetNode<TextureRect>("SportsControl/SportButton" + i.ToString() + "/CountryTextureRect3"));
            gamesModel.flagGameLabel.Add(new System.Collections.Generic.List<Label>());
            gamesModel.flagGameLabel[i - 1].Add(GetNode<Label>("SportsControl/SportButton" + i.ToString() + "/SportTitleLabel1"));
            gamesModel.flagGameLabel[i - 1].Add(GetNode<Label>("SportsControl/SportButton" + i.ToString() + "/SportTitleLabel2"));
            gamesModel.flagGameLabel[i - 1].Add(GetNode<Label>("SportsControl/SportButton" + i.ToString() + "/SportTitleLabel3"));
            gamesModel.gamesButton.Add(GetNode<Button>("SportsControl/SportButton" + i.ToString()));
        }
        for (int i = 1; i < 17; i++)
        {
            gamesModel.sportPosLabel.Add(GetNode<Label>("MedalTableControl/ScrollContainer/VBoxContainer/MedalTable" + i.ToString() + "/SportPosLabel1"));
            gamesModel.countryTextureRect.Add(GetNode<TextureRect>("MedalTableControl/ScrollContainer/VBoxContainer/MedalTable" + i.ToString() + "/CountryTextureRect1"));
            gamesModel.sportCodeLabel.Add(GetNode<Label>("MedalTableControl/ScrollContainer/VBoxContainer/MedalTable" + i.ToString() + "/SportTitleLabel1"));
            gamesModel.sportGoldLabel.Add(GetNode<Label>("MedalTableControl/ScrollContainer/VBoxContainer/MedalTable" + i.ToString() + "/SportMedalLabel1"));
            gamesModel.sportSilverLabel.Add(GetNode<Label>("MedalTableControl/ScrollContainer/VBoxContainer/MedalTable" + i.ToString() + "/SportMedalLabel2"));
            gamesModel.sportBronzeLabel.Add(GetNode<Label>("MedalTableControl/ScrollContainer/VBoxContainer/MedalTable" + i.ToString() + "/SportMedalLabel3"));
            gamesModel.sportTotalLabel.Add(GetNode<Label>("MedalTableControl/ScrollContainer/VBoxContainer/MedalTable" + i.ToString() + "/SportMedalLabel4"));
        }

        gamesController.SetGamesModel(gamesModel);
        gamesController.HideAllMedalInfo();
        gamesController.SetResults();
        gamesController.SetMedalTable();

        gamesController.GetSetLoadingControl = GetNode<Control>("LoadingControl");
        gamesController.ShowHideLoadingControl(false);

        for (int i = 0; i < 5; i++)
        {
            gamesModel.saveButton.Add(GetNode<Button>("SaveControl/SaveSlot" + i.ToString()));
            gamesModel.saveLabel.Add(GetNode<Label>("SaveControl/SaveSlot" + i.ToString() + "/ExitNinePatchRect/SaveTitleLabel"));
        }

        gamesController.GetSetsaveWarnExitButton = GetNode<Button>("SaveWarningControl/ExitButton");
        gamesController.GetSetsaveWarnExitLabel = GetNode<Label>("SaveWarningControl/SaveConfirmLabel");        

    }
    private void AssignInput()
    {
        gamesController.GetExitButton.Pressed += () => { QuitToMainMenuScreen(); };
        gamesController.GetSaveButton.Pressed += () => { SaveMenuScreen(); };
        gamesController.GetSportsButton.Pressed += () => { SportsMenuScreen(); };
        gamesController.GetMedalTable.Pressed += () => { MedalTableMenuScreen(); };
        gamesController.GetQuitButton.Pressed += () => { QuitMenuScreen(); };
        gamesController.GetBackButton.Pressed += () => { QuitBackMenuScreen(); };
        int count = 1;
        foreach (var obj in gamesModel.gamesButton)
        {
            var countP = count;
            obj.Pressed += () => { GoToGamePlay(countP); };
            count++;
        }
        gamesController.GetSetsaveExitButton.Pressed += () => { CloseSaveMenuScreen(); };
        gamesController.GetSetsaveWarnExitButton.Pressed += () => { CloseSaveMenuScreen(); };

        int countSave = 0;
        foreach (var obj in gamesModel.saveButton)
        {
            var countP = countSave;
            obj.Pressed += () => { SaveGame(countP); };
            countSave++;
        }

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
    private void GoToGamePlay(int id)
    {
        if (!GamesSingleton.sportSingleton.Where(x => x.id == id).First().isFinished)
        {
            timerController.StartTimer();
            gamesController.ShowHideLoadingControl(true);
            GameModeSingleton.sport = id;
        }                
    }
    private void LoadGameplay(double delta)
    {
        if (timerController is null)
            return;
        timerController.TimerRunning(delta);
        if (timerController.GetTimer() > 0.5f)
        {
            GetTree().ChangeSceneToFile("res://Scenes/GamePlay.tscn");
        }
    }
    private void CloseSaveMenuScreen()
    {
        gamesController.ShowScreen(3);
    }
    public void SaveGame(int id)
    {
        gamesController.SaveGame(id);
    }
    #endregion
}
