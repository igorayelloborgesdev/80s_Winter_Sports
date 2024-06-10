using Godot;
using System;
using System.Collections.Generic;
using WinterSports.Scripts.Controller;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Prefabs;
using WinterSports.Scripts.Singleton;

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
    #endregion
    #region MeshInstance3D
    MeshInstance3D characterMeshInstance3D = null;
    #endregion
    #region Variables
    private Character character = new Character();
    private Ski skiTrack = new Ski();
    private SpeedSkating speedSkatingTrack = new SpeedSkating();
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
        gamePlayController.Update(delta, prefabName);//<-
    }
    #endregion
    #region Method
    private void Init()
    {
        levelId = GameModeSingleton.sport - 1;
        prefabName = LevelSingleton.levelObjDTO.levelList[levelId].prefabName;
        gamePlayController = new GamePlayController();
        InstantiateLevel();//<-        
        AssignButtons();
        SetMainGamePlayEvents();
        gamePlayController.Init(prefabName);
        gamePlayController.SetTimerLabel(prefabName, timeLabel);
        gamePlayController.SetSpeedLabel(prefabName, speedNinePatchRect);
        gamePlayController.SetReadySetGoControl(prefabName, readySetGoControl, readySetGoLabel);
        gamePlayController.SetCountryUI(prefabName, countryCodeLabel, countryFlagTextureRect);
        gamePlayController.SetDirectionArrowList(speedSkatingTrack.GetDirectionArrowList);
        InstantiateCharacter();
        ReturnMenu();
    }
    private void InstantiateCharacter()
    {        
        if (prefabName == "skiTrack")
            InstantiateCharacterSki();
        if (prefabName == "SpeedSkating")
            InstantiateCharacterSpeedSkating();
    }
    private void InstantiateCharacterSki()
    {
        character = characterPackedScene.Instantiate<Character>();
        character.SetPrefabName = prefabName;
        gamePlayController.SetDefaultPositionRotation(initPoint.Position, initPoint.Rotation);
        gamePlayController.SetCharacter(character);
        gamePlayController.SetCharacterSportSki(gateStart, gateFinish);
        gamePlayController.SetPauseScreen(pauseScreen);        
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
        gamePlayController.SetRailSpeedSkating(speedSkatingTrack.GetStartPointId, speedSkatingTrack.GetSpeedSkatingTrackDTOList);
        this.AddChild(character);        
    }
    private void AssignButtons()
    {
        gamePlayController.GetSetGoToMainMenu = backMenuButton;
        gamePlayController.GetSetReturnMenu = returnMenuButton;
        gamePlayController.GetSetResetMenu = resetMenuButton;
    }
    private void SetMainGamePlayEvents()
    {
        gamePlayController.GetSetGoToMainMenu.Pressed += () => { QuitToMainMenu(); };
        gamePlayController.GetSetReturnMenu.Pressed += () => { ReturnMenu(); };
        gamePlayController.GetSetResetMenu.Pressed += () => { ResetGameMenu(); };
    }
    private void InstantiateLevel()
    {                
        PackedScene prefabScene = (PackedScene)ResourceLoader.Load("res://Prefab/" + LevelSingleton.levelObjDTO.levelList[levelId].prefabName + ".tscn");
        if (prefabName == "skiTrack")
            InstantiateLevelSki(prefabScene, levelId);
        if (prefabName == "SpeedSkating")
            InstantiateLevelSpeedSkating(prefabScene, levelId - 4);//<-
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
    private void ResetGameMenu()
    {
        gamePlayController.Reset();
        gamePlayController.UnPause();
    }
    #endregion

}
