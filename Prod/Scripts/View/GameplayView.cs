using Godot;
using System;
using System.Collections.Generic;
using WinterSports.Scripts.Controller;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Prefabs;
using WinterSports.Scripts.Singleton;
using WinterSports.Scripts.Static;

public partial class GameplayView : Control
{
    #region Exports
    [Export] private PackedScene characterPackedScene = null;
    [Export] private MeshInstance3D initPoint = null;
    [Export] private GateStartFinish gateStart = null;
    [Export] private GateStartFinish gateFinish = null;
    [Export] private Control pauseScreen = null;
    [Export] Button backMenuButton = null;
    [Export] Button returnMenuButton = null;
    [Export] Button resetMenuButton = null;
    [Export] Label timeLabel = null;
    [Export] NinePatchRect speedNinePatchRect = null;
    [Export] Control readySetGoControl = null;
    [Export] Label readySetGoLabel = null;
    [Export] Label countryCodeLabel = null;
    [Export] TextureRect countryFlagTextureRect = null;
    [Export] private Control finishSessionScreen = null;
    [Export] Button backMenuFinishButton = null;
    [Export] Button returnFinishButton = null;
    [Export] Label countryCodeLabelFinish = null;
    [Export] TextureRect countryFlagTextureRectFinish = null;
    [Export] Label timeScoreBestLabelFinish = null;
    [Export] Label timeScoreLastLabelFinish = null;
    [Export] Control controlSkiSpeedSkatingBiathlon = null;
    [Export] Control controlBiathlon = null;
    [Export] Label shoots = null;
    [Export] Label errorLabelScore = null;
    [Export] Label windDirection = null;
    [Export] Control windDirectionArrow = null;
    #endregion
    #region MeshInstance3D
    MeshInstance3D characterMeshInstance3D = null;
    #endregion
    #region Variables
    private Character character = new Character();
    private Ski skiTrack = new Ski();
    private SpeedSkating speedSkatingTrack = new SpeedSkating();
    private Biathlon biathlonTrack = new Biathlon();
    private int levelId = 0;
    private string prefabName = String.Empty;
    #endregion
    #region Controller
    private GamePlayController gamePlayController = null;
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
        gamePlayController.Update(delta, prefabName);
    }
    #endregion
    #region Method
    private void Init()
    {
        levelId = GameModeSingleton.sport - 1;
        prefabName = LevelSingleton.levelObjDTO.levelList[levelId].prefabName;
        gamePlayController = new GamePlayController();
        InstantiateLevel();
        AssignButtons();
        SetMainGamePlayEvents();
        gamePlayController.Init(prefabName);
        gamePlayController.SetTimerLabel(prefabName, timeLabel);
        gamePlayController.SetSpeedLabel(prefabName, speedNinePatchRect);
        gamePlayController.SetReadySetGoControl(prefabName, readySetGoControl, readySetGoLabel);
        gamePlayController.SetCountryUI(prefabName, countryCodeLabel, countryFlagTextureRect);
        gamePlayController.SetCountryUIFinishScreen(prefabName, countryCodeLabelFinish, countryFlagTextureRectFinish);
        gamePlayController.SetTimeScoreBestLastLabelFinish(timeScoreBestLabelFinish, timeScoreLastLabelFinish);
        SetDirectionArrowList();
        InstantiateCharacter();
        InitStaticVariables();
        ReturnMenu();
    }
    private void SetDirectionArrowList()
    {
        if (prefabName == "SpeedSkating")
            gamePlayController.SetDirectionArrowList(speedSkatingTrack.GetDirectionArrowList);
        if (prefabName == "Biathlon")
            gamePlayController.SetDirectionArrowList(biathlonTrack.GetDirectionArrowList);
    }
    private void InstantiateCharacter()
    {        
        if (prefabName == "skiTrack")
            InstantiateCharacterSki();
        if (prefabName == "SpeedSkating")
            InstantiateCharacterSpeedSkating();
        if (prefabName == "Biathlon")
            InstantiateCharacterBiathlon();
    }
    private void InstantiateCharacterSki()
    {
        character = characterPackedScene.Instantiate<Character>();
        character.SetPrefabName = prefabName;
        gamePlayController.SetDefaultPositionRotation(initPoint.Position, initPoint.Rotation);
        gamePlayController.SetCharacter(character);
        gamePlayController.SetCharacterSportSki(gateStart, gateFinish);
        gamePlayController.SetPauseScreen(pauseScreen);
        gamePlayController.SetFinishSessionScreen(finishSessionScreen);
        gamePlayController.SetControlSkiSpeedSkatingBiathlon(controlSkiSpeedSkatingBiathlon);
        gamePlayController.SetControlBiathlon(controlBiathlon);
        character.ShowHideControlSkiSpeedSkatingBiathlon(true);
        character.ShowHideControlBiathlon(false);
        this.AddChild(character);
    }
    private void InstantiateCharacterSpeedSkating()
    {
        character = characterPackedScene.Instantiate<Character>();
        character.SetPrefabName = prefabName;
        character.SetDirectionArrowList(speedSkatingTrack.GetDirectionArrowList);
        gamePlayController.SetDefaultPositionRotation(initPoint.Position, initPoint.Rotation);        
        gamePlayController.SetCharacter(character);
        gamePlayController.SetCharacterSpeedSkating();
        gamePlayController.SetPauseScreen(pauseScreen);
        gamePlayController.SetFinishSessionScreen(finishSessionScreen);
        gamePlayController.SetControlSkiSpeedSkatingBiathlon(controlSkiSpeedSkatingBiathlon);
        gamePlayController.SetControlBiathlon(controlBiathlon);
        gamePlayController.SetRailSpeedSkating(speedSkatingTrack.GetStartPointId, speedSkatingTrack.GetSpeedSkatingTrackDTOList);
        character.ShowHideControlSkiSpeedSkatingBiathlon(true);
        character.ShowHideControlBiathlon(false);
        this.AddChild(character);        
    }
    private void InstantiateCharacterBiathlon()
    {
        character = characterPackedScene.Instantiate<Character>();
        character.SetPrefabName = prefabName;
        character.SetDirectionArrowList(biathlonTrack.GetDirectionArrowList);
        gamePlayController.SetDefaultPositionRotation(initPoint.Position, initPoint.Rotation);
        gamePlayController.SetCharacter(character);
        gamePlayController.SetCharacterBiathlon();
        gamePlayController.SetPauseScreen(pauseScreen);
        gamePlayController.SetFinishSessionScreen(finishSessionScreen);
        gamePlayController.SetControlSkiSpeedSkatingBiathlon(controlSkiSpeedSkatingBiathlon);
        gamePlayController.SetControlBiathlon(controlBiathlon);
        gamePlayController.SetRailBiathlon(biathlonTrack.GetStartPointId, biathlonTrack.GetBiathlonTrackDTOList);
        character.ShowHideControlSkiSpeedSkatingBiathlon(true);
        character.ShowHideControlBiathlon(false);
        character.SetBiathlonUILabels(shoots, errorLabelScore, windDirection, windDirectionArrow);
        this.AddChild(character);
    }
    private void AssignButtons()
    {
        gamePlayController.GetSetGoToMainMenu = backMenuButton;
        gamePlayController.GetSetReturnMenu = returnMenuButton;
        gamePlayController.GetSetResetMenu = resetMenuButton;
        gamePlayController.GetSetBackMenuFinishButton = backMenuFinishButton;
        gamePlayController.GetSetReturnFinishButton = returnFinishButton;
    }
    private void SetMainGamePlayEvents()
    {
        gamePlayController.GetSetGoToMainMenu.Pressed += () => { QuitToMainMenu(); };
        gamePlayController.GetSetReturnMenu.Pressed += () => { ReturnMenu(); };
        gamePlayController.GetSetResetMenu.Pressed += () => { ResetGameMenu(); };
        gamePlayController.GetSetBackMenuFinishButton.Pressed += () => { QuitToMainMenu(); };
        gamePlayController.GetSetReturnFinishButton.Pressed += () => { ReturnMenuFromFinishScreen(); };
    }
    private void InstantiateLevel()
    {                
        PackedScene prefabScene = (PackedScene)ResourceLoader.Load("res://Prefab/" + LevelSingleton.levelObjDTO.levelList[levelId].prefabName + ".tscn");
        if (prefabName == "skiTrack")
            InstantiateLevelSki(prefabScene, levelId);
        if (prefabName == "SpeedSkating")
            InstantiateLevelSpeedSkating(prefabScene, levelId - 4);
        if (prefabName == "Biathlon")
            InstantiateLevelBiathlon(prefabScene);
    }
    private void InstantiateLevelSki(PackedScene prefabScene, int id)
    {
        skiTrack = prefabScene.Instantiate<Ski>();
        initPoint = skiTrack.GetInitPoint(id);
        gateStart = skiTrack.GetStart(id);
        gateFinish = skiTrack.GetFinish(id);
        skiTrack.ShowGate(id);
        AddChild(skiTrack);
    }
    private void InstantiateLevelSpeedSkating(PackedScene prefabScene, int id)
    {
        speedSkatingTrack = prefabScene.Instantiate<SpeedSkating>();
        initPoint = speedSkatingTrack.GetSetInitPoint(id);
        speedSkatingTrack.InstantiateRail();        
        AddChild(speedSkatingTrack);
        gamePlayController.SetLapCount(speedSkatingTrack.GetLaps(id));
    }
    private void InstantiateLevelBiathlon(PackedScene prefabScene)
    {
        biathlonTrack = prefabScene.Instantiate<Biathlon>();
        initPoint = biathlonTrack.GetSetInitPoint();
        biathlonTrack.InstantiateRail();
        gamePlayController.SetBiathlonUILabels(shoots, errorLabelScore, windDirection, windDirectionArrow);        
        AddChild(biathlonTrack);
    }
    #endregion
    #region Events
    private void QuitToMainMenu()
    {
        Engine.TimeScale = 1.0f;
        GetTree().ChangeSceneToFile("res://Scenes/MainScene.tscn");
    }
    private void ReturnMenu()
    {
        gamePlayController.UnPause();
    }
    private void ReturnMenuFromFinishScreen()
    {
        gamePlayController.ShowHideFinishSessionScreen();
    }
    private void ResetGameMenu()
    {        
        if (prefabName == "skiTrack")
            gamePlayController.Reset();
        if (prefabName == "SpeedSkating")
        {            
            gamePlayController.Reset();
            gamePlayController.ResetSpeedSkating();
        }        
        gamePlayController.UnPause();
    }
    private void InitStaticVariables()
    {        
        SkiStatic.Reset();
        SpeedSkatingStatic.Reset();
        BiathlonStatic.Reset();
    }
    #endregion

}
