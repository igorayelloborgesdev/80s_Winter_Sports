using Godot;
using System;
using System.Collections.Generic;
using WinterSports.Scripts.Controller;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Model;
using WinterSports.Scripts.Prefabs;
using WinterSports.Scripts.Singleton;
using WinterSports.Scripts.Static;

public partial class GameplayView : Control
{
    #region Exports
    [Export] private PackedScene characterPackedScene = null;
    [Export] private PackedScene lugePackedScene = null;
    [Export] private PackedScene bobsleighPackedScene = null;
    [Export] private MeshInstance3D initPoint = null;
    [Export] private GateStartFinish gateStart = null;
    [Export] private GateStartFinish gateFinish = null;
    [Export] private Control pauseScreen = null;
    [Export] Button backMenuButton = null;
    [Export] Button returnMenuButton = null;
    [Export] Button resetMenuButton = null;
    [Export] Label timeLabel = null;
    [Export] NinePatchRect speedNinePatchRect = null;
    [Export] NinePatchRect impulseNinePatchRect = null;
    [Export] NinePatchRect impulseSkiJumpNinePatchRect = null;
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
    [Export] Control controlSkiSpeedSkatingScreen = null;
    [Export] Control controlBiathlonScreen = null;
    [Export] Control controlLugeImpulse = null;
    [Export] Control controlSkiJumpImpulse = null;
    [Export] Label bestScoreLabel = null;
    [Export] Label timeScoreLabel = null;
    [Export] Label errorsScoreLabel = null;
    [Export] Label lastScoreLabel = null;
    [Export] Control controlSkiJump = null;
    [Export] Label windDirectionSkiJump = null;
    [Export] Control windDirectionArrowSkiJump = null;
    [Export] Control controlSkiJumpImpulseHorizontal = null;
    [Export] Control windDirectionArrowHorizontal = null;
    [Export] Control controlSkiJumpImpulseVertical = null;
    [Export] Control windDirectionArrowVertical = null;
    [Export] Control controlSkiCrossCountry = null;
    [Export] Label controlSkiCrossCountryTime = null;
    [Export] NinePatchRect controlSkiCrossCountrySpeed = null;
    [Export] NinePatchRect controlSkiCrossCountryEnergy = null;
    #endregion
    #region MeshInstance3D
    MeshInstance3D characterMeshInstance3D = null;
    #endregion
    #region Variables
    private Character character = new Character();
    private List<Character> characterList = new List<Character>();
    private LugeSled lugeSled = new LugeSled();
    private BobsleighSled bobsleighSled = new BobsleighSled();
    private Ski skiTrack = new Ski();
    private SpeedSkating speedSkatingTrack = new SpeedSkating();
    private Biathlon biathlonTrack = new Biathlon();
    private LugeBobsleigh lugeBobsleigh = new LugeBobsleigh();
    private SkiJump skiJump = new SkiJump();
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
        gamePlayController.UpdateSkiJumpRail(prefabName, skiJump);
        gamePlayController.Update(delta, prefabName, levelId);                
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
        gamePlayController.SetCrossCountryLabel(controlSkiCrossCountryTime, controlSkiCrossCountrySpeed, controlSkiCrossCountryEnergy);
        gamePlayController.SetSpeedLabel(prefabName, speedNinePatchRect);
        gamePlayController.SetReadySetGoControl(prefabName, readySetGoControl, readySetGoLabel);
        gamePlayController.SetCountryUI(prefabName, countryCodeLabel, countryFlagTextureRect);
        gamePlayController.SetCountryUIFinishScreen(prefabName, countryCodeLabelFinish, countryFlagTextureRectFinish);
        gamePlayController.SetTimeScoreBestLastLabelFinish(timeScoreBestLabelFinish, timeScoreLastLabelFinish);
        gamePlayController.SetTimeScoreBestLastLabelFinishBiathlon(bestScoreLabel, timeScoreLabel, errorsScoreLabel, lastScoreLabel);
        gamePlayController.SetTimeScreenControl(controlSkiSpeedSkatingScreen, controlBiathlonScreen, controlLugeImpulse, controlSkiJumpImpulse, controlSkiJump);
        gamePlayController.SetImpulseLabel(impulseNinePatchRect, impulseSkiJumpNinePatchRect);
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
        {
            if (levelId == 11)
            {
                InstantiateCharacterSkiCrossCountry();
                InstantiateCharacterSkiCrossCountryAI();
            }
            else
            {
                InstantiateCharacterSki();
            }            
        }        
        if (prefabName == "SpeedSkating")
            InstantiateCharacterSpeedSkating();
        if (prefabName == "Biathlon")
            InstantiateCharacterBiathlon();
        if (prefabName == "LugeBobsleigh")
            InstantiateCharacterLugeBobsleigh();
        if (prefabName == "Skijumping")
            InstantiateCharacterSkiJump();
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
        gamePlayController.SetControlSkiCrossCountry(controlSkiCrossCountry);
        gamePlayController.SetControlBiathlon(controlBiathlon);
        gamePlayController.SetControlSkiJumpImpulseHorizontal(controlSkiJumpImpulseHorizontal, windDirectionArrowHorizontal);
        gamePlayController.SetControlSkiJumpImpulseVertical(controlSkiJumpImpulseVertical, windDirectionArrowVertical);
        gamePlayController.ShowHideControlLugeImpulse(false);
        gamePlayController.ShowHideControlSkiJumpImpulse(false);
        gamePlayController.ShowHideControlSkiJump(false);
        gamePlayController.ShowHideControlSkiJumpImpulseHorizontal(false);
        gamePlayController.ShowHideControlSkiJumpImpulseVertical(false);
        character.ShowHideControlSkiSpeedSkatingBiathlon(true);
        character.ShowHideControlBiathlon(false);
        this.AddChild(character);
    }
    private void InstantiateCharacterSkiCrossCountry()
    {
        character = characterPackedScene.Instantiate<Character>();
        character.SetPrefabName = prefabName;
        gamePlayController.SetDefaultPositionRotation(initPoint.Position, initPoint.Rotation);
        gamePlayController.SetCharacter(character);
        gamePlayController.SetCharacterSportSki(gateStart, gateFinish);
        gamePlayController.SetPauseScreen(pauseScreen);
        gamePlayController.SetFinishSessionScreen(finishSessionScreen);
        gamePlayController.SetControlSkiSpeedSkatingBiathlon(controlSkiSpeedSkatingBiathlon);
        gamePlayController.SetControlSkiCrossCountry(controlSkiCrossCountry);
        gamePlayController.SetControlBiathlon(controlBiathlon);
        gamePlayController.SetControlSkiJumpImpulseHorizontal(controlSkiJumpImpulseHorizontal, windDirectionArrowHorizontal);
        gamePlayController.SetControlSkiJumpImpulseVertical(controlSkiJumpImpulseVertical, windDirectionArrowVertical);
        gamePlayController.ShowHideControlLugeImpulse(false);
        gamePlayController.ShowHideControlSkiJumpImpulse(false);
        gamePlayController.ShowHideControlSkiJump(false);
        gamePlayController.ShowHideControlSkiJumpImpulseHorizontal(false);
        gamePlayController.ShowHideControlSkiJumpImpulseVertical(false);
        character.ShowHideControlSkiSpeedSkatingBiathlon(true);
        character.ShowHideControlBiathlon(false);        
        character.SetCrossCountryModel(skiTrack.GetCrossCountryModel());
        this.AddChild(character);
    }
    private void InstantiateCharacterSkiCrossCountryAI()
    {
        int count = 1;
        foreach (var obj in CountrySingleton.countryObjDTO.countryList)
        {
            //if (obj.Id != GameModeSingleton.country)
            //{
            //    Character characterAI = characterPackedScene.Instantiate<Character>();
            //    characterAI.SetPrefabName = prefabName;
            //    MeshInstance3D initPointAI = skiTrack.GetInitPointCrossCountry(count);                
            //    gamePlayController.SetCharacterCrossCountryAI(characterAI, initPointAI.Position, initPointAI.Rotation, obj.Id - 1);
            //    gamePlayController.SetCharacterSportSkiCrossCountryAI(gateStart, gateFinish);
            //    characterAI.SetCrossCountryModel(skiTrack.GetCrossCountryModel());
            //    characterList.Add(characterAI);
            //    this.AddChild(characterAI);                                
            //    count++;
            //}
            if (obj.Id != GameModeSingleton.country && obj.Id == 5)
            {
                Character characterAI = characterPackedScene.Instantiate<Character>();
                characterAI.SetPrefabName = prefabName;
                MeshInstance3D initPointAI = skiTrack.GetInitPointCrossCountry(4);
                gamePlayController.SetCharacterCrossCountryAI(characterAI, initPointAI.Position, initPointAI.Rotation, obj.Id - 1);
                gamePlayController.SetCharacterSportSkiCrossCountryAI(gateStart, gateFinish);
                characterAI.SetCrossCountryModel(skiTrack.GetCrossCountryModel());
                characterList.Add(characterAI);
                this.AddChild(characterAI);                
            }
        }
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
        gamePlayController.SetControlSkiCrossCountry(controlSkiCrossCountry);
        gamePlayController.SetControlBiathlon(controlBiathlon);
        gamePlayController.SetControlSkiJumpImpulseHorizontal(controlSkiJumpImpulseHorizontal, windDirectionArrowHorizontal);
        gamePlayController.SetControlSkiJumpImpulseVertical(controlSkiJumpImpulseVertical, windDirectionArrowVertical);
        gamePlayController.SetRailSpeedSkating(speedSkatingTrack.GetStartPointId, speedSkatingTrack.GetSpeedSkatingTrackDTOList);
        gamePlayController.ShowHideControlLugeImpulse(false);        
        gamePlayController.ShowHideControlSkiJumpImpulse(false);
        gamePlayController.ShowHideControlSkiJump(false);
        gamePlayController.ShowHideControlSkiJumpImpulseHorizontal(false);
        gamePlayController.ShowHideControlSkiJumpImpulseVertical(false);
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
        gamePlayController.SetControlSkiCrossCountry(controlSkiCrossCountry);
        gamePlayController.SetControlBiathlon(controlBiathlon);
        gamePlayController.SetControlSkiJumpImpulseHorizontal(controlSkiJumpImpulseHorizontal, windDirectionArrowHorizontal);
        gamePlayController.SetControlSkiJumpImpulseVertical(controlSkiJumpImpulseVertical, windDirectionArrowVertical);
        gamePlayController.SetRailBiathlon(biathlonTrack.GetStartPointId, biathlonTrack.GetBiathlonTrackDTOList);
        gamePlayController.ShowHideControlLugeImpulse(false);        
        gamePlayController.ShowHideControlSkiJumpImpulse(false);
        gamePlayController.ShowHideControlSkiJump(false);
        gamePlayController.ShowHideControlSkiJumpImpulseHorizontal(false);
        gamePlayController.ShowHideControlSkiJumpImpulseVertical(false);
        character.ShowHideControlSkiSpeedSkatingBiathlon(true);
        character.ShowHideControlBiathlon(false);
        character.SetBiathlonUILabels(shoots, errorLabelScore, windDirection, windDirectionArrow);
        this.AddChild(character);
    }
    private void InstantiateCharacterLugeBobsleigh()
    {        
        if (levelId == 7)
        {
            lugeSled = lugePackedScene.Instantiate<LugeSled>();
            lugeSled.SetPrefabName(prefabName);
            gamePlayController.SetDefaultPositionRotation(initPoint.Position, initPoint.Rotation);
            gamePlayController.SetCharacter(lugeSled);
            gamePlayController.SetPauseScreenLuge(pauseScreen);
            gamePlayController.SetFinishSessionScreenLuge(finishSessionScreen);
            gamePlayController.SetControlSkiSpeedSkatingBiathlonLuge(controlSkiSpeedSkatingBiathlon, controlBiathlon);
            gamePlayController.SetControlSkiCrossCountry(controlSkiCrossCountry);
            gamePlayController.SetControlSkiJumpImpulseHorizontal(controlSkiJumpImpulseHorizontal, windDirectionArrowHorizontal);
            gamePlayController.SetControlSkiJumpImpulseVertical(controlSkiJumpImpulseVertical, windDirectionArrowVertical);
            gamePlayController.ShowHideControlLugeImpulse(true);
            gamePlayController.ShowHideControlSkiJumpImpulse(false);
            gamePlayController.ShowHideControlSkiJump(false);
            gamePlayController.ShowHideControlSkiJumpImpulseHorizontal(false);
            gamePlayController.ShowHideControlSkiJumpImpulseVertical(false);
            lugeSled.GetSetStartPointIdInit = lugeBobsleigh.GetSetStartPointId;
            lugeSled.GetSetStartPointId = lugeBobsleigh.GetSetStartPointId;
            lugeSled.SetSpeedSkatingTrackDTOList = lugeBobsleigh.GetLugeTrackDTOList;
            lugeSled.ShowHideControlLuge();
            this.AddChild(lugeSled);
        }
        else 
        {
            bobsleighSled = bobsleighPackedScene.Instantiate<BobsleighSled>();
            bobsleighSled.SetPrefabName(prefabName);
            gamePlayController.SetDefaultPositionRotation(initPoint.Position, initPoint.Rotation);            
            gamePlayController.SetCharacter(bobsleighSled);
            gamePlayController.SetBobsleigh();
            gamePlayController.SetPauseScreenBobsleigh(pauseScreen);
            gamePlayController.SetFinishSessionScreenBobsleigh(finishSessionScreen);
            gamePlayController.SetControlSkiSpeedSkatingBiathlonBobsleigh(controlSkiSpeedSkatingBiathlon, controlBiathlon);
            gamePlayController.SetControlSkiCrossCountry(controlSkiCrossCountry);
            gamePlayController.SetControlSkiJumpImpulseHorizontal(controlSkiJumpImpulseHorizontal, windDirectionArrowHorizontal);
            gamePlayController.SetControlSkiJumpImpulseVertical(controlSkiJumpImpulseVertical, windDirectionArrowVertical);
            gamePlayController.ShowHideControlLugeImpulse(true);
            gamePlayController.ShowHideControlSkiJumpImpulse(false);
            gamePlayController.ShowHideControlSkiJump(false);
            gamePlayController.ShowHideControlSkiJumpImpulseHorizontal(false);
            gamePlayController.ShowHideControlSkiJumpImpulseVertical(false);
            bobsleighSled.GetSetStartPointIdInit = lugeBobsleigh.GetSetStartPointId;
            bobsleighSled.GetSetStartPointId = lugeBobsleigh.GetSetStartPointId;
            bobsleighSled.SetSpeedSkatingTrackDTOList = lugeBobsleigh.GetLugeTrackDTOList;
            bobsleighSled.ShowHideControlLuge();
            this.AddChild(bobsleighSled);            
        }        
    }
    private void InstantiateCharacterSkiJump()
    {
        character = characterPackedScene.Instantiate<Character>();
        character.SetPrefabName = prefabName;
        gamePlayController.SetDefaultPositionRotation(initPoint.Position, initPoint.Rotation);
        gamePlayController.SetCharacter(character);
        gamePlayController.SetCharacterSportSkiJump();
        gamePlayController.SetPauseScreen(pauseScreen);
        gamePlayController.SetFinishSessionScreen(finishSessionScreen);
        gamePlayController.SetControlSkiSpeedSkatingBiathlon(controlSkiSpeedSkatingBiathlon);
        gamePlayController.SetControlSkiCrossCountry(controlSkiCrossCountry);
        gamePlayController.SetControlBiathlon(controlBiathlon);
        gamePlayController.SetControlSkiJumpImpulseHorizontal(controlSkiJumpImpulseHorizontal, windDirectionArrowHorizontal);
        gamePlayController.SetControlSkiJumpImpulseVertical(controlSkiJumpImpulseVertical, windDirectionArrowVertical);
        gamePlayController.ShowHideControlLugeImpulse(false);
        gamePlayController.ShowHideControlSkiJumpImpulse(true);
        gamePlayController.ShowHideControlSkiJump(true);
        gamePlayController.ShowHideControlSkiJumpImpulseHorizontal(false);
        gamePlayController.ShowHideControlSkiJumpImpulseVertical(false);
        character.ShowHideControlSkiSpeedSkatingBiathlon(false);
        character.ShowHideControlBiathlon(false);
        character.SetWindDirectionSkiJumpUILabels(windDirectionSkiJump, windDirectionArrowSkiJump);
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
        if (prefabName == "LugeBobsleigh")
            InstantiateLevelLugeBobsleigh(prefabScene, levelId - 7);
        if (prefabName == "Skijumping")
            InstantiateLevelSkiJump(prefabScene, levelId);
    }
    private void InstantiateLevelSki(PackedScene prefabScene, int id)
    {        
        if (id != 11)
        {
            skiTrack = prefabScene.Instantiate<Ski>();
            initPoint = skiTrack.GetInitPoint(id);
            gateStart = skiTrack.GetStart(id);
            gateFinish = skiTrack.GetFinish(id);
            skiTrack.ShowGate(id);
            skiTrack.ShowHideCrossCountry(false, 4);
            AddChild(skiTrack);
        }
        else
        {            
            skiTrack = prefabScene.Instantiate<Ski>();
            initPoint = skiTrack.GetInitPointCrossCountry(0);
            gateStart = skiTrack.GetStart(4);
            gateFinish = skiTrack.GetFinish(4);
            skiTrack.HideGates();
            skiTrack.ShowGateStartFinish();            
            AddChild(skiTrack);
        }        
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
    private void InstantiateLevelLugeBobsleigh(PackedScene prefabScene, int id)
    {
        lugeBobsleigh = prefabScene.Instantiate<LugeBobsleigh>();
        lugeBobsleigh.EnableDisableGate(id);
        initPoint = lugeBobsleigh.GetSetInitPoint(id);
        lugeBobsleigh.InstantiateRail(id);
        lugeSled.MovePlayerToStartPosition();        
        AddChild(lugeBobsleigh);
    }
    private void InstantiateLevelSkiJump(PackedScene prefabScene, int id)
    {
        skiJump = prefabScene.Instantiate<SkiJump>();        
        initPoint = skiJump.GetSetInitPoint();
        AddChild(skiJump);
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
        if (prefabName == "LugeBobsleigh")
        {
            gamePlayController.UnPauseLuge();
            return;
        }        
        gamePlayController.UnPause();
    }
    private void ReturnMenuFromFinishScreen()
    {        
        if (prefabName == "LugeBobsleigh")
        {
            gamePlayController.ShowHideFinishSessionScreenLuge();
            return;
        }
        gamePlayController.ShowHideFinishSessionScreen();
    }
    private void ResetGameMenu()
    {        
        if (prefabName == "Skijumping")
            gamePlayController.ResetSkiJump();
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
        LugeStatic.Reset();
        SkiJumpStatic.Reset();
        CrossCountryStatic.Reset();
    }
    #endregion

}
