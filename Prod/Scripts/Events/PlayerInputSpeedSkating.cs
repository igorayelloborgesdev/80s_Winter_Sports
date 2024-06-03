using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WinterSports.Scripts.Controller;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Interfaces;
using WinterSports.Scripts.Static;
using static Godot.TextServer;

namespace WinterSports.Scripts.Events
{
    public class PlayerInputSpeedSkating : IPlayerInput
    {
        #region Constants
        IDictionary<int, string> animName = new Dictionary<int, string>()
        {
            {1,"SpeedSkating_Idle"},
            {2,"SpeedSkating_Run"}
        };
        #endregion
        #region Constant        
        private const float maxSpeed = 5.0f;        
        private const float speedInc = 0.01f;
        private const double timeSpeedCurrentMin = 0.02;
        private const double timeSpeedCurrentMax = 0.10;
        private const double timeSpeedCurrentInc = 0.01;
        #endregion
        #region Variables
        private string currentAnimation = "";
        private CharacterBody3D characterBody3D = null;
        private bool isPause = false;        
        private float speed = 0.0f;
        private Control pauseScreen = null;
        private int startPointId = 0;        
        private List<SpeedSkatingTrackDTO> speedSkatingTrackDTOList = new List<SpeedSkatingTrackDTO>();
        private TimerController timerGamePlayController = null;
        private int keyPressedId = 0;
        private bool keyEnable = false;
        private int indexSpeed = 0;
        private List<DirectionArrow> directionArrowList = new List<DirectionArrow>();
        #endregion
        #region Implements
        public void PlayerInput(AnimationPlayer animationPlayer, double delta)
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
                        if (!isPause)
                        {
                            if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[1].keyId) && !keyEnable)//Button 1
                            {
                                keyPressedId = ConfigSingleton.saveConfigDTO.keysControlArray[1].keyId;
                                indexSpeed = 3;
                                keyEnable = true;                                
                            }
                            else if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[2].keyId) && !keyEnable)//Button 2
                            {
                                keyPressedId = ConfigSingleton.saveConfigDTO.keysControlArray[2].keyId;
                                indexSpeed = 4;
                                keyEnable = true;                                
                            }
                            else if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[3].keyId) && !keyEnable)//Button 3
                            {
                                keyPressedId = ConfigSingleton.saveConfigDTO.keysControlArray[3].keyId;
                                indexSpeed = 2;
                                keyEnable = true;                                
                            }
                            else if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[4].keyId) && !keyEnable)//Button 4
                            {
                                keyPressedId = ConfigSingleton.saveConfigDTO.keysControlArray[4].keyId;
                                indexSpeed = 1;
                                keyEnable = true;                                
                            }                            
                        }                        
                    }
                    else
                    {
                        var joystickInput = Input.GetConnectedJoypads().FirstOrDefault();
                        if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[0].keyId) && !isPause)//Pause
                        {
                            Pause();
                        }                                               
                    }
                    PlayAnimation(animationPlayer, indexSpeed);
                }
                else
                {
                    if (ConfigSingleton.saveConfigDTO.keyboardJoystick == 0)
                    {
                        if (!isPause)
                        {
                            if (!Input.IsKeyPressed((Key)keyPressedId) && keyEnable)
                            {
                                keyEnable = false;

                                SpeedManager(indexSpeed);
                                indexSpeed = 0;
                            }
                        }
                    }                    
                }
                MovePlayer(delta);
            }
        }
        public void PlayAnimation(AnimationPlayer animationPlayer, int animID)
        {
            if (animationPlayer is not null)
            {
                if (animName.ContainsKey(animID))
                {
                    if (currentAnimation != animName[animID])
                    {
                        animationPlayer.Play(animName[animID]);
                        currentAnimation = animName[animID];
                    }
                }
            }
        }
        public void SetCharacterBody3D(CharacterBody3D rigidBody3D)
        {
            this.characterBody3D = rigidBody3D;
        }
        public void SetPauseScreen(Control pauseScreen)
        {
            this.pauseScreen = pauseScreen;
        }
        public void Pause()
        {
            isPause = !isPause;
            Engine.TimeScale = isPause ? 0.0f : 1.0f;
            ShowHidePauseMenu(isPause);
        }
        public void UnPause()
        {
            isPause = false;
            Engine.TimeScale = isPause ? 0.0f : 1.0f;
            ShowHidePauseMenu(isPause);
        }
        public void Init()
        {            
            speed = (float)timeSpeedCurrentMax;
            timerGamePlayController = new TimerController();
            timerGamePlayController.Init();
            timerGamePlayController.StartTimer();
        }
        public void Reset()
        {            
            speed = (float)timeSpeedCurrentMax;
        }
        public float GetSpeed()
        {
            return speed;
        }
        public float GetMaxSpeed()
        {
            return maxSpeed;
        }
        public void SetRailSpeedSkating(int startPointId, List<SpeedSkatingTrackDTO> speedSkatingTrackDTOList, List<DirectionArrow> directionArrowList) 
        {
            this.startPointId = startPointId;
            this.speedSkatingTrackDTOList = speedSkatingTrackDTOList;
            this.directionArrowList = directionArrowList;
        }
        #endregion
        #region Methods                       
        private void ShowHidePauseMenu(bool isPause)
        {
            if (isPause)
                pauseScreen.Show();
            else
                pauseScreen.Hide();
        }               
        private void MovePlayer(double delta)
        {                                 
            try
            {
                if (timerGamePlayController.GetTimer() > DefineSpeed())//<-
                {
                    if (speedSkatingTrackDTOList.Count == startPointId)
                        startPointId = 0;
                    else
                        startPointId++;
                    this.characterBody3D.Position = new Vector3(speedSkatingTrackDTOList[startPointId].position.X,
                        this.characterBody3D.Position.Y, speedSkatingTrackDTOList[startPointId].position.Z);
                    var newPositionLookAt = Vector3.Zero;
                    if (startPointId + 1 == speedSkatingTrackDTOList.Count)
                    {
                        newPositionLookAt = new Vector3(speedSkatingTrackDTOList[0].position.X,
                            this.characterBody3D.Position.Y, speedSkatingTrackDTOList[0].position.Z);
                    }
                    else
                    {
                        newPositionLookAt = new Vector3(speedSkatingTrackDTOList[startPointId + 1].position.X,
                            this.characterBody3D.Position.Y, speedSkatingTrackDTOList[startPointId + 1].position.Z);
                    }
                    if (this.characterBody3D.Position != newPositionLookAt)
                    {
                        this.characterBody3D.LookAt(newPositionLookAt, Vector3.Up);
                        this.characterBody3D.RotateObjectLocal(Vector3.Up, Mathf.DegToRad(180.0f));
                    }
                    timerGamePlayController.ResetTimerRunning();
                }
                timerGamePlayController.TimerRunning(delta);                
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
        private void SpeedManager(int index)
        {
            if (SpeedSkatingStatic.isCollided && (SpeedSkatingStatic.direction == index))
            {                
                speed -= (float)timeSpeedCurrentInc;//<-                
                SetArrowColor(1);
            }
            else 
            {             
                speed += (float)timeSpeedCurrentInc;//<-         
                SetArrowColor(2);
            }            
        }

        private void SetArrowColor(int colorId)
        {            
            directionArrowList[SpeedSkatingStatic.id].SetBodyColor(colorId);
        }
        #endregion        
    }
}
