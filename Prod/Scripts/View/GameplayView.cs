using Godot;
using System;
using System.Collections.Generic;
using WinterSports.Scripts.Controller;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Model;
using WinterSports.Scripts.Prefabs;
using WinterSports.Scripts.Singleton;
using WinterSports.Scripts.Static;
using static WinterSports.Scripts.Controller.GamePlayController;

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
    [Export] Control controlSkiCrossCountryPosition = null;
    [Export] Label[] crossCountryCountryPositionLabel = null;
    [Export] Label[] crossCountryCountryCodeLabel = null;
    [Export] TextureRect[] crossCountryCountryFlagTextureRect = null;
    [Export] Control finishResultSessionControl = null;
    [Export] Button backMenuFinishButtonStandings = null;
    [Export] Button returnFinishButtonStandings = null;
    #endregion
    #region MeshInstance3D
    MeshInstance3D characterMeshInstance3D = null;
    #endregion
    #region Constant
    private const string iceHockeyFlagSelectName = "CanvasLayer/SelectTeamSessionControl/CountryButton";
    private const string iceHockeyFlagSelectContryName = "CanvasLayer/SelectTeamSessionControl/Contry";
    private const string iceHockeyFlagSelectCountryCodeName = "CanvasLayer/SelectTeamSessionControl/CountryCode";
    private const string iceHockeyKitTeam1 = "CanvasLayer/SelectTeamSessionControl/Kit1_";
    private const string iceHockeyKitTeam2 = "CanvasLayer/SelectTeamSessionControl/Kit2_";
    private const string iceHockeyKitTeam = "CanvasLayer/SelectTeamSessionControl/";
    private const string iceHockeyBackToMainMenuButtonFinish = "CanvasLayer/SelectTeamSessionControl/BackToMainMenuButtonFinish";
    private const string iceHockeyPlayToMainMenuButtonFinish = "CanvasLayer/SelectTeamSessionControl/PlayMenuButtonFinish";
    private const string HUDBGName = "CanvasLayer/HUD/HUDBG";
    private const string CountryFlagName = "CanvasLayer/HUD/CountryFlag";
    private const string CountryCodeName = "CanvasLayer/HUD/CountryCode";
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
    private IceHockey iceHockey = new IceHockey();
    private int levelId = 0;
    private string prefabName = String.Empty;
    private List<int> numbersStart = new List<int>();
    private TextureRect texture2DCountry1 = null;
    private TextureRect texture2DCountry2 = null;
    private Label countryCode1 = null;
    private Label countryCode2 = null;
    private List<Button> kitTeam1 = new List<Button>();
    private List<Button> kitTeam2 = new List<Button>();
    private CanvasItem jersey1_1 = null;
    private CanvasItem jersey1_2 = null;
    private CanvasItem short1_1 = null;
    private CanvasItem jersey2_1 = null;
    private CanvasItem jersey2_2 = null;
    private CanvasItem short2_1 = null;
    private Button quitButtonIceHockey = null;
    private Control selectTeamSessionControlIceHockey = null;
    private Button playButtonIceHockey = null;
    private NinePatchRect HUDBG = null;
    private TextureRect countryFlag = null;
    private Label countryCode = null;
    private RigidBody3D puck = null;
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

        gamePlayController.SetHockeyPower(GetNode<NinePatchRect>("CanvasLayer/HUD/ShootPower"));//<-
        gamePlayController.SetHockeyPowerControl(GetNode<Control>("CanvasLayer/HUD/ShootPower/Control"));//<-
        gamePlayController.SetParentNode(this);//<-

        InstantiateLevel();
        AssignButtons();
        SetMainGamePlayEvents();
        AssignIceHockeyUI();
        SetIceHockeyKitButton(kitTeam1, kitTeam2);
        SetIceHockeyKit(jersey1_1, jersey1_2, short1_1, jersey2_1, jersey2_2, short2_1);
        SetIceHockeyKitEvents();
        InitKit();
        gamePlayController.SetSelectionFlagsTexts(texture2DCountry1, texture2DCountry2, countryCode1, countryCode2);        
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
        gamePlayController.SetSelectTeamSessionControlIceHockey(selectTeamSessionControlIceHockey);                        
        gamePlayController.SetAllControlsToHideIceHockey(readySetGoControl,
                                                        finishSessionScreen,
                                                        controlSkiSpeedSkatingBiathlon,
                                                        controlBiathlon,
                                                        windDirectionArrow,
                                                        controlSkiSpeedSkatingScreen,
                                                        controlBiathlonScreen,
                                                        controlLugeImpulse,
                                                        controlSkiJumpImpulse,
                                                        controlSkiJump,
                                                        controlSkiJumpImpulseHorizontal,
                                                        windDirectionArrowHorizontal,
                                                        controlSkiJumpImpulseVertical,
                                                        windDirectionArrowVertical,
                                                        controlSkiCrossCountry,
                                                        controlSkiCrossCountryPosition,
                                                        finishResultSessionControl,
                                                        HUDBG, countryFlag, countryCode);
        SetDirectionArrowList();
        InstantiateCharacter();
        InitStaticVariables();
        SetIceButtonsEvent();
        gamePlayController.Init(prefabName);
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
                GenerateStartPosition();
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
        if (prefabName == "IceHockeyRink")
        {
            InstantiateCharacterIceHockey();
            InstantiatePuckIceHockey();
            gamePlayController.SetPuckToCharacter(puck);
            gamePlayController.SetCamera3DIceHockey(iceHockey.GetSetCamera3D());
            gamePlayController.SetIceHockeyGoal(iceHockey);            
        }            
    }
    private void InstantiateCharacterIceHockey()
    {
        gamePlayController.SetDefaultPositionRotation(initPoint.Position, initPoint.Rotation);
        for (int i = 0; i < 2; i++)
        {            
            for (int j = 0; j < 6; j++)
            {
                Character character = characterPackedScene.Instantiate<Character>();
                character.GetSetCharacterId = j;
                character.SetPrefabName = prefabName;
                character.ScaleObjectLocal(new Vector3(3.0f, 3.0f, 3.0f));//<-
                if (j > 0)
                {
                    character.ShowHideIceHockeyStick(true);
                    character.ShowHideIceHockeyGoalKeeper(false);
                }
                else
                {
                    character.ShowHideIceHockeyStick(false);
                    character.ShowHideIceHockeyGoalKeeper(true);
                }
                character.playerNumber = j;
                character.SetPlayerIceHockeyPosition(j, i);                
                gamePlayController.SetIceHockeyCharacter(character, i == 0);
                iceHockey.AddChild(character);
            }
        }
        gamePlayController.SetIceHockeyTeams();
        gamePlayController.HideIceHockeyCharacter();//<-TESTE        
    }
    private void InstantiatePuckIceHockey()
    {
        var puckPackedScene = ResourceLoader.Load<PackedScene>("res://Prefab/puckPh2.tscn");
        Node puckInstance = (Node)puckPackedScene.Instantiate();
        puck = puckInstance as RigidBody3D;
        iceHockey.AddChild(puck);        
        puck.Position = new Vector3(0.0f, 5.0f, 0.0f);
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
        gamePlayController.SetControlSkiCrossCountry(controlSkiCrossCountry, controlSkiCrossCountryPosition, crossCountryCountryPositionLabel,
            crossCountryCountryCodeLabel, crossCountryCountryFlagTextureRect, finishResultSessionControl);
        gamePlayController.SetControlBiathlon(controlBiathlon);
        gamePlayController.SetControlSkiJumpImpulseHorizontal(controlSkiJumpImpulseHorizontal, windDirectionArrowHorizontal);
        gamePlayController.SetControlSkiJumpImpulseVertical(controlSkiJumpImpulseVertical, windDirectionArrowVertical);
        gamePlayController.ShowHideControlLugeImpulse(false);
        gamePlayController.ShowHideControlSkiJumpImpulse(false);
        gamePlayController.ShowHideControlSkiJump(false);
        gamePlayController.ShowHideControlSkiJumpImpulseHorizontal(false);
        gamePlayController.ShowHideControlSkiJumpImpulseVertical(false);
        gamePlayController.ShowHideControlSkiCrossCountryPosition(false);
        character.ShowHideControlSkiSpeedSkatingBiathlon(true);
        character.ShowHideControlBiathlon(false);
        character.ShowHideIceHockeyStick(false);
        character.ShowHideIceHockeyGoalKeeper(false);
        this.AddChild(character);
    }
    private void InstantiateCharacterSkiCrossCountry()
    {
        character = characterPackedScene.Instantiate<Character>();
        character.SetPrefabName = prefabName;
        initPoint = skiTrack.GetInitPointCrossCountry(numbersStart[0]);
        gamePlayController.SetDefaultPositionRotation(initPoint.Position, initPoint.Rotation);
        gamePlayController.SetCharacter(character);
        gamePlayController.SetCharacterSportSki(gateStart, gateFinish);
        gamePlayController.SetPauseScreen(pauseScreen);
        gamePlayController.SetFinishSessionScreen(finishSessionScreen);
        gamePlayController.SetControlSkiSpeedSkatingBiathlon(controlSkiSpeedSkatingBiathlon);
        gamePlayController.SetControlSkiCrossCountry(controlSkiCrossCountry, controlSkiCrossCountryPosition, crossCountryCountryPositionLabel, 
            crossCountryCountryCodeLabel, crossCountryCountryFlagTextureRect, finishResultSessionControl);
        gamePlayController.SetControlBiathlon(controlBiathlon);
        gamePlayController.SetControlSkiJumpImpulseHorizontal(controlSkiJumpImpulseHorizontal, windDirectionArrowHorizontal);
        gamePlayController.SetControlSkiJumpImpulseVertical(controlSkiJumpImpulseVertical, windDirectionArrowVertical);
        gamePlayController.ShowHideControlLugeImpulse(false);
        gamePlayController.ShowHideControlSkiJumpImpulse(false);
        gamePlayController.ShowHideControlSkiJump(false);
        gamePlayController.ShowHideControlSkiJumpImpulseHorizontal(false);
        gamePlayController.ShowHideControlSkiJumpImpulseVertical(false);
        gamePlayController.ShowHideControlSkiCrossCountryPosition(true);
        character.ShowHideControlSkiSpeedSkatingBiathlon(true);
        character.ShowHideControlBiathlon(false);        
        character.SetCrossCountryModel(skiTrack.GetCrossCountryModel());
        character.ShowHideIceHockeyStick(false);
        character.ShowHideIceHockeyGoalKeeper(false);
        characterList.Add(character);
        this.AddChild(character);
    }
    private void GenerateStartPosition()
    {
        numbersStart = new List<int>();
        for (int i = 0; i < 16; i++)
        {
            numbersStart.Add(i);
        }        
        Random rand = new Random();
        for (int i = numbersStart.Count - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            int temp = numbersStart[i];
            numbersStart[i] = numbersStart[j];
            numbersStart[j] = temp;
        }        
    }
    private void InstantiateCharacterSkiCrossCountryAI()
    {
        List<List<CrossCountryModel>> crossCountryModel = new List<List<CrossCountryModel>>();
        for (int i = 0; i < 4; i++)
        {
            crossCountryModel.Add(skiTrack.GetCrossCountryAIModel("CrossCountryPosition" + i.ToString()));
        }
        int count = 1;
        foreach (var obj in CountrySingleton.countryObjDTO.countryList)
        {
            if (obj.Id != GameModeSingleton.country)
            {
                Character characterAI = characterPackedScene.Instantiate<Character>();
                characterAI.SetPrefabName = prefabName;
                MeshInstance3D initPointAI = skiTrack.GetInitPointCrossCountry(numbersStart[count]);
                gamePlayController.SetCharacterCrossCountryAI(characterAI, initPointAI.Position, initPointAI.Rotation, obj.Id - 1);
                gamePlayController.SetCharacterSportSkiCrossCountryAI(gateStart, gateFinish);
                characterAI.SetCrossCountryModel(skiTrack.GetCrossCountryModel());
                characterAI.SetCrossCountryAIModel(crossCountryModel);
                characterAI.ShowHideIceHockeyStick(false);
                characterAI.ShowHideIceHockeyGoalKeeper(false);
                characterAI.SetCurrentAILine(obj.Id - 1 < 4 ? obj.Id - 1 : ((obj.Id - 1) % 4));
                characterList.Add(characterAI);
                this.AddChild(characterAI);
                count++;
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
        gamePlayController.SetControlSkiCrossCountry(controlSkiCrossCountry, controlSkiCrossCountryPosition, crossCountryCountryPositionLabel, 
            crossCountryCountryCodeLabel, crossCountryCountryFlagTextureRect, finishResultSessionControl);
        gamePlayController.SetControlBiathlon(controlBiathlon);
        gamePlayController.SetControlSkiJumpImpulseHorizontal(controlSkiJumpImpulseHorizontal, windDirectionArrowHorizontal);
        gamePlayController.SetControlSkiJumpImpulseVertical(controlSkiJumpImpulseVertical, windDirectionArrowVertical);
        gamePlayController.SetRailSpeedSkating(speedSkatingTrack.GetStartPointId, speedSkatingTrack.GetSpeedSkatingTrackDTOList);
        gamePlayController.ShowHideControlLugeImpulse(false);        
        gamePlayController.ShowHideControlSkiJumpImpulse(false);
        gamePlayController.ShowHideControlSkiJump(false);
        gamePlayController.ShowHideControlSkiJumpImpulseHorizontal(false);
        gamePlayController.ShowHideControlSkiJumpImpulseVertical(false);
        gamePlayController.ShowHideControlSkiCrossCountryPosition(false);
        character.ShowHideControlSkiSpeedSkatingBiathlon(true);
        character.ShowHideControlBiathlon(false);
        character.ShowHideIceHockeyStick(false);
        character.ShowHideIceHockeyGoalKeeper(false);
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
        gamePlayController.SetControlSkiCrossCountry(controlSkiCrossCountry, controlSkiCrossCountryPosition, crossCountryCountryPositionLabel, 
            crossCountryCountryCodeLabel, crossCountryCountryFlagTextureRect, finishResultSessionControl);
        gamePlayController.SetControlBiathlon(controlBiathlon);
        gamePlayController.SetControlSkiJumpImpulseHorizontal(controlSkiJumpImpulseHorizontal, windDirectionArrowHorizontal);
        gamePlayController.SetControlSkiJumpImpulseVertical(controlSkiJumpImpulseVertical, windDirectionArrowVertical);
        gamePlayController.SetRailBiathlon(biathlonTrack.GetStartPointId, biathlonTrack.GetBiathlonTrackDTOList);
        gamePlayController.ShowHideControlLugeImpulse(false);        
        gamePlayController.ShowHideControlSkiJumpImpulse(false);
        gamePlayController.ShowHideControlSkiJump(false);
        gamePlayController.ShowHideControlSkiJumpImpulseHorizontal(false);
        gamePlayController.ShowHideControlSkiJumpImpulseVertical(false);
        gamePlayController.ShowHideControlSkiCrossCountryPosition(false);
        character.ShowHideControlSkiSpeedSkatingBiathlon(true);
        character.ShowHideControlBiathlon(false);
        character.SetBiathlonUILabels(shoots, errorLabelScore, windDirection, windDirectionArrow);
        character.ShowHideIceHockeyStick(false);
        character.ShowHideIceHockeyGoalKeeper(false);
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
            gamePlayController.SetControlSkiCrossCountry(controlSkiCrossCountry, controlSkiCrossCountryPosition, crossCountryCountryPositionLabel, 
                crossCountryCountryCodeLabel, crossCountryCountryFlagTextureRect, finishResultSessionControl);
            gamePlayController.SetControlSkiJumpImpulseHorizontal(controlSkiJumpImpulseHorizontal, windDirectionArrowHorizontal);
            gamePlayController.SetControlSkiJumpImpulseVertical(controlSkiJumpImpulseVertical, windDirectionArrowVertical);
            gamePlayController.ShowHideControlLugeImpulse(true);
            gamePlayController.ShowHideControlSkiJumpImpulse(false);
            gamePlayController.ShowHideControlSkiJump(false);
            gamePlayController.ShowHideControlSkiJumpImpulseHorizontal(false);
            gamePlayController.ShowHideControlSkiJumpImpulseVertical(false);
            gamePlayController.ShowHideControlSkiCrossCountryPosition(false);
            gamePlayController.ShowHideIceHockeyStick(false);
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
            gamePlayController.SetControlSkiCrossCountry(controlSkiCrossCountry, controlSkiCrossCountryPosition, crossCountryCountryPositionLabel, 
                crossCountryCountryCodeLabel, crossCountryCountryFlagTextureRect, finishResultSessionControl);
            gamePlayController.SetControlSkiJumpImpulseHorizontal(controlSkiJumpImpulseHorizontal, windDirectionArrowHorizontal);
            gamePlayController.SetControlSkiJumpImpulseVertical(controlSkiJumpImpulseVertical, windDirectionArrowVertical);
            gamePlayController.ShowHideControlLugeImpulse(true);
            gamePlayController.ShowHideControlSkiJumpImpulse(false);
            gamePlayController.ShowHideControlSkiJump(false);
            gamePlayController.ShowHideControlSkiJumpImpulseHorizontal(false);
            gamePlayController.ShowHideControlSkiJumpImpulseVertical(false);
            gamePlayController.ShowHideControlSkiCrossCountryPosition(false);
            gamePlayController.ShowHideIceHockeyStick(false);
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
        gamePlayController.SetControlSkiCrossCountry(controlSkiCrossCountry, controlSkiCrossCountryPosition, crossCountryCountryPositionLabel, 
            crossCountryCountryCodeLabel, crossCountryCountryFlagTextureRect, finishResultSessionControl);
        gamePlayController.SetControlBiathlon(controlBiathlon);
        gamePlayController.SetControlSkiJumpImpulseHorizontal(controlSkiJumpImpulseHorizontal, windDirectionArrowHorizontal);
        gamePlayController.SetControlSkiJumpImpulseVertical(controlSkiJumpImpulseVertical, windDirectionArrowVertical);
        gamePlayController.ShowHideControlLugeImpulse(false);
        gamePlayController.ShowHideControlSkiJumpImpulse(true);
        gamePlayController.ShowHideControlSkiJump(true);
        gamePlayController.ShowHideControlSkiJumpImpulseHorizontal(false);
        gamePlayController.ShowHideControlSkiJumpImpulseVertical(false);
        gamePlayController.ShowHideControlSkiCrossCountryPosition(false);
        character.ShowHideControlSkiSpeedSkatingBiathlon(false);
        character.ShowHideControlBiathlon(false);
        character.SetWindDirectionSkiJumpUILabels(windDirectionSkiJump, windDirectionArrowSkiJump);
        character.ShowHideIceHockeyStick(false);
        character.ShowHideIceHockeyGoalKeeper(false);
        this.AddChild(character);
    }
    private void AssignButtons()
    {
        gamePlayController.GetSetGoToMainMenu = backMenuButton;
        gamePlayController.GetSetReturnMenu = returnMenuButton;
        gamePlayController.GetSetResetMenu = resetMenuButton;
        gamePlayController.GetSetBackMenuFinishButton = backMenuFinishButton;
        gamePlayController.GetSetReturnFinishButton = returnFinishButton;
        gamePlayController.GetSetBackMenuFinishButtonStandings = backMenuFinishButtonStandings;
        gamePlayController.GetSetReturnFinishButtonStandings = returnFinishButtonStandings;

        if (prefabName == "IceHockeyRink")
        {
            for (int i = 0; i < 16; i++) 
            {
                gamePlayController.GetSetIceHockeyFlagSelectButton.Add(GetNode<Button>(iceHockeyFlagSelectName + i.ToString()));                
            }            
        }
    }
    private void AssignIceHockeyUI()
    {
        texture2DCountry1 = GetNode<TextureRect>(iceHockeyFlagSelectContryName + (1).ToString());
        texture2DCountry2 = GetNode<TextureRect>(iceHockeyFlagSelectContryName + (2).ToString());
        countryCode1 = GetNode<Label>(iceHockeyFlagSelectCountryCodeName + (1).ToString());
        countryCode2 = GetNode<Label>(iceHockeyFlagSelectCountryCodeName + (2).ToString());

        kitTeam1.Add(GetNode<Button>(iceHockeyKitTeam1 + (1).ToString()));
        kitTeam1.Add(GetNode<Button>(iceHockeyKitTeam1 + (2).ToString()));
        kitTeam2.Add(GetNode<Button>(iceHockeyKitTeam2 + (1).ToString()));
        kitTeam2.Add(GetNode<Button>(iceHockeyKitTeam2 + (2).ToString()));

        jersey1_1 = GetNode<CanvasItem>(iceHockeyKitTeam + "Jersey1_1");
        jersey1_2 = GetNode<CanvasItem>(iceHockeyKitTeam + "Jersey1_2");
        short1_1 = GetNode<CanvasItem>(iceHockeyKitTeam + "Short1_1");

        jersey2_1 = GetNode<CanvasItem>(iceHockeyKitTeam + "Jersey2_1");
        jersey2_2 = GetNode<CanvasItem>(iceHockeyKitTeam + "Jersey2_2");
        short2_1 = GetNode<CanvasItem>(iceHockeyKitTeam + "Short2_1");

        quitButtonIceHockey = GetNode<Button>(iceHockeyBackToMainMenuButtonFinish);
        playButtonIceHockey = GetNode<Button>(iceHockeyPlayToMainMenuButtonFinish);

        selectTeamSessionControlIceHockey = GetNode<Control>(iceHockeyKitTeam);

        HUDBG = GetNode<NinePatchRect>(HUDBGName);
        countryFlag = GetNode<TextureRect>(CountryFlagName);
        countryCode = GetNode<Label>(CountryCodeName);
    }
    private void SetMainGamePlayEvents()
    {
        gamePlayController.GetSetGoToMainMenu.Pressed += () => { QuitToMainMenu(); };
        gamePlayController.GetSetReturnMenu.Pressed += () => { ReturnMenu(); };
        gamePlayController.GetSetResetMenu.Pressed += () => { ResetGameMenu(); };
        gamePlayController.GetSetBackMenuFinishButton.Pressed += () => { QuitToMainMenu(); };
        gamePlayController.GetSetReturnFinishButton.Pressed += () => { ReturnMenuFromFinishScreen(); };
        gamePlayController.GetSetBackMenuFinishButtonStandings.Pressed += () => { QuitToMainMenu(); };
        gamePlayController.GetSetReturnFinishButtonStandings.Pressed += () => { ResetGameMenu(); };

        if (prefabName == "IceHockeyRink")
        {
            int count = 0;
            foreach (var obj in gamePlayController.GetSetIceHockeyFlagSelectButton)
            {
                int id = count;
                obj.Pressed += () => { SelectIceHockeyTeam2(id); };
                count++;
            }            
        }
    }
    private void SetIceHockeyKitEvents()
    {
        for (int i = 0; i < 2; i++)
        {
            int count = i;
            gamePlayController.GetSetKitTeam1[i].Pressed += () => { SetIceHockeyKitEvent(count, 0); };
            gamePlayController.GetSetKitTeam2[i].Pressed += () => { SetIceHockeyKitEvent(count, 1); };
        }
    }
    private void InitKit()
    {
        SetIceHockeyKitEvent(0, 0);
        SetIceHockeyKitEvent(0, 1);
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
        if (prefabName == "IceHockeyRink")
            InstantiateLevelIceHockey(prefabScene);            
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
    private void InstantiateLevelIceHockey(PackedScene prefabScene)
    {
        iceHockey = prefabScene.Instantiate<IceHockey>();
        initPoint = iceHockey.GetSetInitPoint();
        AddChild(iceHockey);
    }
    public void SetIceHockeyKitButton(List<Button> kitTeam1, List<Button> kitTeam2)
    {
        gamePlayController.SetIceHockeyKitButton(kitTeam1, kitTeam2);
    }
    public void SetIceHockeyKit(CanvasItem jersey1_1, CanvasItem jersey1_2, CanvasItem short1_1, CanvasItem jersey2_1, CanvasItem jersey2_2, CanvasItem short2_1)
    {
        gamePlayController.SetIceHockeyKit(jersey1_1, jersey1_2, short1_1, jersey2_1, jersey2_2, short2_1);
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
        {
            if (11 == levelId)
            {                
                gamePlayController.ResetCrossCountry();
            }
            else
            {
                gamePlayController.Reset();
            }            
        }        
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
        GameModeSingleton.countryOppositeHockeyTeam = 0;
    }
    private void SelectIceHockeyTeam2(int id)
    {        
        gamePlayController.SelectIceHockeyTeam2(id);
        gamePlayController.SetIceHockeyKitEvent(0, 1);
    }
    private void SetIceHockeyKitEvent(int kitId, int teamId)
    {
        gamePlayController.SetIceHockeyKitEvent(kitId, teamId);
    }

    private void SetIceButtonsEvent()
    {        
        quitButtonIceHockey.Pressed += () => { QuitToMainMenu(); };
        playButtonIceHockey.Pressed += () => { PlayIceHockey(); };
    }
    private void PlayIceHockey()
    {
        gamePlayController.PlayIceHockey();
    }
    #endregion

}
