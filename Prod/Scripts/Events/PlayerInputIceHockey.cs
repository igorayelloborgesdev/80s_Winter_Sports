using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Interfaces;
using WinterSports.Scripts.Model;

namespace WinterSports.Scripts.Events
{
    public class PlayerInputIceHockey : IPlayerInput
    {
        #region Variables
        private bool isPause = false;
        public bool isAI = false;
        private CharacterBody3D characterBody3D = null;
        public enum InputUpDown
        {
            None,
            Up,
            Down
        };
        public InputUpDown inputUpDown = InputUpDown.None;
        public enum InputLeftRight
        {
            None,
            Left,
            Right
        };
        public InputLeftRight inputLeftRight = InputLeftRight.None;
        private Vector3 _targetVelocity = Vector3.Zero;

        public enum InputShoot
        {
            None,
            LoadPower,
            Shoot,
            Finish
        };
        public InputShoot inputShoot = InputShoot.None;
        public List<Character> iceHockeyTeam1 = null;
        public List<Character> iceHockeyTeam2 = null;
        public RigidBody3D puck = null;
        public IceHockeyGoal Goal1 = null;
        public IceHockeyGoal Goal2 = null;
        public bool isSelected = false;
        public bool isPuckControl = false;
        private string currentAnimation = "";
        private int shootPosition = 0;
        private float shootPower = 0.0f;
        #endregion
        #region Constant
        private float speed = 3.0f;
        private float angle = 0.0f;
        private const float speedInc = 0.01f;
        private const float maxSpeed = 5.0f;
        private const float shootPowerMax = 74.0f;
        private const float shootPowerInc = 2.0f;
        private const float shootPowerMinAvg = 27.0f;
        private const float shootPowerMaxAvg = 47.0f;
        IDictionary<int, string> animName = new Dictionary<int, string>()
        {
            {1,"IceHockey_Idle"},
            {2,"IceHockey_Run_Puck"}
        };
        
        private const float iceHockeyMaxPower = 2.5f;

