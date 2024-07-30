using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using WinterSports.Scripts.Controller;
using WinterSports.Scripts.DTO;

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
    #endregion
    #region Constant
    private float multiplyPoint = 200.0f;
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
        character.SetPlayerInputAnim();
        InstantiateRail();        
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
                    id = countId
                });
                countId++;
            }
        }
        startPoint = lugeMoveTrackDTOList.Count()/2;
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
    private void PlayerInput(double delta)//<-
    {
        if (!isPause)
        {
            JoystickInput.GetJoyPressed();
            if (Input.IsAnythingPressed())
            {

                if (Input.IsKeyPressed(Key.Enter) && !isPause)//Pause
                {
                    Pause();
                }
                if (Input.IsKeyPressed(Key.A))//Button 1
                {
                    //AccelPlayer();
                }
                if (Input.IsKeyPressed(Key.Left))//Left
                {
                    DirectPlayer(true);
                }
                if (Input.IsKeyPressed(Key.Right))//Right
                {
                    DirectPlayer(false);
                }


                //if (ConfigSingleton.saveConfigDTO.keyboardJoystick == 0)
                //{
                //if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[0].keyId) && !isPause)//Pause
                //{
                //    Pause();
                //}
                //if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[5].keyId))//Button 1
                //{             
                //    //AccelPlayer();
                //}                    
                //if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[3].keyId))//Left
                //{
                //    DirectPlayer(true);
                //}
                //if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[4].keyId))//Right
                //{
                //    DirectPlayer(false);
                //}
                //}
                //else
                //{
                //    var joystickInput = Input.GetConnectedJoypads().FirstOrDefault();
                //    if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[0].keyId) && !isPause)//Pause
                //    {
                //        Pause();
                //    }
                //    if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[5].keyId))//Button 1
                //    {                     
                //        //AccelPlayer();
                //    }                                        
                //    if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[3].keyId))//Left
                //    {
                //        DirectPlayer(true);
                //    }
                //    if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[4].keyId))//Right
                //    {
                //        DirectPlayer(false);
                //    }
                //}                
            }
            //MovePlayer();
            //CalcAngleDirection();
        }
    }
    private void Pause()
    {
        isPause = !isPause;
        Engine.TimeScale = isPause ? 0.0f : 1.0f;
        //ShowHidePauseMenu(isPause);//<-
    }
    private void DirectPlayer(bool isLeft = true)
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
        MoveSled(currentPoint);
        RotateSled(currentPoint);
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
    #endregion
}
