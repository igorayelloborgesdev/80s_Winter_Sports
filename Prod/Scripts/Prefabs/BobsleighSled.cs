using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WinterSports.Scripts.Controller;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Static;

public partial class BobsleighSled : Node3D
{
    #region Exports
    [Export] private Character[] character = null;
    [Export] private Node3D[] pointsList1 = null;
    [Export] private Node3D[] pointsList2 = null;
    [Export] private Node3D[] pointsList3 = null;
    [Export] private Bobsleigh sled = null;
    [Export] private Node3D[] jumpList1 = null;
    [Export] private Node3D[] jumpList2 = null;
    [Export] private Node3D[] jumpList3 = null;
    [Export] private Node3D[] jumpList4 = null;
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
    private bool isImpulse = false;
    private bool isBackToTrack = false;    
    private List<List<SpeedSkatingTrackDTO>> lugeJumpInDTOList = new List<List<SpeedSkatingTrackDTO>>();
    private bool[] charInBobsSleigh = new bool[] { false, false, false, false};
    private int[] charInBobsSleighIndex = new int[] { 0, 0, 0, 0 };
    private int currentCharacterJump = 0;
    private List<List<Node3D>> pointsJumpList = new List<List<Node3D>>();
    private bool isJumpFinished = false;
    #endregion
    #region Constant
    private const float multiplyPoint = 200.0f;
    private const float maxImpulse = 100.0f;
    private const double timeSpeedCurrentMin = 0.02;
    private const double timeSpeedCurrentMax = 0.10;
    private const double timeSpeedCurrentInc = 0.00003;
    private const double timeSpeedRampInc = 0.00005;
    private const double timeSpeedContactInc = 0.0001;
    private const double maxImpulseSpeed = 0.02;
    private const int increaseJumpSpeed = 30;
    #endregion
    #region Behaviors
    public override void _Ready()
    {
        Init();
    }
    public override void _PhysicsProcess(double delta)
    {
        PlayerInput(delta);
        //JumpInCar();
    }
    #endregion
    #region Methods
    private void Init()
    {        
        CharachtersInit();
        InstantiateRail();
        timerGamePlayController = new TimerController();
        timerGamePlayController.Init();
        timerGamePlayController.StartTimer();
        speed = (float)timeSpeedCurrentMax;
        JumpInCarInit();
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
        startPoint = lugeMoveTrackDTOList.Count() / 2;
        for (int i = 0; i < lugeMoveTrackDTOList.Count() / 2; i++)
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
    private void SetInitPointg()
    {
        currentPoint = startPoint;
        MoveSled(startPoint);
    }
    private void MoveSled(int index)
    {
        sled.Position = new Vector3(lugeMoveTrackDTOList[index].position.X, lugeMoveTrackDTOList[index].position.Y, sled.Position.Z);
    }
    private void CharachtersInit()
    {
        foreach (var obj in character)
        {
            obj.SpeedBoxShowHide(false);
            obj.ShowHideTarget(false);
            obj.DisableCamera();
            obj.HideItemsForLuge();
            obj.SetPlayerInputAnim(3);
        }
    }
    public void RunAnim()
    {
        foreach (var obj in character)
        {
            obj.SetPlayerInputAnim(4);
        }
    }
    private void JumpInCarInit()
    {
        pointsJumpList.Add(jumpList1.ToList());
        pointsJumpList.Add(jumpList2.ToList());
        pointsJumpList.Add(jumpList3.ToList());
        pointsJumpList.Add(jumpList4.ToList());

        for (int i = 0; i < pointsJumpList.Count; i++)
        {
            List<SpeedSkatingTrackDTO> lugeJumpInDTO = new List<SpeedSkatingTrackDTO>();
            int countId = 0;
            int points = (int)(RailLengthJumpInCarInit(i) * multiplyPoint);
            for (int j = 0; j < points; j++)
            {
                float t;
                Vector3 position;
                t = j / (points - 1.0f);
                position = (1.0f - t) * (1.0f - t) * pointsJumpList[i][0].Position + 2.0f * (1.0f - t) * t * pointsJumpList[i][1].Position + t * t * pointsJumpList[i][2].Position;
                lugeJumpInDTO.Add(new SpeedSkatingTrackDTO()
                {
                    position = position,
                    id = countId
                });
                countId++;
            }
            lugeJumpInDTOList.Add(lugeJumpInDTO);
        }        
    }
    private float RailLengthJumpInCarInit(int index)
    {
        var chord = (pointsJumpList[index][2].Position - pointsJumpList[index][0].Position).Length();
        var cont_net = (pointsJumpList[index][0].Position - pointsJumpList[index][1].Position).Length()
            + (pointsJumpList[index][2].Position - pointsJumpList[index][1].Position).Length();
        var app_arc_length = (cont_net + chord) / 2;
        return app_arc_length;
    }

    public void JumpInCar()
    {        
        if (currentCharacterJump < lugeJumpInDTOList.Count)
        {
            try
            {     
                if (charInBobsSleighIndex[currentCharacterJump] < lugeJumpInDTOList[currentCharacterJump].Count - 1 && !charInBobsSleigh[currentCharacterJump])
                {             
                    character[currentCharacterJump].SetPlayerInputAnim(5);
                    charInBobsSleighIndex[currentCharacterJump] += increaseJumpSpeed;
                    character[currentCharacterJump].Position = lugeJumpInDTOList[currentCharacterJump][charInBobsSleighIndex[currentCharacterJump]].position;
                    return;
                }
                charInBobsSleigh[currentCharacterJump] = true;
                currentCharacterJump++;
            }
            catch (Exception e)
            {
                charInBobsSleigh[currentCharacterJump] = true;
                currentCharacterJump++;
            }
        }
        else
        {
            isJumpFinished = true;
        }
    }
    public void Reset()
    {        
        for(int i = 0; i < character.Length; i++)
        {
            character[i].SetPlayerInputAnim(3);
            character[i].Position = lugeJumpInDTOList[i][0].position;
            charInBobsSleigh = new bool[] { false, false, false, false };
            charInBobsSleighIndex = new int[] { 0, 0, 0, 0 };
            currentCharacterJump = 0;
        }
        isImpulse = false;
        speed = (float)timeSpeedCurrentMax;
        isJumpFinished = false;
        isPause = false;
        startPoint = lugeMoveTrackDTOList.Count() / 2;
        currentPoint = 0;
        impulse = 180.0f;
        impulsePercent = 0.0f;
        startPointId = startPointIdInit;        
        isBackToTrack = false;
    }
    private void InstatiateRefBox(Vector3 position, int id)
    {
        var myMesh = new MeshInstance3D();
        myMesh.Mesh = new BoxMesh();
        myMesh.Name = "Mesh" + id.ToString();
        this.AddChild(myMesh);
        myMesh.Scale = myMesh.Scale * 0.1f;
        myMesh.Position = position;
    }
    public void SetPrefabName(string prefabName)
    {
        foreach (var character1 in character)
        {
            character1.SetPrefabName = prefabName;
        }        
    }
    public void ShowHideControlLuge()
    {
        controlSkiSpeedSkatingBiathlon.Show();
        controlBiathlon.Hide();
    }
    public void UnPause()
    {
        isPause = false;
        Engine.TimeScale = isPause ? 0.0f : 1.0f;
        ShowHidePauseMenu(isPause);
    }
    private void ShowHidePauseMenu(bool isPause)
    {
        if (isPause)
            pauseScreen.Show();
        else
            pauseScreen.Hide();
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
                    if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[3].keyId))//Left
                    {
                        DirectPlayer(true);
                    }
                    if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[4].keyId))//Right
                    {
                        DirectPlayer(false);
                    }
                }
            }
            else
            {
                isImpulse = false;
            }
            MovePlayer(delta);
            ReturnToBase();
        }

    }
    private void ReturnToBase()
    {
        if (this.character[0].statesSki == Character.StatesSki.Running && LugeStatic.statesLuge == LugeStatic.StatesLuge.trackSpeed && LugeStatic.isSpeedNormal)
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
            if (currentPoint > (lugeMoveTrackDTOList.Count / 2) - 30 && (lugeMoveTrackDTOList.Count / 2) + 30 > currentPoint)
            {
                isBackToTrack = false;
            }
        }
    }
    private void MovePlayer(double delta)
    {
        try
        {            
            if (this.character[0].statesSki == Character.StatesSki.Running || 
                this.character[0].statesSki == Character.StatesSki.JumpInCarBobsleigh || 
                this.character[0].statesSki == Character.StatesSki.RunningBobsleigh)
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
    private void DefineIncreaseSpeed()
    {        
        if (this.character[0].statesSki == Character.StatesSki.JumpInCarBobsleigh || this.character[0].statesSki == Character.StatesSki.RunningBobsleigh)
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
        if (this.character[0].statesSki == Character.StatesSki.Running)
        {
            speed += (float)timeSpeedCurrentInc * 4.0f;
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
    private void DefineImpulse()
    {
        if (this.character[0].statesSki == Character.StatesSki.Ready || this.character[0].statesSki == Character.StatesSki.Running)
        {
            DefineInitSpeed();
            this.character[0].statesSki = Character.StatesSki.Running;            
        }
    }
    private void DirectPlayer(bool isLeft = true)
    {
        if ((this.character[0].statesSki == Character.StatesSki.JumpInCarBobsleigh || this.character[0].statesSki == Character.StatesSki.RunningBobsleigh) && !isBackToTrack)
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
    private void DefineInitSpeed()
    {
        if (!isImpulse)
        {            
            speed -= (impulsePercent * (float)maxImpulseSpeed) / 100.0f;
            isImpulse = true;
        }        
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
    #endregion
    #region Get Set      
    public Character[] GetCharacter
    {
        get
        {
            return character;
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
    public Bobsleigh GetBobsleigh
    {
        get
        {
            return sled;
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
    public float GetMaxImpulse
    {
        get
        {
            return maxImpulse;
        }
    }
    public bool GetIsJumpFinished
    { 
        get 
        { 
            return isJumpFinished; 
        }
    }
    #endregion
}