        private Node3D objRef = null;
        #endregion
        #region Implements
        public void PlayerInput(AnimationPlayer animationPlayer, double delta = 0.0f, int positionID = 0,
            CrossCountryOvertake crossCountryOvertakeM = null, CrossCountryOvertake crossCountryOvertakeRR = null, CrossCountryOvertake crossCountryOvertakeRL = null)
        {
            if (!isPause)
            {
                if (!isAI)
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
                                if (inputShoot == InputShoot.None)
                                {
                                    inputShoot = InputShoot.LoadPower;
                                }
                                if (inputShoot == InputShoot.LoadPower)
                                {
                                    if (shootPower < shootPowerMax)
                                    {
                                        shootPower += shootPowerInc;
                                        this.iceHockeyTeam1[3].hockeyPowerControl.Size = new Vector2(shootPower, 18.0f);//<-
                                    }                                                                        
                                }

                            }
                            if (!Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[5].keyId) && inputShoot == InputShoot.LoadPower)//Button 1
                            {                                
                                GoalShot();//<-                                
                            }
                            if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[6].keyId))//Button 2
                            {
                            
                            }
                            if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[1].keyId))//Up
                            {
                                inputUpDown = InputUpDown.Up;
                            }
                            else if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[2].keyId))//Down
                            {
                                inputUpDown = InputUpDown.Down;
                            }
                            else
                            {
                                inputUpDown = InputUpDown.None;
                            }
                            if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[3].keyId))//Left
                            {
                                inputLeftRight = InputLeftRight.Left;
                            }
                            else if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[4].keyId))//Right
                            {
                                inputLeftRight = InputLeftRight.Right;
                            }
                            else
                            {
                                inputLeftRight = InputLeftRight.None;
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
                                GoalShot();
                            }
                            else
                            {
                                
                            }
                            if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[6].keyId))//Button 2
                            {
                                
                            }
                            if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[1].keyId))//Up
                            {

                            }
                            else if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[2].keyId))//Down
                            {

                            }
                            else 
                            { 
                            
                            }
                            if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[3].keyId))//Left
                            {
                                
                            }
                            else if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[4].keyId))//Right
                            {
                                
                            }
                            else
                            {

                            }
                        }
                        //PlayAnimation(animationPlayer, index);
                    }
                    else
                    {                        
                        inputLeftRight = InputLeftRight.None;
                        inputUpDown = InputUpDown.None;
                        PlayAnimation(animationPlayer, 1);
                        GoalShotReset();
                    }
                    MovePlayer(animationPlayer);
                }
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
        public void PlayAnimationLoop(AnimationPlayer animationPlayer, int animID)
        {
            if (animationPlayer is not null)
            {
                if (animName.ContainsKey(animID))
                {                    
                    animationPlayer.Play(animName[animID]);
                    currentAnimation = animName[animID];
                }
            }
        }

        public void SetCharacterBody3D(CharacterBody3D characterBody3D)
        {            
            this.characterBody3D = characterBody3D;
        }
        public void SetPauseScreen(Control pauseScreen)
        { }
        public void SetFinishSessionScreen(Control finishSessionScreen)
        { }
        public void Pause()
        { }
        public void UnPause()
        { }
        public void ShowHideFinishSessionScreen()
        { }
        public void Init(List<List<CrossCountryModel>> crossCountryModelAIList = null, int initLine = 0)
        { }
        public void Reset()
        { }
        public float GetSpeed()
        {
            return 0.0f;
        }
        public float GetMaxSpeed()
        {
            return 0.0f;
        }
        public void SetRailSpeedSkating(int startPointId, List<SpeedSkatingTrackDTO> speedSkatingTrackDTOList, List<DirectionArrow> directionArrowList)
        { }
        public void SetRailBiathlon(int startPointId, List<List<SpeedSkatingTrackDTO>> speedSkatingTrackDTOList, List<List<DirectionArrow>> directionArrowList)
        { }
        public void SetCharacter(Character character)
        { }
        public bool GetPause()
        {
            return false;
        }
        public void SetSkiJumpPoint(int[] flyPoints)
        { }
        public void SetWindSkiJump(float angle, float power)
        { }
        public void SetSkiCollision(SkiCollision skiCollision)
        { }
        public float GetEnergy()
        {
            return 0.0f;
        }
        public bool GetIsFinished()
        {
            return false;
        }
        public bool GetIsAccel()
        {
            return false;
        }
        public bool GetIsBreak()
        {
            return false;
        }
        public void SetIsAI(bool isAI)
        { }
        public void SetCharacterIdCountry(int characterIdCountry)
        { }
        public int GetLinePosition()
        { return 0; }
        public bool GetIsAI()
        { return false; }
        public void OnlyPause()
        { }
        #endregion
        private void MovePlayer(AnimationPlayer animationPlayer)
        {
            var direction = Vector3.Zero;
            if (inputLeftRight == InputLeftRight.Right)
            {
                direction.X += 1.0f;
            }
            else if (inputLeftRight == InputLeftRight.Left)
            {
                direction.X -= 1.0f;
            }
            else
            {
                direction.X = 0.0f;
            }
            if (inputUpDown == InputUpDown.Down)
            {
                direction.Z += 1.0f;
            }
            else if (inputUpDown == InputUpDown.Up)
            {
                direction.Z -= 1.0f;
            }
            else
            {
                direction.Z = 0.0f;
            }
            if (direction != Vector3.Zero)
            {
                direction = direction.Normalized();
            }
            _targetVelocity.X = direction.X * speed;            
            _targetVelocity.Z = direction.Z * speed;            
            direction = _targetVelocity;
            this.characterBody3D.Velocity = direction;
            this.characterBody3D.MoveAndSlide();
            this.characterBody3D.Position = new Vector3(this.characterBody3D.Position.X, 0.45f, this.characterBody3D.Position.Z);
            if (direction != Vector3.Zero)
            {
                RotatePlayer();
                DefinePuckShootPosition();
                PlayAnimationLoop(animationPlayer, 2);                
            }
            MoveShootGraphic();
        }

        private void MoveShootGraphic()
        {
            Vector3 objectOfInterestPosition = this.iceHockeyTeam1[3].GlobalPosition;//<-
            Vector2 hudPosition = this.iceHockeyTeam1[3].parentNode.GetViewport().GetCamera3D().UnprojectPosition(objectOfInterestPosition);
            this.iceHockeyTeam1[3].hockeyPower.Position = new Vector2(hudPosition.X - 45.0f, hudPosition.Y - 90.0f);
        }

        private void RotatePlayer()
        {
            if (inputLeftRight == InputLeftRight.Right && inputUpDown == InputUpDown.Up)
            {
                angle = 135.0f;                
            }
            else if (inputLeftRight == InputLeftRight.Left && inputUpDown == InputUpDown.Up)
            {
                angle = 225.0f;
            }
            else if (inputLeftRight == InputLeftRight.Right && inputUpDown == InputUpDown.Down)
            {
                angle = 45.0f;
            }
            else if (inputLeftRight == InputLeftRight.Left && inputUpDown == InputUpDown.Down)
            {                
                angle = 315.0f;
            }
            else if (inputUpDown == InputUpDown.Up)
            {
                angle = 180.0f;
            }
            else if (inputUpDown == InputUpDown.Down)
            {
                angle = 0.0f;                
            }
            else if (inputLeftRight == InputLeftRight.Right)
            {
                angle = 90.0f;                
            }
            else if (inputLeftRight == InputLeftRight.Left)
            {
                angle = 270.0f;                
            }

            this.characterBody3D.Rotation = new Vector3(characterBody3D.Rotation.X, Mathf.DegToRad(angle), characterBody3D.Rotation.Z);
        }
        private void DefinePuckShootPosition()
        {
            if (inputLeftRight == InputLeftRight.Right && inputUpDown == InputUpDown.None)
            {
                shootPosition = 0;
            }
            else if (inputLeftRight == InputLeftRight.Left && inputUpDown == InputUpDown.None)
            {
                shootPosition = 2;
            }
            else if (inputLeftRight == InputLeftRight.Right && inputUpDown == InputUpDown.Up)
            {
                shootPosition = 1;            
            }
            else if (inputLeftRight == InputLeftRight.Left && inputUpDown == InputUpDown.Up)
            {
                shootPosition = 3;                
            }
            else if (inputLeftRight == InputLeftRight.Right && inputUpDown == InputUpDown.Down)
            {
                shootPosition = 0;                
            }
            else if (inputLeftRight == InputLeftRight.Left && inputUpDown == InputUpDown.Down)
            {
                shootPosition = 2;                
            }
            else if (inputLeftRight == InputLeftRight.None && inputUpDown == InputUpDown.None)
            {                
                Random rnd = new Random();
                var pos = rnd.Next(0, 4);
                shootPosition = pos;
            }
            else if (inputLeftRight == InputLeftRight.None && inputUpDown == InputUpDown.Up)
            {                
                Random rnd = new Random();
                var pos = rnd.Next(0, 2);
                shootPosition = pos == 0 ? 1 : 3;
            }
            else if (inputLeftRight == InputLeftRight.None && inputUpDown == InputUpDown.Down)
            {                
                Random rnd = new Random();
                var pos = rnd.Next(0, 2);
                shootPosition = pos == 0 ? 0 : 2;
            }            
        }

        public void SetIceHockeyGoals(IceHockeyGoal Goal1, IceHockeyGoal Goal2)
        { 
            this.Goal1 = Goal1;
            this.Goal2 = Goal2;
        }
        public void SetIceHockeyTeams(List<Character> iceHockeyTeam1, List<Character> iceHockeyTeam2) 
        {
            this.iceHockeyTeam1 = iceHockeyTeam1;
            this.iceHockeyTeam2 = iceHockeyTeam2;
        }
        public void SetPuck(RigidBody3D puck) 
        { 
            this.puck = puck;
        }
        public void GoalShot()
        {
            if (inputShoot == InputShoot.LoadPower && isPuckControl && isSelected)
            {     
                
                float angleRadians = new Vector2(Goal1.GetGoalPositionNode3d(shootPosition).GlobalPosition.X, Goal1.GetGoalPositionNode3d(shootPosition).GlobalPosition.Z)
                    .AngleToPoint(new Vector2(puck.GlobalPosition.X, puck.GlobalPosition.Z));
                float hypotenuse = 0.5f;
                float catetoOposto = hypotenuse * Mathf.Sin(angleRadians);
                float catetoAdjacente = hypotenuse * Mathf.Cos(angleRadians);

                float dist = new Vector2(Goal1.GetGoalPositionNode3d(shootPosition).GlobalPosition.X, Goal1.GetGoalPositionNode3d(shootPosition).GlobalPosition.Z)
                    .DistanceTo(new Vector2(puck.GlobalPosition.X, puck.GlobalPosition.Z));

                int xDir = 1;

                float shootPwr = 0.0f;
                if (shootPower > shootPowerMinAvg && shootPower < shootPowerMaxAvg)
                {
                    shootPwr = 0.0f;
                }
                if (shootPower <= shootPowerMinAvg)
                {
                    shootPwr = -1.0f;
                }
                if (shootPower >= shootPowerMaxAvg)
                {
                    shootPwr = 1.0f;
                }

                if (puck.GlobalPosition.X < Goal1.GetGoalPositionNode3d(shootPosition).GlobalPosition.X)
                    xDir = 1;
                if (puck.GlobalPosition.X > Goal1.GetGoalPositionNode3d(shootPosition).GlobalPosition.X)
                    xDir = -1;
                int zDir = 1;
                if (puck.GlobalPosition.Z < Goal1.GetGoalPositionNode3d(shootPosition).GlobalPosition.Z)
                    zDir = 1;
                if (puck.GlobalPosition.Z > Goal1.GetGoalPositionNode3d(shootPosition).GlobalPosition.Z)
                    zDir = -1;
                                
                float catetoOpostoHeight = Goal1.GetGoalPositionNode3d(shootPosition).GlobalPosition.Y;
                float catetoAdjacenteHeight = dist;
                double anguloRadianos = Math.Atan(catetoOpostoHeight / catetoAdjacenteHeight);
                double anguloGraus = anguloRadianos * (180.0 / Math.PI);                
                float catetoOpostoHeight2 = (float)((double)hypotenuse * Mathf.Sin(anguloRadianos));

                objRef.GlobalPosition = new Vector3(puck.GlobalPosition.X + ((xDir + shootPwr) * Mathf.Abs(catetoAdjacente)),                    
                    puck.GlobalPosition.Y,
                    puck.GlobalPosition.Z + (zDir * Mathf.Abs(catetoOposto)));

                var impulse = (objRef.GlobalPosition - puck.GlobalPosition);
                impulse = new Vector3(impulse.X, 
                    (shootPosition == 0 || shootPosition == 2 ? Goal1.GetGoalPositionNode3d(shootPosition).GlobalPosition.Y : catetoOpostoHeight2), impulse.Z);//<-

                inputShoot = InputShoot.Shoot;
                isPuckControl = false;
                isSelected = false;
                puck.ApplyImpulse(impulse);

                shootPower = 0.0f;
                this.iceHockeyTeam1[3].hockeyPowerControl.Size = new Vector2(0.0f, 18.0f);//<-
                this.iceHockeyTeam1[3].hockeyPower.Hide();                
            }
        }
        public void GoalShotReset()
        {
            if (inputShoot == InputShoot.Shoot)
            {
                inputShoot = InputShoot.None;                
            }
        }
        public void SetisSelected(ref bool isSelected) 
        { 
            this.isSelected = isSelected;
        }
        public void SetisPuckControl(ref bool isPuckControl) 
        {
            this.isPuckControl = isPuckControl;
        }
        public bool GetisSelected() { return this.isSelected; }
        public bool GetisPuckControl() { return this.isPuckControl; }
        public InputShoot GetinputShoot()
        {
            return inputShoot;
        }
        public void SetObj<T>(T obj)
        {
            objRef = obj as Node3D;
        }
    }
}
