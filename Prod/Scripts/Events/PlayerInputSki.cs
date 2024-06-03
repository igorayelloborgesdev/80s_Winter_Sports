using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Interfaces;

namespace WinterSports.Scripts.Events
{
    public class PlayerInputSki : IPlayerInput
    {
        #region Constants
        IDictionary<int, string> animName = new Dictionary<int, string>()
        {
            {1,"Idle"},
            {3,"Ski_Left"},
            {4,"Ski_Right"},
            {5,"Ski_Fast"},
            {6,"Ski_Fast"},
        };
        #endregion
        #region Constant        
        private const float maxSpeed = 5.0f;        
        private const float angleInit = 90.0f;        
        private const float angleMin = 0.1f;
        private const float angleMax = 0.5f;
        private const float speedInc = 0.01f;
        private const float brakeInc = 0.05f;
        #endregion
        #region Variables
        private string currentAnimation = "";        
        private CharacterBody3D characterBody3D = null;
        private bool isPause = false;        
        private float angle = 0.0f;
        private float speed = 0.0f;
        private Control pauseScreen = null;
        private float angleInc = 0.0f;
        #endregion
        #region Implements
        public void PlayerInput(AnimationPlayer animationPlayer, double delta) 
        {
            if (!isPause)
            {
                JoystickInput.GetJoyPressed();
                if (Input.IsAnythingPressed())
                {
                    var index = 5;
                    if (ConfigSingleton.saveConfigDTO.keyboardJoystick == 0)
                    {
                        if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[0].keyId) && !isPause)//Pause
                        {                            
                            Pause();
                        }
                        if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[5].keyId))//Button 1
                        {
                            index = 5;
                            AccelPlayer();
                        }
                        else
                        {
                            DecreaseSpeedPlayer();
                        }
                        if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[6].keyId))//Button 2
                        {
                            index = 6;
                            BrakePlayer();
                        }
                        if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[3].keyId))//Left
                        {
                            index = 3;
                            DirectPlayer(true);
                        }
                        if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[4].keyId))//Right
                        {
                            index = 4;
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
                            index = 5;
                            AccelPlayer();
                        }
                        else
                        {
                            DecreaseSpeedPlayer();
                        }
                        if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[6].keyId))//Button 2
                        {
                            index = 6;
                            BrakePlayer();
                        }
                        if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[3].keyId))//Left
                        {
                            index = 3;
                            DirectPlayer(true);
                        }
                        if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[4].keyId))//Right
                        {
                            index = 4;
                            DirectPlayer(false);
                        }
                    }
                    PlayAnimation(animationPlayer, index);
                }
                else
                {
                    PlayAnimation(animationPlayer, 5);
                    DecreaseSpeedPlayer();
                }
                MovePlayer();
                CalcAngleDirection();
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
            angle = angleInit;
            speed = 0.0f;
            angleInc = 0.0f;
        }
        public void Reset()
        {
            angle = 0.0f;
            speed = 0.0f;
        }
        public float GetSpeed()
        {
            return speed;
        }
        public float GetMaxSpeed()
        {
            return maxSpeed;
        }
        public void SetRailSpeedSkating(int startPointId, List<SpeedSkatingTrackDTO> speedSkatingTrackDTOList, List<DirectionArrow> directionArrowList) { }
        #endregion
        #region Methods
        private void AccelPlayer()
        {
            if(speed < maxSpeed)
                speed += speedInc;                        
        }
        private void DecreaseSpeedPlayer()
        {
            if (speed > 0.0f)
                speed -= speedInc;
            else
                speed = 0.0f;
        }
        private void BrakePlayer()
        {
            if (speed > 0.0f)
                speed -= brakeInc;
            else
                speed = 0.0f;            
        }
        private void MovePlayer()
        {            
            float hyp = speed;
            Vector3 dir = Vector3.Zero;
            dir.X += Mathf.Sin(Mathf.DegToRad(angle)) * hyp;
            dir.Z += Mathf.Cos(Mathf.DegToRad(angle)) * hyp;
            this.characterBody3D.Velocity = dir;
            this.characterBody3D.MoveAndSlide();
        }        
        private void DirectPlayer(bool isLeft = true)
        {                              
            if (isLeft)
            {                
                angle += angleInc;
            }
            else
            {                                
                angle -= angleInc;
            }
            this.characterBody3D.Rotation = new Vector3(characterBody3D.Rotation.X, Mathf.DegToRad(angle), characterBody3D.Rotation.Z);                        
        }
        private void ShowHidePauseMenu(bool isPause)
        {
            if(isPause)
                pauseScreen.Show();
            else
                pauseScreen.Hide();
        }        
        private void CalcAngleDirection()
        {
            var speedCalc = speed/ maxSpeed;
            var angleCalc = angleMax -((angleMax - angleMin) * speedCalc);
            angleInc = angleCalc;            
        }
        #endregion
    }
}
