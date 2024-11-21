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
        #region Constant        
        IDictionary<int, string> animName = new Dictionary<int, string>()
        {
            {1,"SpeedSkating_Idle"},
            {2,"SpeedSkating_Run"}
        };
        private const float maxSpeed = 5.0f;                
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
        private Control finishSessionScreen = null;
        private int startPointId = 0;        
        private List<SpeedSkatingTrackDTO> speedSkatingTrackDTOList = new List<SpeedSkatingTrackDTO>();
        private TimerController timerGamePlayController = null;
        private int keyPressedId = 0;
        private bool keyEnable = false;
        private int indexSpeed = 0;
        private List<DirectionArrow> directionArrowList = new List<DirectionArrow>();
        #endregion
        #region Implements
        public void PlayerInput(AnimationPlayer animationPlayer, double delta, int positionID = 0, 
            CrossCountryOvertake crossCountryOvertakeM = null, CrossCountryOvertake crossCountryOvertakeFR = null, CrossCountryOvertake crossCountryOvertakeFL = null)
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
                PlayAnimation(animationPlayer, 2);
                GetNotScore();
            }
        }
        public void PlayAnimation(AnimationPlayer animationPlayer, int animID)
        {            
            if (animationPlayer is not null)
            {                
                if (animName.ContainsKey(animID))
                {
                    var speedAnimScale = 2.0f + ((timeSpeedCurrentMax - speed) * 10.0f);                    
                    animationPlayer.SpeedScale = (float)speedAnimScale;
                    animationPlayer.Play(animName[animID]);
                    currentAnimation = animName[animID];                    
                }
            }
        }
        public void SetCharacterBody3D(CharacterBody3D rigidBody3D)
        {
            this.characterBody3D = rigidBody3D;
        }
        public void SetSkiCollision(SkiCollision skiCollision) { }
        public void SetPauseScreen(Control pauseScreen)
        {
            this.pauseScreen = pauseScreen;
        }
        public void SetFinishSessionScreen(Control finishSessionScreen)
        {
            this.finishSessionScreen = finishSessionScreen;
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
        public void ShowHideFinishSessionScreen()
        {
            isPause = !isPause;
            Engine.TimeScale = isPause ? 0.0f : 1.0f;
            ShowHideFinishSessionScreenMenu(isPause);
        }
        public void Init()
        {            
            speed = (float)timeSpeedCurrentMax;
            timerGamePlayController = new TimerController();
            timerGamePlayController.Init();
            timerGamePlayController.StartTimer();
            startPointId = 0;            
        }
        public void Reset()
        {            
            speed = (float)timeSpeedCurrentMax;            
        }
        public float GetSpeed()
        {
            return (float)(-1.0f * ((speed - timeSpeedCurrentMin) - (timeSpeedCurrentMax - timeSpeedCurrentMin)));
        }
        public float GetMaxSpeed()
        {
            return (float)(timeSpeedCurrentMax - timeSpeedCurrentMin);
        }
        public void SetSkiJumpPoint(int[] flyPoints) { }
        public void SetWindSkiJump(float angle, float power) { }
        public void SetRailSpeedSkating(int startPointId, List<SpeedSkatingTrackDTO> speedSkatingTrackDTOList, List<DirectionArrow> directionArrowList) 
        {
            this.startPointId = startPointId;
            this.speedSkatingTrackDTOList = speedSkatingTrackDTOList;
            this.directionArrowList = directionArrowList;
        }
        public void SetRailBiathlon(int startPointId, List<List<SpeedSkatingTrackDTO>> speedSkatingTrackDTOList, List<List<DirectionArrow>> directionArrowList)
        {
        }
        public void SetCharacter(Character character)
        {
         
        }
        public float GetEnergy()
        {
            return 0.0f;
        }
        public bool GetIsFinished() { return false; }
        public bool GetIsAccel() { return false; }
        public bool GetIsBreak() { return false; }
        public void SetIsAI(bool isAI) { }
        #endregion
        #region Methods  
        private void ShowHideFinishSessionScreenMenu(bool isPause)
        {
            if (isPause)
                finishSessionScreen.Show();
            else
                finishSessionScreen.Hide();
        }
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
                if (timerGamePlayController.GetTimer() > DefineSpeed())
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
                speed -= (float)timeSpeedCurrentInc;           
                SetArrowColor(1);
                SetPlayerScore(true);
            }
            else 
            {                
                speed += (float)timeSpeedCurrentInc;
                SetArrowColor(2);
                SetEnableDisableArrow(false);
                SetPlayerScore(false);
            }            
        }
        private void SetArrowColor(int colorId)
        {
            try
            {
                directionArrowList[SpeedSkatingStatic.id].SetBodyColor(colorId);
            }
            catch (Exception ex)
            {
                SpeedSkatingStatic.id = 0;
                SpeedSkatingStatic.currentIndex = 0;
            }
            
        }
        private void SetEnableDisableArrow(bool enable)
        {
            try
            {
                directionArrowList[SpeedSkatingStatic.id].enable = enable;
            }catch (Exception ex) 
            {
                SpeedSkatingStatic.id = 0;
                SpeedSkatingStatic.currentIndex = 0;
            }            
        }
        private void SetPlayerScore(bool enable)
        {
            try
            {
                directionArrowList[SpeedSkatingStatic.id].playerScore = enable;
            }
            catch (Exception ex)
            {                
            }
        }
        private void GetNotScore()
        {
            if (SpeedSkatingStatic.isNotScore)
            {                
                SpeedSkatingStatic.isNotScore = false;                
                speed += (float)timeSpeedCurrentInc;                
            }

        }

        public bool GetPause()
        {
            return isPause;
        }
        #endregion        
    }
}
