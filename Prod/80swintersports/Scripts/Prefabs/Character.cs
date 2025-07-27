using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Transactions;
using System.Xml.Linq;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Events;
using WinterSports.Scripts.Interfaces;
using WinterSports.Scripts.Model;
using WinterSports.Scripts.Static;
using static CrossCountryOvertake;
using static WinterSports.Scripts.Controller.GamePlayController;
using static WinterSports.Scripts.Events.PlayerInputIceHockey;
using static WinterSports.Scripts.Model.TimerModel;

public partial class Character : CharacterBody3D
{
    #region Exports
    [Export] AnimationPlayer animationPlayer = null;
    [Export] MeshInstance3D[] bodyMeshInstance3D = null;
    [Export] MeshInstance3D[] armsMeshInstance3D = null;
    [Export] MeshInstance3D[] handsAndHeadMeshInstance3D = null;
    [Export] MeshInstance3D[] legsMeshInstance3D = null;
    [Export] MeshInstance3D[] bootsMeshInstance3D = null;
    [Export] MeshInstance3D[] hairMeshInstance3D = null;
    [Export] MeshInstance3D[] cameraRef = null;
    [Export] Node3D camera3D = null;
    [Export] Node3D[] skiPole = null;
    [Export] Node3D[] ski = null;    
    [Export] Node3D speedBox = null;
    [Export] Node3D target = null;
    [Export] Node3D armature = null;
    [Export] SkiCollision skiCollision = null;
    [Export] CrossCountryOvertake crossCountryOvertakeFM = null;
    [Export] CrossCountryOvertake crossCountryOvertakeFR = null;
    [Export] CrossCountryOvertake crossCountryOvertakeFL = null;
    [Export] CrossCountryOvertake crossCountryOvertakeMM = null;
    [Export] CrossCountryOvertake crossCountryOvertakeMR = null;
    [Export] CrossCountryOvertake crossCountryOvertakeML = null;
    [Export] Node3D stick = null;
    [Export] Node3D[] goalKeeperItems = null;
    [Export] Node3D iceHockeySelect = null;
    [Export] Node3D puckRef = null;
    #endregion
    #region Variables    
    private IPlayerInput playerInput = null;
    private SceneTree sceneTree = null;
    private Control pauseScreen = null;
    private Control finishSessionScreen = null;
    private Control controlSkiSpeedSkatingBiathlon = null;
    private Control controlSkiCrossCountry = null;
    private Control controlBiathlon = null;
    private Control controlSkiJumpImpulseHorizontal = null;
    private Control windDirectionArrowHorizontal = null;
    private Control controlSkiJumpImpulseVertical = null;
    private Control windDirectionArrowVertical = null;
    private string prefabName = String.Empty;
    private int startPointId = 0;
    private List<DirectionArrow> directionArrowList = new List<DirectionArrow>();
    private List<List<DirectionArrow>> directionArrowBiathlonList = new List<List<DirectionArrow>>();
    private int characterId = 0;
    private int characterIdCountry = 0;
    private bool isAI = false;    
    private Vector3 initPosition = Vector3.Zero;
    private Vector3 initRotation = Vector3.Zero;    
    #endregion
    #region Variables Speed Skating
    private List<SpeedSkatingTrackDTO> speedSkatingTrackDTOList = new List<SpeedSkatingTrackDTO>();
    #endregion
    #region Variables Biathlon
    private List<List<SpeedSkatingTrackDTO>> biathlonTrackDTOList = new List<List<SpeedSkatingTrackDTO>>();
    private float maxXTarget = 0.2f;
    private float minXTarget = -0.2f;
    private float minYTarget = -2.7f;
    private float maxYTarget = -3.4f;
    [Export] private RayCast3D rayCast3D = null;
    private Label shoots = null;
    private Label errorLabelScore = null;
    private Label windDirection = null;
    private Control windDirectionArrow = null;
    private int shootErrors = 0;
    #endregion    
    #region Constant
    private float[] scaleFactorArray = { 1.0f, 1.5f, 1.5f, 1.5f };
    private float increment = 0.01f;
    private float shootInc = 30000.0f;
    private const int minRotateZ = -45;
    private const int maxRotateZ = 45;
    private const int minRotateZUI = 0;
    private const int medRotateZUI = 85;
    private const int maxRotateZUI = 170;
    private const int minRotateX = -45;
    private const int maxRotateX = 45;
    #endregion
    #region Sport Ski
    [Export] Area3D startGate = null;
    [Export] Area3D finishGate = null;
    public enum StatesSki
    {
        Ready,
        Set,
        Go,
        Init,
        Running,
        JumpInCarBobsleigh,
        RunningBobsleigh,
        Finish,
        Disqualified,
        Shooting,
        SkiJumpingDown,
        SkiJumpingFly,
        SkiJumpingLand,
        SkiJumpingFinish,
        End
    };
    public StatesSki statesSki = StatesSki.Ready;
    #endregion
    #region Sport Biathlon    
    [Export] public Node3D rifle = null;
    [Export] Node3D[] skiPoleBiathlon = null;
    #endregion    
    #region Sport Ski jump
    private float impulse = 180.0f;
    private float impulsePercent = 0.0f;
    private const float maxImpulse = 100.0f;
    private float impulseRail = 0.0f;
    private Label windDirectionSkiJump = null;
    private Control windDirectionArrowSkiJump = null;
    private float angle = 0.0f;
    private float power = 0.0f;
    #endregion
    #region Ski Cross Country
    private List<CrossCountryModel> crossCountryModelList = null;
    private List<List<CrossCountryModel>> crossCountryModelAIList = null;
    private int currentAILine = 0;
    private double score = 0.0f;
    public double GetSetScore
    {
        get 
        {
            return score;
        }
        set 
        {
            score = value; 
        }
    }
    private bool isRunFinished = false;
    public bool GetIsRunFinished
    { 
        get 
        { 
            return isRunFinished; 
        }
    }
    private int currentPoint = 0;
    public int GetSetCurrentPoint
    {
        get
        {
            return currentPoint;
        }
        set 
        {
            currentPoint = value;
        }
    }
    private bool isScoreSet = false;
    #endregion
    #region Ice Hockey
    public enum IceHockeyPosition
    {
        GK,
        DF,
        MF,
        FW
    };
    public IceHockeyPosition iceHockeyPosition = IceHockeyPosition.GK;
    public IceHockeyPosition[] iceHockeyPositionArray = {
        IceHockeyPosition.GK,
        IceHockeyPosition.DF,
        IceHockeyPosition.DF,
        IceHockeyPosition.MF,
        IceHockeyPosition.FW,
        IceHockeyPosition.FW
    };
    public enum IceHockeyPositionSide
    {
        L,
        M,
        R
    };
    public IceHockeyPositionSide iceHockeyPositionSide = IceHockeyPositionSide.M;
    public IceHockeyPositionSide[] iceHockeyPositionSideArray = {
        IceHockeyPositionSide.M,
        IceHockeyPositionSide.R,
        IceHockeyPositionSide.L,
        IceHockeyPositionSide.M,
        IceHockeyPositionSide.L,
        IceHockeyPositionSide.R,
    };
    public Dictionary<string, bool> iceHockeyMoveLimit = new Dictionary<string, bool>()
    {
        {"up",false},
        {"down",false},
        {"left",false},
        {"right",false}                                        
    };
    public int playerNumber = 0;
    public bool isPlayerTeam = false;
    public bool isSelected = false;
    public bool isPuckControl = false;
    public RigidBody3D puck = null;
    private Transform3D originalTransform;
    private IceHockeyGoal Goal1 = null;
    private IceHockeyGoal Goal2 = null;
    public List<Character> iceHockeyTeam1 = null;
    public List<Character> iceHockeyTeam2 = null;
    private Node3D ShootRef = null;
    public NinePatchRect hockeyPower = null;
    public Node parentNode = null;
    public Control hockeyPowerControl = null;
    public bool isIceHockeySeletion = false;
    public IceHockeyPlayerCollision iceHockeyPlayerCollision = null;
    #endregion
    #region Behavior
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        DefineShootRef();
        Init();
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {        
    }
    public override void _PhysicsProcess(double delta)
    {
        try
        {
            if (prefabName == "skiTrack")
            {
                if (statesSki > StatesSki.Go)
                {
                    if (GameModeSingleton.sport == 12)
                    {
                        playerInput.PlayerInput(animationPlayer, 0, GetSkiCrossCountryCurrentId(),
                            crossCountryOvertakeFM, crossCountryOvertakeMR, crossCountryOvertakeML);
                    }
                    else
                        playerInput.PlayerInput(animationPlayer);
                }
            }
            if (prefabName == "SpeedSkating")
            {
                if (statesSki > StatesSki.Go)
                    playerInput.PlayerInput(animationPlayer, delta);
            }
            if (prefabName == "Biathlon")
            {
                if (statesSki > StatesSki.Go)
                    playerInput.PlayerInput(animationPlayer, delta);
            }
            if (prefabName == "Skijumping")
            {
                playerInput.PlayerInput(animationPlayer, delta);
            }
            if (prefabName == "IceHockeyRink")
            {
                if (IceHockeyStatic.statesIceHockey == IceHockeyStatic.StatesIceHockey.Init && IceHockeyStatic.statesIceHockeyStart == IceHockeyStatic.StatesIceHockeyStart.InGame)
                {
                    if (isPlayerTeam)
                    {
                        if (isSelected)
                        {
                            playerInput.PlayerInput(animationPlayer, delta);
                        }
                        else
                        {
                            playerInput.PlayerInputAI(animationPlayer, delta);
                        }
                    }
                    else
                    {
                        playerInput.PlayerInputAIOpponent(animationPlayer, delta);
                    }
                }
            }
        }
        catch (Exception ex) { }        
    }    
    #endregion
    #region Methods
    public void Init()
    {        
        PlayerInputSetUp();        
        playerInput.SetCharacterBody3D(this);
        playerInput.SetPauseScreen(pauseScreen);
        playerInput.SetFinishSessionScreen(finishSessionScreen);        
        playerInput.PlayAnimation(animationPlayer, 1);
        playerInput.SetCharacter(this);
        playerInput.SetSkiCollision(skiCollision);
        playerInput.SetIsAI(isAI);
        playerInput.SetCharacterIdCountry(characterIdCountry);
        playerInput.Init(crossCountryModelAIList, currentAILine);            
        //Sport Ski 
        if (prefabName == "skiTrack")
            InitSki();
        //Sport Speed Skating
        if (prefabName == "SpeedSkating")
            InitSpeedSkating();
        //Sport Biathlon
        if (prefabName == "Biathlon")
            InitBiathlon();
        //Ski jumping
        if (prefabName == "Skijumping")
            InitSkiJump();
        //IceHockeyRink
        if (prefabName == "IceHockeyRink")        
            InitIceHockey();                    

        ShowHideIceHockeySeletion(false);
    }
    private void PlayerInputSetUp()
    {        
        //Sport Ski 
        if (prefabName == "skiTrack")
            playerInput = new PlayerInputSki();
        //Sport Speed Skating
        if (prefabName == "SpeedSkating")
            playerInput = new PlayerInputSpeedSkating();
        //Sport Biathlon
        if (prefabName == "Biathlon")
            playerInput = new PlayerInputBiathlon();
        //Sport Luge        
        if (prefabName == "LugeBobsleigh")        
            playerInput = new PlayerInputLuge();
        //Ski jumping        
        if (prefabName == "Skijumping")
            playerInput = new PlayerInputSkiJump();
        //Ice Hockey
        if (prefabName == "IceHockeyRink")
            playerInput = new PlayerInputIceHockey();
    }
    public void Reset()
    {
        playerInput.PlayAnimation(animationPlayer, 1);
        playerInput.Init();
        playerInput.Reset();
        impulse = 180.0f;
        impulsePercent = 0.0f;
        impulseRail = 0.0f;
    }    
    public void GenerateBodyColor(Godot.Color aColor)
    {        
        GenerateColor(aColor, bodyMeshInstance3D);
    }
    public void GenerateArmsColor(Godot.Color aColor)
    {
        GenerateColor(aColor, armsMeshInstance3D);
    }
    public void GenerateHandsAndHeadColor(Godot.Color aColor)
    {
        GenerateColor(aColor, handsAndHeadMeshInstance3D);
    }
    public void GenerateLegsColor(Godot.Color aColor)
    {
        GenerateColor(aColor, legsMeshInstance3D);
    }
    public void GenerateBotsColor(Godot.Color aColor)
    {
        GenerateColor(aColor, bootsMeshInstance3D);
    }
    public void GenerateHairColor(Godot.Color aColor)
    {
        GenerateColor(aColor, hairMeshInstance3D);
    }
    private void GenerateColor(Godot.Color aColor, MeshInstance3D[] meshInstance3D)
    {
        foreach (var mesh in meshInstance3D)
        {
            var materialNew = new StandardMaterial3D();
            materialNew.AlbedoColor = aColor;
            materialNew.ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded;
            materialNew.DiffuseMode = BaseMaterial3D.DiffuseModeEnum.Lambert;
            materialNew.SpecularMode = BaseMaterial3D.SpecularModeEnum.Disabled;
            mesh.SetSurfaceOverrideMaterial(0, materialNew);
        }
    }
    public void UnPause()
    {        
        playerInput.UnPause();
    }
    public void Pause()
    {        
        playerInput.Pause();
    }
    public void ShowHideFinishSessionScreen()
    {
        playerInput.ShowHideFinishSessionScreen();
    }
    public void MoveCameraPositionRotation(int id)
    {
        camera3D.Position = cameraRef[id].Position;
        camera3D.Rotation = cameraRef[id].Rotation;        
    }
    private void ReScaleCharacter(int id)
    {        
        this.Scale = new Vector3(scaleFactorArray[id], scaleFactorArray[id], scaleFactorArray[id]);
    }
    public void MoveAndReScaleCharacter(int id)
    {
        MoveCameraPositionRotation(id);
        ReScaleCharacter(id);
    }
    public void MoveAndReScaleCharacterAI(int id)
    {
        HideCamera();
        ReScaleCharacter(id);
    }
    public void HideCamera()
    {
        camera3D.Hide();        
    }
    public void DisableCamera()
    {
        camera3D.Hide();
    }
    public float CalculateImpulsePercent()
    {

        if (!playerInput.GetPause())
        {
            impulse += 5.0f;
            var normalize = 1.0f + MathF.Cos((float)((Math.PI / 180.0) * (double)impulse));
            impulsePercent = (normalize * 100.0f) / 2.0f;
            return impulsePercent;
        }
        return 0.0f;
    }
    public void OnlyPause()
    {
        playerInput.OnlyPause();
    }
    public bool GetPause()
    {
        return playerInput.GetPause();
    }
    #endregion
    #region Get Set
    public Control SetPauseScreen
    {
        set 
        { 
            this.pauseScreen = value;
        }
    }
    public Control SetFinishSessionScreen
    {
        set
        {
            this.finishSessionScreen = value;
        }
    }
    public Control SetControlSkiSpeedSkatingBiathlon
    {
        set
        {
            this.controlSkiSpeedSkatingBiathlon = value;
        }
    }
    public Control SetControlSkiCrossCountry
    {
        set
        {
            this.controlSkiCrossCountry = value;
        }
    }
    public Control SetControlBiathlon
    {
        set
        {
            this.controlBiathlon = value;
        }
    }
    public Control SetControlSkiJumpImpulseHorizontal
    {
        set
        {
            this.controlSkiJumpImpulseHorizontal = value;
        }
    }
    public Control SetWindDirectionArrowHorizontal
    {
        set
        {
            this.windDirectionArrowHorizontal = value;
        }
    }
    public Control SetControlSkiJumpImpulseVertical
    {
        set
        {
            this.controlSkiJumpImpulseVertical = value;
        }
    }
    public Control SetWindDirectionArrowVertical
    {
        set
        {
            this.windDirectionArrowVertical = value;
        }
    }
    public string SetPrefabName
    {
        set 
        { 
            this.prefabName = value; 
        }
    }
    public float GetMaxImpulse
    {
        get 
        {
            return maxImpulse;
        }
    }
    public float GetSetImpulseRail
    {
        get
        {
            return impulseRail;
        }
        set
        {
            impulseRail = value;
        }
    }
    public float GetMaxRotateX
    {
        get
        {
            return maxRotateX;
        }
    }
    public int GetSetCharacterId
    {
        get
        {
            return characterId;
        }
        set
        {
            characterId = value;
        }
    }
    public int GetSetCharacterIdCountry
    {
        get
        {
            return characterIdCountry;
        }
        set
        {
            characterIdCountry = value;
        }
    }
    public Vector3 GetSetInitPosition
    {
        get
        {
            return initPosition;
        }
        set
        {
            initPosition = value;
        }
    }
    public Vector3 GetSetInitRotation
    {
        get
        {
            return initRotation;
        }
        set
        {
            initRotation = value;
        }
    }
    #endregion
    #region Sport Ski
    public Area3D SetStartGate
    {
        set 
        {
            this.startGate = value;
        }        
    }
    public Area3D SetFinishGate
    {
        set
        {
            this.finishGate = value;
        }
    }
    private void InitSki()
    {        
        startGate.Connect("body_entered", new Callable(this, nameof(OnAreaEnteredStartGate)));
        finishGate.Connect("body_entered", new Callable(this, nameof(OnAreaEnteredFinishGate)));
        speedBox.Hide();
        ShowHideTarget(false);
    }
    private void InitSpeedSkating()
    {        
        playerInput.SetRailSpeedSkating(startPointId, speedSkatingTrackDTOList, directionArrowList);
        ShowHideTarget(false);
    }
    private void OnAreaEnteredStartGate(Node body)
    {        
        if (body.Name == this.Name)
        {     
            statesSki = StatesSki.Running;            
        }            
    }
    private void OnAreaEnteredFinishGate(Node body)
    {
        if (body.Name == this.Name)
        {            
            statesSki = StatesSki.Finish;
        }
    }
    public float GetSpeed
    {
        get 
        {
            return playerInput.GetSpeed();
        }
    }
    public float GetEnergy
    {
        get
        {
            return playerInput.GetEnergy();
        }
    }
    public float GetMaxSpeed
    {
        get
        {
            return playerInput.GetMaxSpeed();
        }
    }
    public void ShowHideSkiItems()
    {
        foreach (var item in skiPole)
        {
            item.Show();
        }
        foreach (var item in ski)
        {
            item.Show();
        }
        foreach (var item in skiPoleBiathlon)
        {
            item.Hide();
        }
        rifle.Hide();
    }
    #endregion
    #region Speed Skating
    public void ShowHideSpeedSkatingItems()
    {
        foreach (var item in skiPole)
        {
            item.Hide();
        }
        foreach (var item in ski)
        {
            item.Hide();
        }
        foreach (var item in skiPoleBiathlon)
        {
            item.Hide();
        }
        rifle.Hide();
    }
    public void SetRailSpeedSkating(int startPointId, List<SpeedSkatingTrackDTO> speedSkatingTrackDTOList)
    {        
        this.startPointId = startPointId;
        this.speedSkatingTrackDTOList = speedSkatingTrackDTOList;
        InitSkiJumpRail();
    }
    public void SetDirectionArrowList(List<DirectionArrow> aDirectionArrowList)
    {
        directionArrowList = aDirectionArrowList;
    }
    #endregion
    #region Biathlon
    public void ShowHideSkiPoleBiathlonItems()
    {        
        foreach (var item in skiPole)
        {
            item.Hide();
        }
        foreach (var item in ski)
        {
            item.Show();
        }
        foreach (var item in skiPoleBiathlon)
        {
            item.Show();
        }
        rifle.Show();
    }
    public void SetDirectionArrowList(List<List<DirectionArrow>> aDirectionArrowList)
    {
        directionArrowBiathlonList = aDirectionArrowList;
    }
    public void SetRailBiathlon(int startPointId, List<List<SpeedSkatingTrackDTO>> biathlonTrackDTOList)
    {
        this.startPointId = startPointId;
        this.biathlonTrackDTOList = biathlonTrackDTOList;
    }
    private void InitBiathlon()
    {
        playerInput.SetRailBiathlon(startPointId, biathlonTrackDTOList, directionArrowBiathlonList);
        ShowHideTarget(false);
    }
    public void SpeedBoxShowHide(bool show)
    {
        if(show)
            speedBox.Show();
        else        
            speedBox.Hide();
    }
    public void ShowHideTarget(bool show)
    {
        if (show)
            target.Show();
        else
            target.Hide();        
    }
    public void MoveCameraY(bool isLeft)
    {        
        var inc = camera3D.Rotation.Y + ((isLeft ? -1.0f : 1.0f) * increment);
        if (inc < minYTarget && inc > maxYTarget)
        {
            camera3D.Rotation = new Vector3(camera3D.Rotation.X, inc, camera3D.Rotation.Z);
        }        
    }
    public void MoveCameraX(bool isUp)
    {                
        var inc = camera3D.Rotation.X + ((isUp ? -1.0f : 1.0f) * increment);
        if (inc > minXTarget && inc < maxXTarget)
        {
            camera3D.Rotation = new Vector3(inc, camera3D.Rotation.Y, camera3D.Rotation.Z);            
        }        
    }
    public void MoveCameraXWind(float moveInc, float windAngle)
    {
        var moveIncNormal = (windAngle > 180.0f && windAngle <= 360.0f ? -1.0f : 1.0f) * MathF.Abs(moveInc);
        var inc = camera3D.Rotation.X + ((moveIncNormal / shootInc));        
        if (inc > minXTarget && inc < maxXTarget)
        {
            camera3D.Rotation = new Vector3(inc, camera3D.Rotation.Y, camera3D.Rotation.Z);
        }
    }
    public void MoveCameraYWind(float moveInc, float windAngle)
    {        
        var moveIncNormal = (windAngle > 90.0f && windAngle <= 270.0f ? 1.0f : -1.0f) * MathF.Abs(moveInc);
        var inc = camera3D.Rotation.Y + ((moveIncNormal / shootInc));
        if (inc < minYTarget && inc > maxYTarget)
        {
            camera3D.Rotation = new Vector3(camera3D.Rotation.X, inc, camera3D.Rotation.Z);
        }
    }
    public RayCast3D GetTargetRayCast()
    {
        return rayCast3D;
    }
    public void ShowHideControlSkiSpeedSkatingBiathlon(bool isShow)
    {
        if (GameModeSingleton.sport == 12)
        {
            ShowHideControlSkiCrossCountry(true);
            this.controlSkiSpeedSkatingBiathlon.Hide();
        }        
        else
        {            
            ShowHideControlSkiCrossCountry(false);
            if (this.controlSkiSpeedSkatingBiathlon is not null)
            {
                if (isShow)
                    this.controlSkiSpeedSkatingBiathlon.Show();
                else
                    this.controlSkiSpeedSkatingBiathlon.Hide();
            }            
        }        
    }
    public void ShowHideControlSkiCrossCountry(bool isShow)
    {
        if (this.controlSkiCrossCountry is not null)
        {
            if (isShow)
                this.controlSkiCrossCountry.Show();
            else
                this.controlSkiCrossCountry.Hide();
        }        
    }
    public void ShowHideControlBiathlon(bool isShow)
    {
        if(this.controlBiathlon is not null)
        {
            if (isShow)
                this.controlBiathlon.Show();
            else
                this.controlBiathlon.Hide();
        }        
    }
    public void SetBiathlonUILabels(Label shoots, Label errorLabelScore, Label windDirection, Control windDirectionArrow)
    {
        this.shoots = shoots;
        this.errorLabelScore = errorLabelScore;
        this.windDirection = windDirection;
        this.windDirectionArrow = windDirectionArrow;
    }
    public void SetWindDirectionSkiJumpUILabels(Label windDirectionSkiJump, Control windDirectionArrowSkiJump)
    {
        this.windDirectionSkiJump = windDirectionSkiJump;
        this.windDirectionArrowSkiJump = windDirectionArrowSkiJump;
    }
    public void SetShoots(int shoots)
    {
        this.shoots.Text = shoots.ToString();
    }
    public void SetErrors(int errors)
    {
        this.shootErrors = errors;
        this.errorLabelScore.Text = errors.ToString();
    }
    public int GetErrors()
    { 
        return this.shootErrors;
    }
    public void SetWind(float angle, float power)
    {        
        windDirectionArrow.RotationDegrees = angle;
        windDirection.Text = string.Format("{0:0.0}", power) + "m/s";
    }
    #endregion
    #region Luge
    public void SetPlayerInputAnim(int id)
    {        
        playerInput.PlayAnimation(animationPlayer, id);
    }
    public void HideItemsForLuge()
    {        
        foreach (var item in skiPole)
        {
            item.Hide();
        }
        foreach (var item in ski)
        {
            item.Hide();
        }
        foreach (var item in skiPoleBiathlon)
        {
            item.Hide();
        }
        rifle.Hide();
    }
    #endregion
    #region Ski jump
    private void InitSkiJump()
    {        
        ShowHideTarget(false);
        speedBox.Hide();        
    }
    public void ShowHideSkiJumpItems()
    {
        foreach (var item in skiPole)
        {
            item.Hide();
        }
        foreach (var item in ski)
        {
            item.Show();
        }
        foreach (var item in skiPoleBiathlon)
        {
            item.Hide();
        }
        rifle.Hide();
    }
    private void InitSkiJumpRail()
    {
        playerInput.SetRailSpeedSkating(startPointId, speedSkatingTrackDTOList, directionArrowList);        
    }
    public void SetWindSkiJump(float angle, float power)
    {
        this.angle = angle;
        this.power = power;
        playerInput.SetWindSkiJump(angle, power);
        windDirectionArrowSkiJump.RotationDegrees = angle;
        windDirectionSkiJump.Text = string.Format("{0:0.0}", power) + "m/s";
    }
    public void SetSkiJumpPoint(int[] flyPoints)
    { 
        playerInput.SetSkiJumpPoint(flyPoints);
    }
    public void RotateZ(bool isLeft, float inc)
    {
        if (isLeft)
        {
            if (armature.RotationDegrees.Z < maxRotateZ)
                armature.RotationDegrees = new Vector3(armature.RotationDegrees.X, armature.RotationDegrees.Y, armature.RotationDegrees.Z + inc);            
        }
        else
        {
            if(armature.RotationDegrees.Z > minRotateZ)
                armature.RotationDegrees = new Vector3(armature.RotationDegrees.X, armature.RotationDegrees.Y, armature.RotationDegrees.Z - inc);            
        }                
    }    
    public void RotateZWind(float inc)
    {
        if (armature.RotationDegrees.Z < maxRotateZ && armature.RotationDegrees.Z > minRotateZ)
            armature.RotationDegrees = new Vector3(armature.RotationDegrees.X, armature.RotationDegrees.Y, armature.RotationDegrees.Z + inc);
        SkiJumpStatic.fliyngScore.Add(System.Math.Abs(armature.RotationDegrees.Z));        
        MoveZArrowWind();        
    }
    public void RotateX(bool isUp, float inc)
    {
        if (isUp)
        {
            if (armature.RotationDegrees.X < maxRotateX)
                armature.RotationDegrees = new Vector3(armature.RotationDegrees.X + inc, armature.RotationDegrees.Y, armature.RotationDegrees.Z);
        }
        else
        {
            if (armature.RotationDegrees.X > minRotateX)
                armature.RotationDegrees = new Vector3(armature.RotationDegrees.X - inc, armature.RotationDegrees.Y, armature.RotationDegrees.Z);
        }
    }
    public void RotateXWind(float inc)
    {
        if (armature.RotationDegrees.X < maxRotateX && armature.RotationDegrees.X > minRotateX)
            armature.RotationDegrees = new Vector3(armature.RotationDegrees.X + inc, armature.RotationDegrees.Y, armature.RotationDegrees.Z);
        SkiJumpStatic.landingScore = armature.RotationDegrees.X;
        MoveXArrowWind();
    }
    public void ShoHideControlSkiJumpImpulseHorizontal(bool isShow)
    {
        if (isShow)
            this.controlSkiJumpImpulseHorizontal.Show();
        else
            this.controlSkiJumpImpulseHorizontal.Hide();
    }
    public void ShoHideControlSkiJumpImpulseVertical(bool isShow)
    {
        if (isShow)
            this.controlSkiJumpImpulseVertical.Show();
        else
            this.controlSkiJumpImpulseVertical.Hide();
    }
    private void MoveZArrowWind()
    {
        if (armature.RotationDegrees.Z < 0)
        {            
            var perc = ((-1.0f * armature.RotationDegrees.Z) / 45.0f) * 100.0f;
            var pos = (perc * medRotateZUI) / 100.0f;
            windDirectionArrowHorizontal.Position = new Vector2(medRotateZUI + pos, windDirectionArrowHorizontal.Position.Y);
        }
        else if (armature.RotationDegrees.Z > 0)
        {
            
            var perc = ((-1.0f * armature.RotationDegrees.Z) / 45.0f) * 100.0f;
            var pos = (perc * medRotateZUI) / 100.0f;
            windDirectionArrowHorizontal.Position = new Vector2(medRotateZUI + pos, windDirectionArrowHorizontal.Position.Y);
        }
        else 
        {
            windDirectionArrowHorizontal.Position = new Vector2(medRotateZUI, windDirectionArrowHorizontal.Position.Y);
        }
        
    }
    private void MoveXArrowWind()
    {
        if (armature.RotationDegrees.X < 0)
        {
            var perc = ((-1.0f * armature.RotationDegrees.X) / 45.0f) * 100.0f;
            var pos = (perc * medRotateZUI) / 100.0f;
            windDirectionArrowVertical.Position = new Vector2(medRotateZUI + pos, windDirectionArrowVertical.Position.Y);
        }
        else if (armature.RotationDegrees.X > 0)
        {
            var perc = ((-1.0f * armature.RotationDegrees.X) / 45.0f) * 100.0f;
            var pos = (perc * medRotateZUI) / 100.0f;
            windDirectionArrowVertical.Position = new Vector2(medRotateZUI + pos, windDirectionArrowVertical.Position.Y);
        }
        else
        {
            windDirectionArrowVertical.Position = new Vector2(medRotateZUI, windDirectionArrowVertical.Position.Y);
        }

    }
    public void ResetSkiJump()
    {
        impulse = 180.0f;
        impulsePercent = 0.0f;
        impulseRail = 0.0f;
        angle = 0.0f;
        power = 0.0f;
        armature.RotationDegrees = Vector3.Zero;
        playerInput.PlayAnimation(animationPlayer, 1);
        playerInput.Init();
        playerInput.Reset();        
    }
    #endregion
    #region Ski Cross Country
    public void SetCrossCountryModel(List<CrossCountryModel> crossCountryModelList)
    {        
        this.crossCountryModelList = crossCountryModelList;
    }
    public void SetCrossCountryAIModel(List<List<CrossCountryModel>> crossCountryModelAIList)
    {
        this.crossCountryModelAIList = crossCountryModelAIList;
    }
    public void SetCurrentAILine(int currentAILine)
    {        
        this.currentAILine = currentAILine;        
    }
    public int GetSkiCrossCountryDistance()
    {        
        foreach (var crossCountryModel in crossCountryModelList)
        {
            crossCountryModel.distance = this.Position.DistanceTo(crossCountryModel.position);            
        }
        return crossCountryModelList.OrderBy(x => x.distance).First().id;
    }
    public int GetSkiCrossCountryCurrentId()
    {        
        if (playerInput.GetIsAI())
        {            
            foreach (var crossCountryModel in crossCountryModelAIList[playerInput.GetLinePosition()])
            {
                crossCountryModel.distance = this.Position.DistanceTo(crossCountryModel.position);
            }
            return crossCountryModelAIList[playerInput.GetLinePosition()].OrderBy(x => x.distance).First().id;
        }
        return GetSkiCrossCountryDistance();
    }
    public bool GetIsFinished() 
    {        
        return playerInput.GetIsFinished();
    }
    public bool GetIsAccel()
    {  
        return playerInput.GetIsAccel(); 
    }
    public bool GetIsBreak() 
    {
        return playerInput.GetIsBreak();
    }
    public void SetIsAI(bool isAI)
    { 
        this.isAI = isAI;        
    }
    public void SetScore()
    {        
        if (!isRunFinished && crossCountryOvertakeFM.GetSetIsFinished)
        {
            isRunFinished = true;
            currentPoint = 10000;            
        }
        if (!isRunFinished)
        {
            currentPoint = GetSkiCrossCountryCurrentId();
        }
    }
    public void ResetCrossCountry()
    {
        currentAILine = 0;
        score = 0.0f;
        isRunFinished = false;
        currentPoint = 0;
        isScoreSet = false;
        playerInput.Init(crossCountryModelAIList, currentAILine);
        playerInput.Reset();
        crossCountryOvertakeFM.GetSetIsFinished = false;
    }
    #endregion
    #region Ice Hockey
    public void HideAllNonIceHockeyItems()
    {
        foreach (var item in skiPole)
        {
            item.Hide();
        }
        foreach (var item in ski)
        {
            item.Hide();
        }
        foreach (var item in skiPoleBiathlon)
        {
            item.Hide();
        }
        rifle.Hide();
        speedBox.Hide();
        target.Hide();
        if (this.controlSkiSpeedSkatingBiathlon is not null)                 
            this.controlSkiSpeedSkatingBiathlon.Hide();
        if (this.controlSkiCrossCountry is not null)        
            this.controlSkiCrossCountry.Hide();
        if (this.controlBiathlon is not null)                 
            this.controlBiathlon.Hide();        
        if(this.controlSkiJumpImpulseHorizontal is not null)
            this.controlSkiJumpImpulseHorizontal.Hide();
        if (this.controlSkiJumpImpulseVertical is not null)
            this.controlSkiJumpImpulseVertical.Hide();
    }
    public void ShowHideIceHockeyStick(bool isShow)
    {
        if(isShow)
            stick.Show();
        else 
            stick.Hide();
    }
    public void ShowHideIceHockeyGoalKeeper(bool isShow)
    {
        foreach (var obj in goalKeeperItems)
        {
            if (isShow)
                obj.Show();
            else
                obj.Hide();
        }        
    }
    public void SetisPlayerTeam(bool isPlayerTeam)
    {
        this.isPlayerTeam = isPlayerTeam;
    }
    public void SetisSelected(bool isSelected)
    {
        this.isSelected = isSelected;
    }    
    public void SetPlayerIceHockeyPosition(int playerId, int teamID)
    {
        iceHockeyPosition = iceHockeyPositionArray[playerId];
        if(teamID == 0)
            iceHockeyPositionSide = iceHockeyPositionSideArray[playerId];    
        else
            if(playerId != 0 && playerId != 3)
                iceHockeyPositionSide = iceHockeyPositionSideArray[playerId] == IceHockeyPositionSide.R ? IceHockeyPositionSide.L : IceHockeyPositionSide.R;
            else
                iceHockeyPositionSide = iceHockeyPositionSideArray[playerId];
    }
    public void ShowHideIceHockeySeletion(bool isShow)
    {
        if (isShow)
            iceHockeySelect.Show();
        else
            iceHockeySelect.Hide();
        isIceHockeySeletion = isShow;        
    }
    public void SetPuckRefPosition()
    {
        if (playerInput.GetisPuckControl())
        {
            Transform3D newTransform = new Transform3D(originalTransform.Basis, puckRef.GlobalTransform.Origin);
            puck.Transform = newTransform;
            puck.LinearVelocity = Vector3.Zero;
        }        
    }
    public void SetPuckOriginalTransform(RigidBody3D puck)
    {
        if (isPuckControl)
        {
            this.puck = puck;
            originalTransform = puck.Transform;            
        }
        playerInput.SetPuck(puck);
        playerInput.SetisSelected(ref isSelected);
        playerInput.SetisPuckControl(ref isPuckControl);
        playerInput.SetObj<Node3D>(ShootRef);        
    }
    public void SetPuckOriginalTransform()
    {
        if (isPuckControl)
        {
            this.puck = playerInput.GetPuck();
            originalTransform = puck.Transform;
        }
        playerInput.SetPuck(puck);
        playerInput.SetisSelected(ref isSelected);
        playerInput.SetisPuckControl(ref isPuckControl);
        playerInput.SetObj<Node3D>(ShootRef);        
    }
    public void SetIceHockeyGoal(IceHockeyGoal Goal1, IceHockeyGoal Goal2)
    {
        this.Goal1 = Goal1;
        this.Goal2 = Goal2;
        playerInput.SetIceHockeyGoals(Goal1, Goal2);
    }
    public void SetIceHockeyTeams(ref List<Character> iceHockeyTeam1, ref List<Character> iceHockeyTeam2)
    {
        this.iceHockeyTeam1 = iceHockeyTeam1;
        this.iceHockeyTeam2 = iceHockeyTeam2;
        playerInput.SetIceHockeyTeams(ref this.iceHockeyTeam1, ref this.iceHockeyTeam2);
        playerInput.SetIsPlayerTeamPlayerNumber(isPlayerTeam, playerNumber);
    }
    private void DefineShootRef()
    {
        var ShootRefChild = this.GetParent().GetChildren();
        foreach (var obj in ShootRefChild)
        {
            if (obj.Name == "MeshInstance3D2")
                ShootRef = obj as Node3D;
        }
    }
    private void InitIceHockey()
    {        
        this.GetNode<Area3D>("MeshInstance3D/Area3D/").ProcessMode = ProcessModeEnum.Disabled;
        this.GetNode<Area3D>("SkiCollision/Area3D/").ProcessMode = ProcessModeEnum.Disabled;
        this.GetNode<Area3D>("CrossCountryCollisionF/Area3D/").ProcessMode = ProcessModeEnum.Disabled;
        this.GetNode<Area3D>("CrossCountryCollisionML/Area3D/").ProcessMode = ProcessModeEnum.Disabled;
        this.GetNode<Area3D>("CrossCountryCollisionMR/Area3D/").ProcessMode = ProcessModeEnum.Disabled;

        iceHockeyPlayerCollision = this.GetNode<IceHockeyPlayerCollision>("MeshInstance3DIceHockey/Area3D/");
    }
    public IPlayerInput GetPlayerInput()
    { 
        return this.playerInput;
    }
    public void SetGoalKeeperShoot(AnimationPlayer animationPlayer, int xDir, int zDir)
    {
        this.playerInput.SetGoalKeeperShoot(animationPlayer, xDir, zDir);
    }    
    #endregion
}
