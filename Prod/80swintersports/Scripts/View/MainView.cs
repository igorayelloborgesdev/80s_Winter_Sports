using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Reflection;
using System.Xml.Linq;
using WinterSports.Scripts.Controller;
using WinterSports.Scripts.Singleton;

public partial class MainView : Control
{    
    #region Controllers
    private MainController mainController = new MainController();
    private ConfigController configController = new ConfigController();
    private TimerController timerController = new TimerController();
    #endregion
    #region Imports
    [Export] Control loadingControl = null;
    #endregion
    #region Behaviors
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{        
        mainController.GetSetMenuControlsNameList = GetAllMenuControls(mainController.GetMenuControlsNameList);
        mainController.GetSetMenuControlsNameList.AddRange(GetAllMenuControls(mainController.GetMenuControlsGameModeTitleNameList));
        mainController.GetSetMenuButtonsList = SetAllMenuButtons(mainController.GetMenuButtonsNameList);
        mainController.GetSetMenuModeCountryButtonsList = SetAllMenuButtons(mainController.GetMainMenuModeCountrySelectionButtonName, 17);
        SetAllMenuModeCountryEvents();
        mainController.GetSetMainMenuModeTrainningButton = SetMenuButtons(mainController.GetMainMenuModeTrainningButtonName);
        mainController.GetSetMainMenuModeGamesButton = SetMenuButtons(mainController.GetMainMenuModeGamesButtonName);
        mainController.GetSetMainMenuGoToSportsButton = SetMenuButtons(mainController.GetMainMenuGoToSportsButtonName);
        mainController.GetSetGoToDifficultButton = SetMenuButtons(mainController.GetGoToDifficultButtonName);
        mainController.GetSetBackToCountryButton = SetMenuButtons(mainController.GetBackToCountryButtonName);
        mainController.GetSetMenuModeSportButtonsList = SetAllMenuButtons(mainController.GetSportButtonName, 13);
        mainController.GetSetGoToGamePlayButton = SetMenuButtons(mainController.GetGoToGamePlayButtonName);
        mainController.GetSetBackToSportButtonButton = SetMenuButtons(mainController.GetBackToSportButtonButtonName);

        mainController.GetSetmenuLoadButtonsList = SetLoadMenuButtons();
        mainController.GetSetmenuLoadLabelList = SetLoadMenuLabels();

        SetAllMenuModeSportEvents();
        mainController.GetSetDifficultButtonButtonsList = SetAllMenuButtons(mainController.GetDifficultButtonName, 4);
        SetAllDifficultButtonsEvents();        
        SetMainMenuButtonEvents();
        SetAllMenuButtonsEvents();
        SetAllMainMenuButton();
        AssignMainMenuButtonEvent();
        mainController.GetSetMainScreenTitle = SetMainMenuLabel(mainController.GetMainScreenTitleLabel);
        mainController.GetSetLoadingControl = loadingControl;
        mainController.Init();        
        configController.GetSetMenuConfigList = SetAllMenuButtons(configController.GetMenuConfigNameList);
        SetAllConfigButtonsEvents();
        configController.GetSetMenuConfigInputList = SetAllMenuButtons(configController.GetMenuConfigInputNameLabelList);
        SetAllConfigKeyboardJoyStickControlButtonsEvents();        
        SetConfigMenuButtons();        
        SetModals();
        SetConfigMenuButtonsEvents();        
        configController.Init();
        timerController.Init();
        mainController.InitSaveGame();
        SetLoadMenuButtonsEvents();
    }
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        configController.GetKeyPress();        
        LoadGameplay(delta);
    }
    #endregion

    #region Util Methods
    private List<Control> GetAllMenuControls(string[] nameList)
    {
        var controlList = new List<Control>();
        foreach (string name in nameList)
        {
            controlList.Add(GetNode<Control>(name));
        }
        return controlList;
    }  
    private Button SetMenuButtons(string name)
    {                            
        return GetNode<Button>(name);
    }
    private List<Button> SetAllMenuButtons(string[] nameList)
    {
        var buttonList = new List<Button>();
        for (int i = 0; i < nameList.Length; i++)
        {            
            buttonList.Add(GetNode<Button>(nameList[i]));
        }
        return buttonList;
    }
    private List<Button> SetAllMenuButtons(string nameObj, int count)
    {
        var buttonList = new List<Button>();
        for (int i = 1; i < count; i++)
        {
            buttonList.Add(GetNode<Button>(nameObj + i.ToString()));
        }
        return buttonList;
    }
    private void SetAllMenuButtonsEvents()
    {
        int count = 0;
        foreach (var item in mainController.GetSetMenuButtonsList)
        {
            int countNew = count;
            item.Pressed += () => { ShowHidePanels(countNew); };
            count++;            
        }        
    }
    private void SetLoadMenuButtonsEvents()
    {
        int count = 0;
        foreach (var item in mainController.GetSetmenuLoadButtonsList)
        {
            int countNew = count;
            item.Pressed += () => { LoadGame(countNew); };
            count++;
        }
    }

    private void SetAllDifficultButtonsEvents()
    {
        int count = 0;
        foreach (var item in mainController.GetSetDifficultButtonButtonsList)
        {
            int countNew = count;
            item.Pressed += () => { SetDifficult(countNew); };
            count++;
        }
    }
    private void SetAllMainMenuButton()
    {
        mainController.GetSetQuitButton = SetMainMenuButton(mainController.GetQuitName);
    }
    private Button SetMainMenuButton(string name)
    {
        return GetNode<Button>(name);
    }
    private Label SetMainMenuLabel(string name)
    {
        return GetNode<Label>(name);
    }
    private Control SetControl(string name)
    {
        return GetNode<Control>(name);
    }
    private void AssignMainMenuButtonEvent()
    {
        mainController.GetSetQuitButton.Pressed += () => { QuitGame(); };
    }
    private void SetAllConfigButtonsEvents()
    {
        int count = 0;
        foreach (var item in configController.GetSetMenuConfigList)
        {
            int countNew = count;
            item.Pressed += () => { AssignControl(countNew); };
            count++;
        }
    }
    private void SetAllConfigKeyboardJoyStickControlButtonsEvents()
    {
        int count = 0;
        foreach (var item in configController.GetSetMenuConfigInputList)
        {
            int countNew = count;
            item.Pressed += () => { AssignConfigKeyboardJoyStickControl(countNew); };
            count++;
        }
    }
    private void SetConfigMenuButtons()
    {
        configController.GetSetConfigSaveButton = SetMenuButtons(configController.GetSaveButtonName);
        configController.GetSetConfigRestoreButton = SetMenuButtons(configController.GetRestoreButtonName);
    }
    private void SetConfigMenuButtonsEvents()
    {
        configController.GetSetConfigSaveButton.Pressed += () => { SaveConfig(); };
        configController.GetSetModalSimple.GetSetSimpleModalButton.Pressed += () => { ConfigSaveCloseModal(); };
        configController.GetSetConfigRestoreButton.Pressed += () => { RestoreConfig(); };
    }
    private void SetModals()
    {
        var modalSimple = new ModalSimple().SetModalBuilder(
                SetControl(ModalMainMenuModel.simpleModalControlName),
                SetMainMenuLabel(ModalMainMenuModel.simpleModalTitleLabelName),
                SetMainMenuLabel(ModalMainMenuModel.simpleModalContextLabelName),
                SetMainMenuButton(ModalMainMenuModel.simpleModalButtonName)
            );        
        configController.ModalSimpleBuilder(modalSimple);
    }

    private List<Button> SetLoadMenuButtons()
    {
        var buttonList = new List<Button>();
        for (int i = 0; i < 5; i++)
        {
            buttonList.Add(GetNode<Button>("LoadControl/SaveSlot" + i.ToString()));
        }
        return buttonList;
    }
    private List<Label> SetLoadMenuLabels()
    {
        var buttonList = new List<Label>();
        for (int i = 0; i < 5; i++)
        {
            buttonList.Add(GetNode<Label>("LoadControl/SaveSlot" + i.ToString() + "/ExitNinePatchRect/SaveTitleLabel"));
        }
        return buttonList;
    }
    #endregion
    #region Events
    private void SetMainMenuButtonEvents()
    {
        mainController.GetSetMainMenuModeTrainningButton.Pressed += () => { SelectGameMode(0); };
        mainController.GetSetMainMenuGoToSportsButton.Pressed += () => { GoToSportsSelection(); };
        mainController.GetSetGoToDifficultButton.Pressed += () => { GoToDifficult(); };
        mainController.GetSetBackToCountryButton.Pressed += () => { BackToCountry(); };
        mainController.GetSetGoToGamePlayButton.Pressed += () => { GotoGamePlay(); };
        mainController.GetSetBackToSportButtonButton.Pressed += () => { BackToSport(); };
        mainController.GetSetMainMenuModeGamesButton.Pressed += () => { SelectGameMode(1); };
    }
    private void SetAllMenuModeCountryEvents()
    {
        int count = 1;
        foreach (var item in mainController.GetSetMenuModeCountryButtonsList)
        {
            int countNew = count;
            item.Pressed += () => { SelectCountry(countNew); };
            count++;
        }
    }
    private void SetAllMenuModeSportEvents()
    {
        int count = 1;
        foreach (var item in mainController.GetSetMenuModeSportButtonsList)
        {
            int countNew = count;
            item.Pressed += () => { SelectSport(countNew); };
            count++;
        }
    }
    private void ShowHidePanels(int id)
    {
        mainController.ShowHidePanels(id);
        mainController.SetTitleText(id);
        configController.DismissAssign();
    }
    private void LoadGame(int id)
    {
        var result = mainController.LoadGame(id);
        if (GameModeSingleton.gameMode == 1)
        {        
            GetTree().ChangeSceneToFile("res://Scenes/Games.tscn");
        }
    }

    private void AssignControl(int id)
    {        
        configController.AssignControl(id);
    }
    private void AssignConfigKeyboardJoyStickControl(int id)
    {
        configController.AssignConfigKeyboardJoyStickControl(id);
    }
    private void QuitGame()
    {
        GetTree().Quit();
    }
    private void SaveConfig()
    {
        configController.SaveConfig();
    }
    private void ConfigSaveCloseModal()
    {
        configController.ConfigSaveCloseModal();
    }
    private void RestoreConfig()
    {
        configController.RestoreConfigEvent();
    }
    private void GoToCountrySelection(int id, int gameModeID)
    {
        ShowHidePanels(id);        
    }    
    private void SelectCountry(int id)
    {
        mainController.SelectCountry(id);
    }
    private void SelectSport(int id)
    {        
        mainController.SelectSport(id);        
    }
    private void GoToSportsSelection()
    {        
        if(GameModeSingleton.gameMode == 0)
            ShowHidePanels(mainController.GetSetMenuControlsNameList.Count - 1);        
        else
            ShowHidePanels(mainController.GetSetMenuControlsNameList.Count - 2);        
    }
    private void GoToDifficult()
    {
        GoToCountrySelection(mainController.GetSetMenuControlsNameList.Count - 2, 1);        
    }
    private void BackToCountry()
    {
        GoToCountrySelection(mainController.GetSetMenuControlsNameList.Count - 3, 1);        
    }
    private void GotoGamePlay()
    {
        timerController.StartTimer();        
        mainController.ShowHideLoadingControl(true);        
    }
    private void LoadGameplay(double delta)
    {        
        timerController.TimerRunning(delta);
        if (timerController.GetTimer() > 0.5f)
        {
            if (GameModeSingleton.gameMode == 0)
                GetTree().ChangeSceneToFile("res://Scenes/GamePlay.tscn");
            else if (GameModeSingleton.gameMode == 1)
            {
                GamesSingleton.Init();
                GetTree().ChangeSceneToFile("res://Scenes/Games.tscn");
            }            
        }
    }
    private void BackToSport()
    {
        if(GameModeSingleton.gameMode == 0)
            ShowHidePanels(mainController.GetSetMenuControlsNameList.Count - 1);
        else if (GameModeSingleton.gameMode == 1)
            ShowHidePanels(6);
    }
    private void SetDifficult(int id)
    {
        mainController.SelectDiffcult(id);
    }

    private void SelectGameMode(int gameModeID)
    {
        GoToCountrySelection(mainController.GetSetMenuControlsNameList.Count - 3, 0);
        GameModeSingleton.gameMode = gameModeID;
    }
    #endregion
}
