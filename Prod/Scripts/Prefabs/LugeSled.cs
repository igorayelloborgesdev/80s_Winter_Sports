using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using WinterSports.Scripts.Controller;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Interfaces;
using WinterSports.Scripts.Static;
using static System.Net.Mime.MediaTypeNames;

public partial class LugeSled : Node3D
{
    #region Exports
    [Export] Character character = null;
    [Export] private Node3D[] pointsList1 = null;
    [Export] private Node3D[] pointsList2 = null;
    [Export] private Node3D[] pointsList3 = null;
    [Export] private Node3D sled = null;    
    #endregion
    #region Variables        
    private List<List<Node3D>> pointsList = new List<List<Node3D>>();
    private List<SpeedSkatingTrackDTO> lugeMoveTrackDTOList = new List<SpeedSkatingTrackDTO>();
    private List<int> pointsRotationList = new List<int>();
    private bool isPause = false;
    private int startPoint = 0;
    private int currentPoint = 0;    
    private List<SpeedSkatingTrackDTO> speedSkatingTrackDTOList = new List<SpeedSkatingTrackDTO>();
    private Control pauseScreen = null;
    private Control controlSkiSpeedSkatingBiathlon = null;
    private Control controlBiathlon = null;
    private float impulse = 180.0f;
    private float impulsePercent = 0.0f;
    private int startPointId = 0;
    private int startPointIdInit = 0;
    private TimerController timerGamePlayController = null;
    private float speed = 0.0f;
    private bool isBackToTrack = false;    
    #endregion
    #region Constant
    private const float multiplyPoint = 200.0f;    
    private const float maxImpulse = 100.0f;
    private const double timeSpeedCurrentMin = 0.04;
    private const double timeSpeedCurrentMax = 0.10;
    private const double timeSpeedCurrentInc = 0.00001;
    private const double timeSpeedRampInc = 0.00003;    
    private const double timeSpeedContactInc = 0.0001;
    private const double maxImpulseSpeed = 0.02;
    #endregion
    #region Behaviors
    public override void _Ready()
    {        
        Init();        
    }
    public override void _PhysicsProcess(double delta)
    {        
        PlayerInput(delta);
    }
    #endregion
    #region Methods
    private void Init()
    {        
        character.SpeedBoxShowHide(false);
        character.ShowHideTarget(false);
        character.DisableCamera();
        character.HideItemsForLuge();
        character.SetPlayerInputAnim(1);
        character.ShowHideIceHockeyStick(false);
        character.ShowHideIceHockeyGoalKeeper(false);
        InstantiateRail();
        timerGamePlayController = new TimerController();
        timerGamePlayController.Init();
        timerGamePlayController.StartTimer();
    }
    private void SetInitPointg()
    {
        currentPoint = startPoint;
        MoveSled(startPoint);
    }
    private void InstantiateRail()
    {
        pointsList.Add(pointsList1.ToList());
        pointsList.Add(pointsList2.ToList());
        pointsList.Add(pointsList3.ToList());        
        int countId = 0;
        pointsRotationList.Add(0);
        for (int a = 0; a < pointsList.Count; a++)
        {            
            int points = (int)(RailLength(a) * multiplyPoint);
            pointsRotationList.Add(pointsRotationList[pointsRotationList.Count() - 1] + points);
            for (int j = 0; j < points; j++)
            {                
                float t;
                Vector3 position;
                t = j / (points - 1.0f);
                position = (1.0f - t) * (1.0f - t) * pointsList[a][0].Position + 2.0f * (1.0f - t) * t * pointsList[a][1].Position + t * t * pointsList[a][2].Position;                
                lugeMoveTrackDTOList.Add(new SpeedSkatingTrackDTO()
                {
                    position = position,
                    id = countId,
                    isRamp = a % 2 == 0 ? true : false,
                    incRamp = (float)((a == 0 ? points - j : j) / 1000000.0f),
                    isLeftWall = false
                });
                countId++;
            }
        }        
        startPoint = lugeMoveTrackDTOList.Count()/2;
        for (int i = 0; i < lugeMoveTrackDTOList.Count()/ 2; i++) 
        {
            lugeMoveTrackDTOList[i].isLeftWall = true;
        }
        SetInitPointg();
    }
    private float RailLength(int indexX)
    {
        var chord = (pointsList[indexX][2].Position - pointsList[indexX][0].Position).Length();
        var cont_net = (pointsList[indexX][0].Position - pointsList[indexX][1].Position).Length()
            + (pointsList[indexX][2].Position - pointsList[indexX][1].Position).Length();
        var app_arc_length = (cont_net + chord) / 2;
        return app_arc_length;
    }
    private void PlayerInput(double delta)
    {
        if (!isPause)
        {
            JoystickInput.GetJoyPressed();
            if (Input.IsAnythingPressed())
            {
                if (ConfigSingleton.saveConfigDTO.keyboardJoystick == 0)
                {                    
                    if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[0].keyId) && !isPause)//Pause
                    {
                        Pause();
                    }
                    if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[5].keyId))//Button 1
                    {
                        DefineImpulse();                        
                    }
                    if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[3].keyId))//Left
                    {
                        DirectPlayer(true);
                    }
                    if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[4].keyId))//Right
                    {
                        DirectPlayer(false);
                    }
                }
                else
                {
                    var joystickInput = Input.GetConnectedJoypads().FirstOrDefault();
                    if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[0].keyId) && !isPause)//Pause
                    {
                        Pause();
                    }
                    if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[5].keyId))//Button 1
                    {
                        DefineImpulse();                        
                    }
                    float axisX = Input.GetJoyAxis(joystickInput, JoyAxis.LeftX);
                    if (axisX < 0)//Button 3
                    {
                        DirectPlayer(true);
                    }
                    else if (axisX > 0)//Button 4
                    {
                        DirectPlayer(false);
                    }
                    //if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[3].keyId))//Left
                    //{
                    //    DirectPlayer(true);
                    //}
                    //if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[4].keyId))//Right
                    //{
                    //    DirectPlayer(false);
                    //}
                }
            }            
            MovePlayer(delta);
            ReturnToBase();            
        }
        
    }    
    private void ReturnToBase()
    {     
        if (this.character.statesSki == Character.StatesSki.Running && LugeStatic.statesLuge == LugeStatic.StatesLuge.trackSpeed && LugeStatic.isSpeedNormal)
        {
            isBackToTrack = true;
        }
        if (isBackToTrack)
        {            
            if (lugeMoveTrackDTOList[currentPoint].isLeftWall)
            {
                currentPoint += 2;
            }
            if (!lugeMoveTrackDTOList[currentPoint].isLeftWall)
            {
                currentPoint -= 2;
            }
            MoveSled(currentPoint);
            RotateSled(currentPoint);                        
            if (currentPoint > (lugeMoveTrackDTOList.Count/ 2) - 30 && (lugeMoveTrackDTOList.Count / 2) + 30 > currentPoint)
            {
                isBackToTrack = false;
            }
        }
    }
    private void Pause()
    {
        isPause = !isPause;
        Engine.TimeScale = isPause ? 0.0f : 1.0f;
        ShowHidePauseMenu(isPause);
    }
    public void Pause(bool aIsPause)
    {
        isPause = aIsPause;
        Engine.TimeScale = isPause ? 0.0f : 1.0f;        
    }
    private void DirectPlayer(bool isLeft = true)
    {
        if (this.character.statesSki == Character.StatesSki.Running && !isBackToTrack)
        {            
            if (!LugeStatic.isColliding)
            {                
                if (isLeft)
                {
                    currentPoint--;
                    if (currentPoint < 0)
                        currentPoint = 0;
                }
                else
                {
                    currentPoint++;
                    if (currentPoint >= lugeMoveTrackDTOList.Count())
                        currentPoint = lugeMoveTrackDTOList.Count() - 1;
                }                
            }
            else
            {                
                if (lugeMoveTrackDTOList[currentPoint].isLeftWall)
                {
                    if (!isLeft)
                    {                        
                        currentPoint++;                        
                    }                    
                }
                if (!lugeMoveTrackDTOList[currentPoint].isLeftWall)
                {
                    if (isLeft)
                    {
                        currentPoint--;
                    }
                }
            }
            MoveSled(currentPoint);
            RotateSled(currentPoint);
        }        
    }
    private void MoveSled(int index)
    {        
        sled.Position = new Vector3(lugeMoveTrackDTOList[index].position.X, lugeMoveTrackDTOList[index].position.Y, sled.Position.Z);        
    }
    private void RotateSled(int index)
    {
        var rotation = 0.0f;
        var multiply = 5.0f;
        if (index >= pointsRotationList[0] && index < pointsRotationList[1])
        {         
            rotation = (pointsRotationList[1] - index) * multiply;
            rotation = rotation > 90.0f ? 90.0f : rotation;
        }        
        if (index >= pointsRotationList[2] && index < pointsRotationList[3])
        {            
            rotation = (pointsRotationList[2] - index) * multiply;
            rotation = rotation < -90.0f ? -90.0f : rotation;
        }        
        sled.RotationDegrees = new Vector3(sled.RotationDegrees.X, sled.RotationDegrees.Y, rotation);
    }
    public void SetPrefabName(string prefabName)
    {        
        character.SetPrefabName = prefabName;
    }
    public void UnPause()
    {        
        isPause = false;
        Engine.TimeScale = isPause ? 0.0f : 1.0f;
        ShowHidePauseMenu(isPause);
    }
    public void ShowHideControlLuge()
    {
        controlSkiSpeedSkatingBiathlon.Show();
        controlBiathlon.Hide();
    }
    public float CalculateImpulsePercent()
    {        
        if (!isPause)
        {     
            impulse += 5.0f;
            var normalize = 1.0f + MathF.Cos((float)((Math.PI / 180.0) * (double)impulse));
            impulsePercent = (normalize * 100.0f) / 2.0f;
            return impulsePercent;
        }
        return 0.0f;
    }    
    private void DefineImpulse()
    {
        if (this.character.statesSki == Character.StatesSki.Ready)
        {
            DefineInitSpeed();
            this.character.statesSki = Character.StatesSki.Running;
            character.SetPlayerInputAnim(2);            
        }        
    }
    private void DefineInitSpeed()
    {
        speed = (float)timeSpeedCurrentMax - ((impulsePercent * (float)maxImpulseSpeed)/ 100.0f);        
    }    
    private void DefineIncreaseSpeed()
    {
        if (!LugeStatic.isColliding)
        {
            if (LugeStatic.statesLuge == LugeStatic.StatesLuge.trackNormal)
            {             
                speed -= (float)timeSpeedCurrentInc;
            }
            else if (LugeStatic.statesLuge == LugeStatic.StatesLuge.trackSlow)
            {             
                speed += (float)timeSpeedCurrentInc;
            }
            else if (LugeStatic.statesLuge == LugeStatic.StatesLuge.trackSpeed)
            {                
                speed -= (float)timeSpeedRampInc;
            }
            if (lugeMoveTrackDTOList[currentPoint].isRamp)
            {
                speed -= lugeMoveTrackDTOList[currentPoint].incRamp;
            }            
        }
        else
        {                        
            speed += (float)timeSpeedContactInc * 5.0f;            
        }
    }
    private void MovePlayer(double delta)
    {
        try
        {
            if (this.character.statesSki == Character.StatesSki.Running)
            {                
                DefineIncreaseSpeed();
                if (timerGamePlayController.GetTimer() > DefineSpeed())
                {
                    startPointId++;
                    this.Position = new Vector3(speedSkatingTrackDTOList[startPointId].position.X,
                                        this.Position.Y, speedSkatingTrackDTOList[startPointId].position.Z);
                    var newPositionLookAt = Vector3.Zero;
                    if (startPointId + 1 == speedSkatingTrackDTOList.Count)
                    {
                        newPositionLookAt = new Vector3(speedSkatingTrackDTOList[startPointId].position.X,
                            this.Position.Y, speedSkatingTrackDTOList[startPointId].position.Z);
                    }
                    else
                    {
                        newPositionLookAt = new Vector3(speedSkatingTrackDTOList[startPointId + 1].position.X,
                            this.Position.Y, speedSkatingTrackDTOList[startPointId + 1].position.Z);
                    }
                    if (this.Position != newPositionLookAt)
                    {
                        this.LookAt(newPositionLookAt, Vector3.Up);
                        this.RotateObjectLocal(Vector3.Up, Mathf.DegToRad(180.0f));
                    }
                    timerGamePlayController.ResetTimerRunning();
                }
                timerGamePlayController.TimerRunning(delta);                
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void MovePlayerReset()
    {
        try
        {                                           
            this.Position = new Vector3(speedSkatingTrackDTOList[startPointId].position.X,
                                this.Position.Y, speedSkatingTrackDTOList[startPointId].position.Z);
            var newPositionLookAt = Vector3.Zero;
            if (startPointId + 1 == speedSkatingTrackDTOList.Count)
            {
                newPositionLookAt = new Vector3(speedSkatingTrackDTOList[startPointId].position.X,
                    this.Position.Y, speedSkatingTrackDTOList[startPointId].position.Z);
            }
            else
            {
                newPositionLookAt = new Vector3(speedSkatingTrackDTOList[startPointId + 1].position.X,
                    this.Position.Y, speedSkatingTrackDTOList[startPointId + 1].position.Z);
            }
            if (this.Position != newPositionLookAt)
            {
                this.LookAt(newPositionLookAt, Vector3.Up);
                this.RotateObjectLocal(Vector3.Up, Mathf.DegToRad(180.0f));
            }
            SetInitPointg();
            RotateSled(currentPoint);
        }
        catch (Exception ex)
        {
        }
    }
    private double DefineSpeed()
    {        
        if (speed > timeSpeedCurrentMax)
        {
            speed = (float)timeSpeedCurrentMax;
            return timeSpeedCurrentMax;
        }
        if (speed < timeSpeedCurrentMin)
        {
            speed = (float)timeSpeedCurrentMin;
            return timeSpeedCurrentMin;
        }
        return speed;
    }
    public void MovePlayerToStartPosition()
    {
        try
        {            
            this.Position = new Vector3(speedSkatingTrackDTOList[startPointId].position.X,
                                this.Position.Y, speedSkatingTrackDTOList[startPointId].position.Z);         
        }
        catch (Exception ex)
        {
        }
    }
    private void ShowHidePauseMenu(bool isPause)
    {
        if (isPause)
            pauseScreen.Show();
        else
            pauseScreen.Hide();
    }
    public void Reset()
    {        
        isPause = false;
        startPoint = lugeMoveTrackDTOList.Count() / 2;
        currentPoint = 0;
        impulse = 180.0f;
        impulsePercent = 0.0f;
        startPointId = startPointIdInit;
        speed = 0.0f;
        isBackToTrack = false;
    }
    #endregion
    #region Get Set      
    public Character GetCharacter
    {
        get 
        { 
            return character;
        }
    }
    public Control SetPauseScreen
    {
        set
        {
            this.pauseScreen = value;
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
    public float GetMaxImpulse
    { 
        get 
        { 
            return maxImpulse; 
        }
    }
    public int GetSetStartPointId
    { 
        get 
        { 
            return startPointId; 
        }
        set 
        { 
            startPointId = value; 
        }
    }
    public int GetSetStartPointIdInit
    {
        get
        {
            return startPointIdInit;
        }
        set
        {
            startPointIdInit = value;
        }
    }    
    public List<SpeedSkatingTrackDTO> SetSpeedSkatingTrackDTOList
    {
        set
        {
            speedSkatingTrackDTOList = value;
        }
    }
    public double GetMaxSpeed
    {
        get 
        {
            return timeSpeedCurrentMax - timeSpeedCurrentMin;
        }    
    }
    public double GetSpeed
    {
        get
        {
            return (timeSpeedCurrentMax - speed) == 0.1 ? 0.0 : timeSpeedCurrentMax - speed;
        }
    }
    #endregion
}
