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
using WinterSports.Scripts.Singleton;
using WinterSports.Scripts.Static;

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
        private const float speedDec = 0.02f;
        private const float brakeInc = 0.05f;
        private const float maxEnergy = 5.0f;
        private const float minEnergy = 0.25f;        
        private float[] energyArray = new float[] { 0.0f, 1.25f, 2.5f, 3.75f, 5.0f };
        private const float minSpeed = 0.25f;
        private const float speedIncCollision = 0.025f;
        private const float maxSpeedEnergy = 4.9f;
        private const float maxSpeedEnergyReturn = 4.75f;
        #endregion
        #region Variables
        private string currentAnimation = "";        
        private CharacterBody3D characterBody3D = null;
        private bool isPause = false;        
        private float angle = 0.0f;
        private float speed = 0.0f;        
        private Control pauseScreen = null;
        private Control finishSessionScreen = null;
        private float angleInc = 0.0f;
        private float energy = 0.0f;
        private float energyInc = 0.0f;        
        private SkiCollision skiCollision = null;
        private float speedEnergy = 0.0f;
        private bool isEnergySlow = false;
        private float energyDecrease = 1500.0f;
        private bool isAccel = false;
        private bool isBreak = false;        
        #endregion
        #region Variables AI
        private bool isAI = false;
        private int currentWayPoint = 0;
        private List<CrossCountryDTO> crossCountryDTOList = new List<CrossCountryDTO>();
        #endregion
        #region Implements
        public void PlayerInput(AnimationPlayer animationPlayer, double delta, int positionID) 
        {
            if (!isPause)
            {
                if (!isAI)
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
                    ManageCollisionSpeed();
                    MovePlayer();
                    CalcAngleDirection();
                }                
            }            
            if (!CrossCountryStatic.isPause && isAI)
            {
                MoveDirectionAI(positionID);                
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
        public void SetFinishSessionScreen(Control finishSessionScreen)
        {
            this.finishSessionScreen = finishSessionScreen;
        }
        public void Pause()
        {
            isPause = !isPause;
            Engine.TimeScale = isPause ? 0.0f : 1.0f;
            ShowHidePauseMenu(isPause);
            SetStaticPause();
        }
        public void UnPause()
        {
            isPause = false;
            Engine.TimeScale = isPause ? 0.0f : 1.0f;
            ShowHidePauseMenu(isPause);
            SetStaticPause();
        }
        public void ShowHideFinishSessionScreen()
        {
            isPause = !isPause;
            Engine.TimeScale = isPause ? 0.0f : 1.0f;
            ShowHideFinishSessionScreenMenu(isPause);
            SetStaticPause();
        }
        private void SetStaticPause()
        {
            if (!isAI)
            { 
                CrossCountryStatic.isPause = isPause;
            }
        }
        public void Init()
        {            
            angle = angleInit;
            speed = 0.0f;
            speedEnergy = 0.0f;
            angleInc = 0.0f;
            energy = maxEnergy;
            this.characterBody3D.Rotation = new Vector3(characterBody3D.Rotation.X, Mathf.DegToRad(angle), characterBody3D.Rotation.Z);
            if (isAI)
            {
                crossCountryDTOList = AISingleton.crossCountryObjDTO.CrossCountryDTOList.OrderBy(x => x.id).ToList();                
            }            
        }
        public void Reset()
        {
            angle = angleInit;
            speed = 0.0f;
            speedEnergy = 0.0f;
            this.characterBody3D.Rotation = new Vector3(characterBody3D.Rotation.X, Mathf.DegToRad(angle), characterBody3D.Rotation.Z);            
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
        public void SetSkiJumpPoint(int[] flyPoints) { }
        public void SetWindSkiJump(float angle, float power) { }
        public void SetSkiCollision(SkiCollision skiCollision) 
        { 
            this.skiCollision = skiCollision;
        }
        public void SetRailBiathlon(int startPointId, List<List<SpeedSkatingTrackDTO>> speedSkatingTrackDTOList, List<List<DirectionArrow>> directionArrowList){}
        public void SetCharacter(Character character)
        {

        }
        public bool GetPause()
        {
            return isPause;
        }
        public float GetEnergy()
        {
            return energy;
        }
        #endregion
        #region Methods
        private void AccelPlayer()
        {
            if (GameModeSingleton.sport == 12)
            {
                ManageEnergy();
                if (speed < maxSpeed)
                {                                     
                    speed = speed + speedInc;
                }
                if (speedEnergy < maxSpeedEnergy)
                {                 
                    speedEnergy = speedEnergy + speedInc;
                }                
            }
            else
            {
                if (speed < maxSpeed)
                {
                    speed += speedInc;                    
                }                
            }
            isAccel = true;
        }
        private void DecreaseSpeedPlayer()
        {
            if (GameModeSingleton.sport == 12)
            {                
                ManageEnergy();
                if (speed > 0.25f)
                {
                    speed -= speedInc + energyInc;
                    speedEnergy -= speedInc + energyInc;
                }
                else
                {
                    speed = 0.25f;
                    speedEnergy = 0.25f;
                }                
            }
            else
            {
                if (speed > 0.0f)
                {
                    speed -= speedInc;                    
                }
                else
                {
                    speed = 0.0f;                    
                }                
            }
            isAccel = false;
            isBreak = false;
        }
        private void BrakePlayer()
        {
            if (GameModeSingleton.sport == 12)
            {             
                ManageEnergy();
                if (speed > 0.25f)
                {
                    speed -= brakeInc + energyInc;
                    speedEnergy -= brakeInc + energyInc;
                }
                else
                {
                    speed = 0.25f;
                    speedEnergy = 0.25f;
                }                
            }
            else
            {
                if (speed > 0.0f)
                {
                    speed -= brakeInc;                    
                }
                else
                {
                    speed = 0.0f;                    
                }
                
            }
            isBreak = true;
        }
        private void ManageCollisionSpeed()
        {
            if (skiCollision.GetIsCollided)
            {
                if (speed > minSpeed)
                {
                    speed -= speedIncCollision;
                    speedEnergy -= speedIncCollision;
                }
                else
                {
                    speed = minSpeed;
                    speedEnergy = minSpeed;
                }
            }            
        }        
        private void ManageEnergy()
        {
            if (!isEnergySlow)
            {
                DecreaseEnergy();
                if (energy < energyArray[1])
                {
                    DecreaseSpeed();
                    if (speed < energy)
                    {
                        isEnergySlow = true;
                    }
                }
            }
            else
            {
                if (speed < energy)
                {
                    IncreaseEnergy();
                }
                if (speed > energy + (maxSpeed / 50.0f))
                {                    
                    DecreaseEnergy();
                    speed -= speedDec;
                    DecreaseSpeed();
                }
                if (energy >= maxSpeedEnergyReturn)
                {
                    isEnergySlow = false;
                }
            }                                   
            LimitEnergy();
        }        
        private void DecreaseEnergy()
        {                                   
            if (speed > energyArray[0] && speed <= energyArray[1])
                energyInc = energyArray[1] / energyDecrease;
            else if (speed > energyArray[1] && speed <= energyArray[2])
                energyInc = energyArray[2] / energyDecrease;
            else if (speed > energyArray[2] && speed <= energyArray[3])
                energyInc = energyArray[3] / energyDecrease;
            else if (speed > energyArray[3] && speed <= energyArray[4])
                energyInc = energyArray[4] / energyDecrease;
            else
                energyInc = energyArray[4] / energyDecrease;                
            energy -= energyInc;                        
        }
        private void DecreaseSpeed()
        {            
            if (energy <= energyArray[1])
            {
                speed -= speedDec;
            }
            if (speed < minSpeed)
            {
                speed = minSpeed;
            }
        }
        private void LimitEnergy()
        {
            if (energy >= maxEnergy)
            {
                energy = maxEnergy;
            }
            if (energy <= minEnergy)
            {
                energy = minEnergy;
            }        
        }
        private void IncreaseEnergy()
        {            
            if (speed > energyArray[0] && speed <= energyArray[1])
                energyInc = energyArray[4] / energyDecrease;
            else if (speed > energyArray[1] && speed <= energyArray[2])
                energyInc = energyArray[3] / energyDecrease;
            else if (speed > energyArray[2] && speed <= energyArray[3])
                energyInc = energyArray[2] / energyDecrease;
            else if (speed > energyArray[3] && speed <= energyArray[4])
                energyInc = energyArray[1] / energyDecrease;
            else
                energyInc = energyArray[1] / energyDecrease;
            energy += energyInc;            
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
        private void ShowHideFinishSessionScreenMenu(bool isPause)
        {
            if (isPause)
                finishSessionScreen.Show();
            else
                finishSessionScreen.Hide();
        }
        public bool GetIsFinished()
        {
            return skiCollision.GetIsFinished;
        }
        public bool GetIsAccel() 
        { 
            return isAccel; 
        }
        public bool GetIsBreak() 
        { 
            return isBreak; 
        }
        public void SetIsAI(bool isAI)
        { 
            this.isAI = isAI;
        }

        private void MoveDirectionAI(int positionID)
        {            
            Vector3 targetPosition = crossCountryDTOList[currentWayPoint].position;
            Vector3 currentPosition = this.characterBody3D.GlobalPosition;
            Vector3 direction = (targetPosition - currentPosition).Normalized();
            float targetYRotation = Mathf.Atan2(direction.X, direction.Z);
            float angleDegrees = Mathf.RadToDeg(targetYRotation);
            if ((int)angleDegrees != (int)this.characterBody3D.RotationDegrees.Y)
            {
                CalcAngleDirection();                
                if ((int)angleDegrees < (int)this.characterBody3D.RotationDegrees.Y)
                {
                    DirectPlayer(false);                    
                }
                else if ((int)angleDegrees > (int)this.characterBody3D.RotationDegrees.Y)
                {                    
                    DirectPlayer(true);
                }                
            }
            if (crossCountryDTOList[currentWayPoint].isAccel)
            {                
                AccelPlayer();                
            }
            if (crossCountryDTOList[currentWayPoint].id == positionID)
            {
                GD.Print("A");//<-PAREI AQUI
            }
            ManageCollisionSpeed();
            MovePlayer();
        }        
        #endregion
    }
}
