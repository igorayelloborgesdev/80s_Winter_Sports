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
    #endregion    
    #region Variables    
    private IPlayerInput playerInput = null;
    private SceneTree sceneTree = null;
    private Control pauseScreen = null;
    private Control finishSessionScreen = null;
    private Control controlSkiSpeedSkatingBiathlon = null;
    private Control controlBiathlon = null;
    private string prefabName = String.Empty;
    private int startPointId = 0;
    private List<DirectionArrow> directionArrowList = new List<DirectionArrow>();
    private List<List<DirectionArrow>> directionArrowBiathlonList = new List<List<DirectionArrow>>();
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
    private float[] scaleFactorArray = { 1.0f, 1.5f };
    private float increment = 0.01f;
    private float shootInc = 30000.0f;
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
        Finish,
        Disqualified,
        Shooting
    };
    public StatesSki statesSki = StatesSki.Ready;
    #endregion
    #region Sport Biathlon    
    [Export] public Node3D rifle = null;
    [Export] Node3D[] skiPoleBiathlon = null;
    #endregion    
    #region Behavior
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {        
        Init();
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
    public override void _PhysicsProcess(double delta)
    {
        if (prefabName == "skiTrack")
        {
            if (statesSki > StatesSki.Go)
                playerInput.PlayerInput(animationPlayer);
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
    }
    #endregion
    #region Methods
    private void Init()
    {        
        PlayerInputSetUp();        
        playerInput.SetCharacterBody3D(this);
        playerInput.SetPauseScreen(pauseScreen);
        playerInput.SetFinishSessionScreen(finishSessionScreen);
        playerInput.PlayAnimation(animationPlayer, 1);
        playerInput.SetCharacter(this);
        playerInput.Init();            
        //Sport Ski 
        if (prefabName == "skiTrack")
            InitSki();
        //Sport Speed Skating
        if (prefabName == "SpeedSkating")
            InitSpeedSkating();
        //Sport Biathlon
        if (prefabName == "Biathlon")
            InitBiathlon();
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
    }
    public void Reset()
    {
        playerInput.PlayAnimation(animationPlayer, 1);
        playerInput.Init();
        playerInput.Reset();
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
    public void DisableCamera()
    {
        camera3D.Hide();
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
    public Control SetControlBiathlon
    {
        set
        {
            this.controlBiathlon = value;
        }
    }    
    public string SetPrefabName
    {
        set 
        { 
            this.prefabName = value; 
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
        if(isShow)
            this.controlSkiSpeedSkatingBiathlon.Show();
        else
            this.controlSkiSpeedSkatingBiathlon.Hide();
    }
    public void ShowHideControlBiathlon(bool isShow)
    {
        if (isShow)
            this.controlBiathlon.Show();
        else
            this.controlBiathlon.Hide();
    }
    public void SetBiathlonUILabels(Label shoots, Label errorLabelScore, Label windDirection, Control windDirectionArrow)
    {
        this.shoots = shoots;
        this.errorLabelScore = errorLabelScore;
        this.windDirection = windDirection;
        this.windDirectionArrow = windDirectionArrow;
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
}
