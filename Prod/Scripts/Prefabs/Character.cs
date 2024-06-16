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
    #endregion    
    #region Variables    
    private IPlayerInput playerInput = null;
    private SceneTree sceneTree = null;
    private Control pauseScreen = null;
    private string prefabName = String.Empty;
    private int startPointId = 0;
    private List<DirectionArrow> directionArrowList = new List<DirectionArrow>();
    #endregion
    #region Variables Speed Skating
    private List<SpeedSkatingTrackDTO> speedSkatingTrackDTOList = new List<SpeedSkatingTrackDTO>();    
    #endregion
    #region Constant
    private float[] scaleFactorArray = { 1.0f, 1.5f };
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
        Disqualified
    };
    public StatesSki statesSki = StatesSki.Ready;
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
            {
                playerInput.PlayerInput(animationPlayer, delta);                
            }            
        }
    }
    #endregion
    #region Methods
    private void Init()
    {        
        PlayerInputSetUp();
        playerInput.SetCharacterBody3D(this);
        playerInput.SetPauseScreen(pauseScreen);
        playerInput.PlayAnimation(animationPlayer, 1);
        playerInput.Init();
        //Sport Ski 
        if (prefabName == "skiTrack")
            InitSki();
        //Sport Speed Skating
        if (prefabName == "SpeedSkating")
            InitSpeedSkating();
    }
    private void PlayerInputSetUp()
    {
        //Sport Ski 
        if (prefabName == "skiTrack")
            playerInput = new PlayerInputSki();
        //Sport Speed Skating
        if (prefabName == "SpeedSkating")
            playerInput = new PlayerInputSpeedSkating();
    }
    public void Reset()
    {
        playerInput.PlayAnimation(animationPlayer, 1);
        playerInput.Init();        
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
    private void MoveCameraPositionRotation(int id)
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
    #endregion
    #region Get Set
    public Control SetPauseScreen
    {
        set 
        { 
            this.pauseScreen = value;
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
    }
    private void InitSpeedSkating()
    {
        //startGate.Connect("body_entered", new Callable(this, nameof(OnAreaEnteredStartGate)));//<-
        //finishGate.Connect("body_entered", new Callable(this, nameof(OnAreaEnteredFinishGate)));
        playerInput.SetRailSpeedSkating(startPointId, speedSkatingTrackDTOList, directionArrowList);        
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
}
