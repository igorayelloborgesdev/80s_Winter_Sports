using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Interfaces;
using WinterSports.Scripts.Model;
using static Character;

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
        private float moveTimeRandom = 5.0f;
        private float moveTime = 0.0f;
        private float moveTimeInc = 0.01f;
        private float moveSpaceRandomMin = 1.0f;
        private float moveSpaceRandomMax = 2.5f;
        private int playerIndex = -1;
        private int playerNumberIndexPass = -1;
        private float moveSpaceRandomAIMin = 0.25f;
        private float moveSpaceRandomAIMax = 0.75f;
        public bool isCollidedWithTeamMate = false;
        public bool isMove = false;
        private float moveSpaceRandomShootAI = 2.0f;
        private int moveSpaceRandomPositionAI = 0;
        private float moveTimeOppononent = 0.0f;
        private float moveTimeOppononentLimit = 0.5f;
        public enum ChangePlayerEnum
        {
            None,
            Press,
            Finish
        };
        public ChangePlayerEnum changePlayerEnumSelect = ChangePlayerEnum.None;


        public enum PassPlayerEnum
        {
            None,
            Press,
            Finish
        };
        public PassPlayerEnum passPlayerEnum = PassPlayerEnum.None;

        public AnimationPlayer animationPlayer = null;

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
            {2,"IceHockey_Run_Puck"},
            {3,"IceHockey_GK_Idle"},
            {4,"IceHockey_GK_Left_Mid"},
            {5,"IceHockey_GK_Left_Up"},
            {6,"IceHockey_GK_Right_Mid"},
            {7,"IceHockey_GK_Right_Up"},
        };
        private const float iceHockeyMaxPower = 2.5f;
        private Node3D objRef = null;
        public int playerNumber = 0;
        public bool isPlayerTeam = false;
        private const float moveTimeMin = 5.0f;
        private const float moveTimeMax = 20.0f;
        private const float moveSpaceMin1 = 1.0f;
        private const float moveSpaceMin2 = 2.5f;
        private const float moveSpaceMax1 = 2.5f;
        private const float moveSpaceMax2 = 4.0f;
        private const float goalKeeperMoveSpaceMax = 0.1f;

        private const float moveSpaceMinAI1 = 0.25f;
        private const float moveSpaceMinAI2 = 0.5f;
        private const float moveSpaceMaxAI1 = 0.5f;
        private const float moveSpaceMaxAI2 = 0.75f;

        private const float moveSpaceRandomShootAIMin1 = 2.0f;        
        private const float moveSpaceRandomShootAIMax1 = 3.0f;
        #endregion
        #region Implements
        public void PlayerInput(AnimationPlayer animationPlayer, double delta = 0.0f, int positionID = 0,
            CrossCountryOvertake crossCountryOvertakeM = null, CrossCountryOvertake crossCountryOvertakeRR = null, CrossCountryOvertake crossCountryOvertakeRL = null)
        {
            this.animationPlayer = animationPlayer;
            if (!isPause)
            {
                DefineWhoIsControllingThePuck();
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
                            if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[5].keyId) && isPuckControl && isSelected)//Button 1
                            {
                                if (inputShoot == InputShoot.None)
                                {
                                    inputShoot = InputShoot.LoadPower;
                                }
                                if (inputShoot == InputShoot.LoadPower)
                                {
                                    if (shootPower < shootPowerMax && isPuckControl && this.iceHockeyTeam1[playerIndex].hockeyPowerControl is not null)
                                    {
                                        shootPower += shootPowerInc;
                                        this.iceHockeyTeam1[playerIndex].hockeyPowerControl.Size = new Vector2(shootPower, 18.0f);
                                    }
                                }

                            }
                            if (!Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[5].keyId) && inputShoot == InputShoot.LoadPower)//Button 1
                            {
                                if (this.iceHockeyTeam1.Any(x => x.isPuckControl))
                                    GoalShot();
                            }
                            if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[6].keyId))//Button 2
                            {
                                if (this.iceHockeyTeam1.Any(x => x.isPuckControl) && passPlayerEnum == PassPlayerEnum.None)
                                {
                                    passPlayerEnum = PassPlayerEnum.Press;
                                }
                                else if (!this.iceHockeyTeam1.Any(x => x.isPuckControl) && changePlayerEnumSelect == ChangePlayerEnum.None)
                                {
                                    changePlayerEnumSelect = ChangePlayerEnum.Press;                                    
                                }
                            }
                            if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[1].keyId))//Up
                            {
                                if (!this.iceHockeyTeam1[playerNumber].iceHockeyMoveLimit["down"])
                                {
                                    inputUpDown = InputUpDown.Up;
                                }
                                else
                                {
                                    inputUpDown = InputUpDown.None;
                                }
                            }
                            else if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[2].keyId))//Down
                            {
                                if (!this.iceHockeyTeam1[playerNumber].iceHockeyMoveLimit["up"])
                                {
                                    inputUpDown = InputUpDown.Down;
                                }
                                else
                                {
                                    inputUpDown = InputUpDown.None;
                                }
                            }
                            else
                            {
                                inputUpDown = InputUpDown.None;
                            }
                            if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[3].keyId))//Left
                            {
                                if (!this.iceHockeyTeam1[playerNumber].iceHockeyMoveLimit["right"])
                                {
                                    inputLeftRight = InputLeftRight.Left;
                                }
                                else
                                {
                                    inputLeftRight = InputLeftRight.None;
                                }

                            }
                            else if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[4].keyId))//Right
                            {
                                if (!this.iceHockeyTeam1[playerNumber].iceHockeyMoveLimit["left"])
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
                        if (!Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[5].keyId) && inputShoot == InputShoot.LoadPower)//Button 1
                        {
                            GoalShot();
                        }                        
                        inputLeftRight = InputLeftRight.None;
                        inputUpDown = InputUpDown.None;
                        PlayAnimation(animationPlayer, 1);
                        GoalShotReset();
                    }

                    if (!Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[6].keyId))//Button 2
                    {
                        if (!this.iceHockeyTeam1.Any(x => x.isPuckControl))
                            ChangePlayer();
                        else
                            PuckPass();
                    }

                    MovePlayer(animationPlayer, -1, iceHockeyTeam1);
                    ShowHideCursor();
                }
            }
        }

        private void ShowHideCursor()
        {
            if (this.iceHockeyTeam1.Where(x => x.isIceHockeySeletion).Count() > 1)
            {
                foreach (var obj in this.iceHockeyTeam1)
                {
                    if (!obj.isPuckControl)
                        obj.ShowHideIceHockeySeletion(false);
                }
            }
        }

        private void GetPlayerNextPuck()
        {            
            Vector3 posPuck = puck.GlobalPosition;

            var jogadorMaisProximo = this.iceHockeyTeam1
                .OrderBy(player => player.GlobalPosition.DistanceTo(posPuck))
                .FirstOrDefault();

            int indiceJogadorMaisProximo = this.iceHockeyTeam1.IndexOf(jogadorMaisProximo);

            if (jogadorMaisProximo != null)
            {
                foreach (var obj in this.iceHockeyTeam1)
                {
                    obj.isSelected = false;
                }

                playerIndex = indiceJogadorMaisProximo;

                if (!this.iceHockeyTeam2.Any(x => x.isPuckControl))
                {
                    this.iceHockeyTeam1[playerIndex].GetPlayerInput().ResetControlAndSelected();
                }
                else
                {
                    //this.iceHockeyTeam1[playerIndex].GetPlayerInput().ResetControlAndSelectedTeam1();                    
                }
                this.iceHockeyTeam1[playerIndex].isSelected = true;
                this.iceHockeyTeam1[playerIndex].GetPlayerInput().SetisSelected(ref this.iceHockeyTeam1[playerIndex].isSelected);
                this.iceHockeyTeam1[playerIndex].ShowHideIceHockeySeletion(true);
            }

        }

        public void PlayerInputAI(AnimationPlayer animationPlayer, double delta = 0.0f)
        {
            this.animationPlayer = animationPlayer;
            if (!isPause)
            {
                DefineWhoIsControllingThePuck();
                int playerNumberIndex = this.iceHockeyTeam1.FindIndex(x => x.playerNumber == this.playerNumber);

                if (playerNumberIndex != playerIndex && playerIndex != -1)
                {
                    int playerNumberIndexPuckControl = this.iceHockeyTeam1.FindIndex(x => x.isPuckControl);
                    if (playerNumberIndexPuckControl != -1)
                    {
                        float distanciaX = Mathf.Abs(this.iceHockeyTeam1[playerIndex].GlobalPosition.X - this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.X); // Calcula a distância no eixo X                    
                        float distanciaZ = Mathf.Abs(this.iceHockeyTeam1[playerIndex].GlobalPosition.Z - this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.Z); // Calcula a distância no eixo X                                                                                             

                        if (iceHockeyTeam1[playerNumberIndex].iceHockeyPosition == IceHockeyPosition.FW &&
                            iceHockeyTeam1[playerNumberIndex].iceHockeyPositionSide == IceHockeyPositionSide.L)
                        {

                            distanciaX = Mathf.Abs(this.iceHockeyTeam1.Where(x => x.iceHockeyPosition == IceHockeyPosition.MF).First().GlobalPosition.X - this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.X);
                            distanciaZ = Mathf.Abs(this.iceHockeyTeam1.Where(x => x.iceHockeyPosition == IceHockeyPosition.MF).First().GlobalPosition.Z - this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.Z);

                            if (distanciaX > moveSpaceRandomMax)
                            {
                                inputLeftRight = InputLeftRight.Right;
                            }
                            else if (distanciaX < moveSpaceRandomMin)
                            {
                                inputLeftRight = InputLeftRight.Left;
                            }
                            else
                            {
                                inputLeftRight = InputLeftRight.None;
                            }
                            if (distanciaZ > moveSpaceRandomMax)
                            {
                                inputUpDown = InputUpDown.Down;
                            }
                            else if (distanciaZ < moveSpaceRandomMin)
                            {
                                inputUpDown = InputUpDown.Up;
                            }
                            else
                            {
                                inputUpDown = InputUpDown.None;
                            }
                        }
                        else if (iceHockeyTeam1[playerNumberIndex].iceHockeyPosition == IceHockeyPosition.FW &&
                            iceHockeyTeam1[playerNumberIndex].iceHockeyPositionSide == IceHockeyPositionSide.R)
                        {

                            distanciaX = Mathf.Abs(this.iceHockeyTeam1.Where(x => x.iceHockeyPosition == IceHockeyPosition.MF).First().GlobalPosition.X - this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.X);
                            distanciaZ = Mathf.Abs(this.iceHockeyTeam1.Where(x => x.iceHockeyPosition == IceHockeyPosition.MF).First().GlobalPosition.Z - this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.Z);

                            if (distanciaX > moveSpaceRandomMax)
                            {
                                inputLeftRight = InputLeftRight.Left;
                            }
                            else if (distanciaX < moveSpaceRandomMin)
                            {
                                inputLeftRight = InputLeftRight.Right;
                            }
                            else
                            {
                                inputLeftRight = InputLeftRight.None;
                            }
                            if (distanciaZ > moveSpaceRandomMax)
                            {
                                inputUpDown = InputUpDown.Down;
                            }
                            else if (distanciaZ < moveSpaceRandomMin)
                            {
                                inputUpDown = InputUpDown.Up;
                            }
                            else
                            {
                                inputUpDown = InputUpDown.None;
                            }
                        }
                        else if (iceHockeyTeam1[playerNumberIndex].iceHockeyPosition == IceHockeyPosition.MF)
                        {
                            if (this.iceHockeyTeam1[playerNumberIndexPuckControl].iceHockeyPosition == IceHockeyPosition.FW &&
                                iceHockeyTeam1[playerNumberIndexPuckControl].iceHockeyPositionSide == IceHockeyPositionSide.L)
                            {
                                if (distanciaX > moveSpaceRandomMax)
                                {
                                    inputLeftRight = InputLeftRight.Left;
                                }
                                else if (distanciaX < moveSpaceRandomMin)
                                {
                                    inputLeftRight = InputLeftRight.Right;
                                }
                                else
                                {
                                    inputLeftRight = InputLeftRight.None;
                                }
                                if (distanciaZ > moveSpaceRandomMax)
                                {
                                    inputUpDown = InputUpDown.Up;
                                }
                                else if (distanciaZ < moveSpaceRandomMin)
                                {
                                    inputUpDown = InputUpDown.Down;
                                }
                                else
                                {
                                    inputUpDown = InputUpDown.None;
                                }
                            }
                            else if (this.iceHockeyTeam1[playerNumberIndexPuckControl].iceHockeyPosition == IceHockeyPosition.FW &&
                                iceHockeyTeam1[playerNumberIndexPuckControl].iceHockeyPositionSide == IceHockeyPositionSide.R)
                            {
                                if (distanciaX > moveSpaceRandomMax)
                                {
                                    inputLeftRight = InputLeftRight.Right;
                                }
                                else if (distanciaX < moveSpaceRandomMin)
                                {
                                    inputLeftRight = InputLeftRight.Left;
                                }
                                else
                                {
                                    inputLeftRight = InputLeftRight.None;
                                }
                                if (distanciaZ > moveSpaceRandomMax)
                                {
                                    inputUpDown = InputUpDown.Up;
                                }
                                else if (distanciaZ < moveSpaceRandomMin)
                                {
                                    inputUpDown = InputUpDown.Down;
                                }
                                else
                                {
                                    inputUpDown = InputUpDown.None;
                                }
                            }
                            else if (iceHockeyTeam1[playerNumberIndexPuckControl].iceHockeyPosition == IceHockeyPosition.DF &&
                                iceHockeyTeam1[playerNumberIndexPuckControl].iceHockeyPositionSide == IceHockeyPositionSide.L)
                            {
                                if (distanciaX > moveSpaceRandomMax)
                                {
                                    inputLeftRight = InputLeftRight.Left;
                                }
                                else if (distanciaX < moveSpaceRandomMin)
                                {
                                    inputLeftRight = InputLeftRight.Right;
                                }
                                else
                                {
                                    inputLeftRight = InputLeftRight.None;
                                }
                                if (distanciaZ > moveSpaceRandomMax)
                                {
                                    inputUpDown = InputUpDown.Down;
                                }
                                else if (distanciaZ < moveSpaceRandomMin)
                                {
                                    inputUpDown = InputUpDown.Up;
                                }
                                else
                                {
                                    inputUpDown = InputUpDown.None;
                                }
                            }
                            else if (iceHockeyTeam1[playerNumberIndexPuckControl].iceHockeyPosition == IceHockeyPosition.DF &&
                                iceHockeyTeam1[playerNumberIndexPuckControl].iceHockeyPositionSide == IceHockeyPositionSide.R)
                            {
                                if (distanciaX > moveSpaceRandomMax)
                                {
                                    inputLeftRight = InputLeftRight.Right;
                                }
                                else if (distanciaX < moveSpaceRandomMin)
                                {
                                    inputLeftRight = InputLeftRight.Left;
                                }
                                else
                                {
                                    inputLeftRight = InputLeftRight.None;
                                }
                                if (distanciaZ > moveSpaceRandomMax)
                                {
                                    inputUpDown = InputUpDown.Down;
                                }
                                else if (distanciaZ < moveSpaceRandomMin)
                                {
                                    inputUpDown = InputUpDown.Up;
                                }
                                else
                                {
                                    inputUpDown = InputUpDown.None;
                                }
                            }
                            else if (iceHockeyTeam1[playerNumberIndexPuckControl].iceHockeyPosition == IceHockeyPosition.GK)
                            {
                                if (distanciaX > moveSpaceRandomMax)
                                {
                                    inputLeftRight = InputLeftRight.Right;
                                }
                                else if (distanciaX < moveSpaceRandomMin)
                                {
                                    inputLeftRight = InputLeftRight.Left;
                                }
                                else
                                {
                                    inputLeftRight = InputLeftRight.None;
                                }
                                if (distanciaZ > moveSpaceRandomMax)
                                {
                                    inputUpDown = InputUpDown.Down;
                                }
                                else if (distanciaZ < moveSpaceRandomMin)
                                {
                                    inputUpDown = InputUpDown.Up;
                                }
                                else
                                {
                                    inputUpDown = InputUpDown.None;
                                }
                            }
                        }
                        else if (iceHockeyTeam1[playerNumberIndex].iceHockeyPosition == IceHockeyPosition.DF &&
                            iceHockeyTeam1[playerNumberIndex].iceHockeyPositionSide == IceHockeyPositionSide.L)
                        {
                            distanciaX = Mathf.Abs(this.iceHockeyTeam1.Where(x => x.iceHockeyPosition == IceHockeyPosition.MF).First().GlobalPosition.X - this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.X);
                            distanciaZ = Mathf.Abs(this.iceHockeyTeam1.Where(x => x.iceHockeyPosition == IceHockeyPosition.MF).First().GlobalPosition.Z - this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.Z);

                            if (distanciaX > moveSpaceRandomMax)
                            {
                                inputLeftRight = InputLeftRight.Right;
                            }
                            else if (distanciaX < moveSpaceRandomMin)
                            {
                                inputLeftRight = InputLeftRight.Left;
                            }
                            else
                            {
                                inputLeftRight = InputLeftRight.None;
                            }
                            if (distanciaZ > moveSpaceRandomMax)
                            {
                                inputUpDown = InputUpDown.Up;
                            }
                            else if (distanciaZ < moveSpaceRandomMin)
                            {
                                inputUpDown = InputUpDown.Down;
                            }
                            else
                            {
                                inputUpDown = InputUpDown.None;
                            }

                        }
                        else if (iceHockeyTeam1[playerNumberIndex].iceHockeyPosition == IceHockeyPosition.DF &&
                            iceHockeyTeam1[playerNumberIndex].iceHockeyPositionSide == IceHockeyPositionSide.R)
                        {

                            distanciaX = Mathf.Abs(this.iceHockeyTeam1.Where(x => x.iceHockeyPosition == IceHockeyPosition.MF).First().GlobalPosition.X - this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.X);
                            distanciaZ = Mathf.Abs(this.iceHockeyTeam1.Where(x => x.iceHockeyPosition == IceHockeyPosition.MF).First().GlobalPosition.Z - this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.Z);

                            if (distanciaX > moveSpaceRandomMax)
                            {
                                inputLeftRight = InputLeftRight.Left;
                            }
                            else if (distanciaX < moveSpaceRandomMin)
                            {
                                inputLeftRight = InputLeftRight.Right;
                            }
                            else
                            {
                                inputLeftRight = InputLeftRight.None;
                            }
                            if (distanciaZ > moveSpaceRandomMax)
                            {
                                inputUpDown = InputUpDown.Up;
                            }
                            else if (distanciaZ < moveSpaceRandomMin)
                            {
                                inputUpDown = InputUpDown.Down;
                            }
                            else
                            {
                                inputUpDown = InputUpDown.None;
                            }
                        }
                        else if (iceHockeyTeam1[playerNumberIndex].iceHockeyPosition == IceHockeyPosition.GK)
                        {
                            if (puck.GlobalPosition.X <= goalKeeperMoveSpaceMax &&
                                puck.GlobalPosition.X >= -1.0f * goalKeeperMoveSpaceMax)
                            {
                                iceHockeyTeam1[playerNumberIndex].GlobalPosition =
                                    new Vector3(puck.GlobalPosition.X, iceHockeyTeam1[playerNumberIndex].GlobalPosition.Y, iceHockeyTeam1[playerNumberIndex].GlobalPosition.Z);
                            }
                            else
                            {
                                if (iceHockeyTeam1[playerNumberIndex].GlobalPosition.X < puck.GlobalPosition.X)
                                {
                                    iceHockeyTeam1[playerNumberIndex].GlobalPosition =
                                    new Vector3(goalKeeperMoveSpaceMax, iceHockeyTeam1[playerNumberIndex].GlobalPosition.Y, iceHockeyTeam1[playerNumberIndex].GlobalPosition.Z);
                                }
                                else if (iceHockeyTeam1[playerNumberIndex].GlobalPosition.X > puck.GlobalPosition.X)
                                {
                                    iceHockeyTeam1[playerNumberIndex].GlobalPosition =
                                    new Vector3(-goalKeeperMoveSpaceMax, iceHockeyTeam1[playerNumberIndex].GlobalPosition.Y, iceHockeyTeam1[playerNumberIndex].GlobalPosition.Z);
                                }
                            }//<-
                            PlayAnimationLoop(animationPlayer, 3);
                        }

                        if (this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.Z > 9.25f || this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.Z < -9.25f)
                        {

                            if (this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.Z > 9.5f)
                            {
                                inputUpDown = InputUpDown.Up;
                            }
                            else if (this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.Z < -9.5f)
                            {
                                inputUpDown = InputUpDown.Down;
                            }
                            else if (this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.Z > 9.25f)
                            {
                                this.iceHockeyTeam1[playerNumberIndex].GlobalPosition = new Vector3(
                                    this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.X,
                                    this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.Y,
                                    9.25f);
                            }
                            else if (this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.Z < -9.25f)
                            {
                                this.iceHockeyTeam1[playerNumberIndex].GlobalPosition = new Vector3(
                                    this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.X,
                                    this.iceHockeyTeam1[playerNumberIndex].GlobalPosition.Y,
                                    -9.25f);
                            }
                        }
                        MovePlayer(animationPlayer, playerNumberIndex, iceHockeyTeam1);

                        moveTime += moveTimeInc;

                        if (moveTime > moveTimeRandom)
                        {
                            GenerateRandomMove();
                            GenerateRandomSpace();
                        }
                    }
                    else
                    {
                        if (!this.iceHockeyTeam1.Any(x => x.isSelected))
                        {
                            GetPlayerNextPuck();
                        }                        
                    }

                }
            }
        }

        public void PlayerInputAIOpponent(AnimationPlayer animationPlayer, double delta = 0.0f)
        {
            if (iceHockeyTeam2[playerNumber].iceHockeyPosition == IceHockeyPosition.GK)
            {
                if (puck.GlobalPosition.X <= goalKeeperMoveSpaceMax &&
                           puck.GlobalPosition.X >= -1.0f * goalKeeperMoveSpaceMax)
                {
                    iceHockeyTeam2[playerNumber].GlobalPosition =
                        new Vector3(puck.GlobalPosition.X, iceHockeyTeam2[playerNumber].GlobalPosition.Y, iceHockeyTeam2[playerNumber].GlobalPosition.Z);
                }
                else
                {
                    if (iceHockeyTeam2[playerNumber].GlobalPosition.X < puck.GlobalPosition.X)
                    {
                        iceHockeyTeam2[playerNumber].GlobalPosition =
                        new Vector3(goalKeeperMoveSpaceMax, iceHockeyTeam2[playerNumber].GlobalPosition.Y, iceHockeyTeam2[playerNumber].GlobalPosition.Z);
                    }
                    else if (iceHockeyTeam2[playerNumber].GlobalPosition.X > puck.GlobalPosition.X)
                    {
                        iceHockeyTeam2[playerNumber].GlobalPosition =
                        new Vector3(-goalKeeperMoveSpaceMax, iceHockeyTeam2[playerNumber].GlobalPosition.Y, iceHockeyTeam2[playerNumber].GlobalPosition.Z);
                    }
                }//<-
                PlayAnimationLoop(animationPlayer, 3);
            }
            else if (iceHockeyTeam2[playerNumber].iceHockeyPosition == IceHockeyPosition.DF)
            {
                if (!iceHockeyTeam2[playerNumber].iceHockeyPlayerCollision.GetIsCollided)
                {
                    if (!iceHockeyTeam1.Any(x => x.isPuckControl) && !iceHockeyTeam2.Any(x => x.isPuckControl))
                    {
                        if (iceHockeyTeam2[playerNumber].GlobalPosition.X < puck.GlobalPosition.X)
                        {
                            inputLeftRight = InputLeftRight.Right;
                        }
                        else if (iceHockeyTeam2[playerNumber].GlobalPosition.X > puck.GlobalPosition.X)
                        {
                            inputLeftRight = InputLeftRight.Left;
                        }
                        else
                        {
                            inputLeftRight = InputLeftRight.None;
                        }
                        if (iceHockeyTeam2[playerNumber].GlobalPosition.Z < puck.GlobalPosition.Z)
                        {
                            inputUpDown = InputUpDown.Down;
                        }
                        else if (iceHockeyTeam2[playerNumber].GlobalPosition.Z > puck.GlobalPosition.Z)
                        {
                            inputUpDown = InputUpDown.Up;
                        }
                        else
                        {
                            inputUpDown = InputUpDown.None;
                        }
                    }
                    else if (iceHockeyTeam1.Any(x => x.isPuckControl))//<-
                    {
                        if (iceHockeyTeam2[playerNumber].iceHockeyPositionSide == IceHockeyPositionSide.L && iceHockeyTeam2[playerNumber].iceHockeyPosition == IceHockeyPosition.DF)
                        {
                            var playericeHockeyTeam1 = this.iceHockeyTeam1.Where(x => x.iceHockeyPosition == IceHockeyPosition.FW
                                                            && x.iceHockeyPositionSide == IceHockeyPositionSide.R).First();

                            float distanciaX = Mathf.Abs(iceHockeyTeam2[playerNumber].GlobalPosition.X - playericeHockeyTeam1.GlobalPosition.X);
                            float distanciaZ = Mathf.Abs(iceHockeyTeam2[playerNumber].GlobalPosition.Z - playericeHockeyTeam1.GlobalPosition.Z);

                            if (iceHockeyTeam2[playerNumber].GlobalPosition.X < playericeHockeyTeam1.GlobalPosition.X && distanciaX > moveSpaceRandomAIMax)
                            {
                                inputLeftRight = InputLeftRight.Right;
                            }
                            else if (iceHockeyTeam2[playerNumber].GlobalPosition.X > playericeHockeyTeam1.GlobalPosition.X && distanciaX > moveSpaceRandomAIMax)
                            {
                                inputLeftRight = InputLeftRight.Left;
                            }
                            else
                            {
                                inputLeftRight = InputLeftRight.None;
                            }
                            if (iceHockeyTeam2[playerNumber].GlobalPosition.Z < playericeHockeyTeam1.GlobalPosition.Z && distanciaZ > moveSpaceRandomAIMax)
                            {
                                inputUpDown = InputUpDown.Down;
                            }
                            else if (iceHockeyTeam2[playerNumber].GlobalPosition.Z > playericeHockeyTeam1.GlobalPosition.Z && distanciaZ > moveSpaceRandomAIMax)
                            {
                                inputUpDown = InputUpDown.Up;
                            }
                            else
                            {
                                inputUpDown = InputUpDown.None;
                            }
                        }
                        else if (iceHockeyTeam2[playerNumber].iceHockeyPositionSide == IceHockeyPositionSide.R && iceHockeyTeam2[playerNumber].iceHockeyPosition == IceHockeyPosition.DF)
                        {
                            var playericeHockeyTeam1 = this.iceHockeyTeam1.Where(x => x.iceHockeyPosition == IceHockeyPosition.FW
                                                            && x.iceHockeyPositionSide == IceHockeyPositionSide.L).First();

                            float distanciaX = Mathf.Abs(iceHockeyTeam2[playerNumber].GlobalPosition.X - playericeHockeyTeam1.GlobalPosition.X);
                            float distanciaZ = Mathf.Abs(iceHockeyTeam2[playerNumber].GlobalPosition.Z - playericeHockeyTeam1.GlobalPosition.Z);

                            if (iceHockeyTeam2[playerNumber].GlobalPosition.X < playericeHockeyTeam1.GlobalPosition.X && distanciaX > moveSpaceRandomAIMax)
                            {
                                inputLeftRight = InputLeftRight.Right;
                            }
                            else if (iceHockeyTeam2[playerNumber].GlobalPosition.X > playericeHockeyTeam1.GlobalPosition.X && distanciaX > moveSpaceRandomAIMax)
                            {
                                inputLeftRight = InputLeftRight.Left;
                            }
                            else
                            {
                                inputLeftRight = InputLeftRight.None;
                            }
                            if (iceHockeyTeam2[playerNumber].GlobalPosition.Z < playericeHockeyTeam1.GlobalPosition.Z && distanciaZ > moveSpaceRandomAIMax)
                            {
                                inputUpDown = InputUpDown.Down;
                            }
                            else if (iceHockeyTeam2[playerNumber].GlobalPosition.Z > playericeHockeyTeam1.GlobalPosition.Z && distanciaZ > moveSpaceRandomAIMax)
                            {
                                inputUpDown = InputUpDown.Up;
                            }
                            else
                            {
                                inputUpDown = InputUpDown.None;
                            }
                        }
                    }
                    else if (iceHockeyTeam2.Any(x => x.isPuckControl))
                    {
                        if (iceHockeyTeam2[playerNumber].iceHockeyPositionSide == IceHockeyPositionSide.L)
                        {
                            if (moveSpaceRandomPositionAI == 0)
                            {
                                if (iceHockeyTeam2[playerNumber].GlobalPosition.X < -0.5f)
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
                                if (iceHockeyTeam2[playerNumber].GlobalPosition.X > -2.0f)
                                {
                                    inputLeftRight = InputLeftRight.Left;
                                }
                                else
                                {
                                    inputLeftRight = InputLeftRight.None;
                                }
                            }
                        }
                        else
                        {
                            if (moveSpaceRandomPositionAI == 0)
                            {
                                if (iceHockeyTeam2[playerNumber].GlobalPosition.X > 0.5f)
                                {
                                    inputLeftRight = InputLeftRight.Left;
                                }
                                else
                                {
                                    inputLeftRight = InputLeftRight.None;
                                }
                            }
                            else
                            {
                                if (iceHockeyTeam2[playerNumber].GlobalPosition.X < 2.0f)
                                {
                                    inputLeftRight = InputLeftRight.Right;
                                }
                                else
                                {
                                    inputLeftRight = InputLeftRight.None;
                                }
                            }
                        }
                    }
                }
                
                
            }
            else if (iceHockeyTeam2[playerNumber].iceHockeyPosition == IceHockeyPosition.MF)
            {
                if (!iceHockeyTeam2[playerNumber].iceHockeyPlayerCollision.GetIsCollided)
                {
                    if (!iceHockeyTeam1.Any(x => x.isPuckControl) && !iceHockeyTeam2.Any(x => x.isPuckControl))
                    {
                        if (iceHockeyTeam2[playerNumber].GlobalPosition.X < puck.GlobalPosition.X)
                        {
                            inputLeftRight = InputLeftRight.Right;
                        }
                        else if (iceHockeyTeam2[playerNumber].GlobalPosition.X > puck.GlobalPosition.X)
                        {
                            inputLeftRight = InputLeftRight.Left;
                        }
                        else
                        {
                            inputLeftRight = InputLeftRight.None;
                        }
                        if (iceHockeyTeam2[playerNumber].GlobalPosition.Z < puck.GlobalPosition.Z)
                        {
                            inputUpDown = InputUpDown.Down;
                        }
                        else if (iceHockeyTeam2[playerNumber].GlobalPosition.Z > puck.GlobalPosition.Z)
                        {
                            inputUpDown = InputUpDown.Up;
                        }
                        else
                        {
                            inputUpDown = InputUpDown.None;
                        }
                    }
                    else if (iceHockeyTeam1.Any(x => x.isPuckControl))
                    {
                        var playericeHockeyTeam1 = this.iceHockeyTeam1.Where(x => x.iceHockeyPosition == IceHockeyPosition.MF).First();

                        float distanciaX = Mathf.Abs(iceHockeyTeam2[playerNumber].GlobalPosition.X - playericeHockeyTeam1.GlobalPosition.X);
                        float distanciaZ = Mathf.Abs(iceHockeyTeam2[playerNumber].GlobalPosition.Z - playericeHockeyTeam1.GlobalPosition.Z);
                        if (iceHockeyTeam2[playerNumber].GlobalPosition.X < playericeHockeyTeam1.GlobalPosition.X && distanciaX > moveSpaceRandomAIMax)
                        {
                            inputLeftRight = InputLeftRight.Right;
                        }
                        else if (iceHockeyTeam2[playerNumber].GlobalPosition.X > playericeHockeyTeam1.GlobalPosition.X && distanciaX > moveSpaceRandomAIMax)
                        {
                            inputLeftRight = InputLeftRight.Left;
                        }
                        else
                        {
                            inputLeftRight = InputLeftRight.None;
                        }
                        if (iceHockeyTeam2[playerNumber].GlobalPosition.Z < playericeHockeyTeam1.GlobalPosition.Z && distanciaZ > moveSpaceRandomAIMax)
                        {
                            inputUpDown = InputUpDown.Down;
                        }
                        else if (iceHockeyTeam2[playerNumber].GlobalPosition.Z > playericeHockeyTeam1.GlobalPosition.Z && distanciaZ > moveSpaceRandomAIMax)
                        {
                            inputUpDown = InputUpDown.Up;
                        }
                        else
                        {
                            inputUpDown = InputUpDown.None;
                        }
                    }
                    else if (iceHockeyTeam2.Any(x => x.isPuckControl))
                    {
                        if (iceHockeyTeam2.Where(x => x.isPuckControl).First().playerNumber == playerNumber)
                        {
                            float distanciaZ = Mathf.Abs(iceHockeyTeam2[playerNumber].GlobalPosition.Z - ((this.Goal2.GetGoalPosition(0).Z + this.Goal2.GetGoalPosition(2).Z) / 2.0f));
                            if (distanciaZ > moveSpaceRandomShootAI)
                            {
                                inputUpDown = InputUpDown.Down;
                                if (moveSpaceRandomPositionAI == 0)
                                {
                                    if (iceHockeyTeam2[playerNumber].GlobalPosition.X > -0.5f)//<-
                                    {
                                        inputLeftRight = InputLeftRight.Left;
                                    }
                                    else
                                    {
                                        inputLeftRight = InputLeftRight.None;
                                    }
                                }
                                else
                                {
                                    if (iceHockeyTeam2[playerNumber].GlobalPosition.X < 0.5f)//<-
                                    {
                                        inputLeftRight = InputLeftRight.Right;
                                    }
                                    else
                                    {
                                        inputLeftRight = InputLeftRight.None;
                                    }
                                }

                            }
                            else
                            {
                                GoalShotAI();
                                inputUpDown = InputUpDown.None;
                            }
                        }
                        else
                        {
                            //GD.Print("C");
                        }                                                

                    }
                }

                    
            }
            else if (iceHockeyTeam2[playerNumber].iceHockeyPosition == IceHockeyPosition.FW)
            {
                if (!iceHockeyTeam2[playerNumber].iceHockeyPlayerCollision.GetIsCollided)
                {
                    if (!iceHockeyTeam1.Any(x => x.isPuckControl) && !iceHockeyTeam2.Any(x => x.isPuckControl))
                    {
                        if (iceHockeyTeam2[playerNumber].GlobalPosition.X < puck.GlobalPosition.X)
                        {
                            inputLeftRight = InputLeftRight.Right;
                        }
                        else if (iceHockeyTeam2[playerNumber].GlobalPosition.X > puck.GlobalPosition.X)
                        {
                            inputLeftRight = InputLeftRight.Left;
                        }
                        else
                        {
                            inputLeftRight = InputLeftRight.None;
                        }
                        if (iceHockeyTeam2[playerNumber].GlobalPosition.Z < puck.GlobalPosition.Z)
                        {
                            inputUpDown = InputUpDown.Down;
                        }
                        else if (iceHockeyTeam2[playerNumber].GlobalPosition.Z > puck.GlobalPosition.Z)
                        {
                            inputUpDown = InputUpDown.Up;
                        }
                        else
                        {
                            inputUpDown = InputUpDown.None;
                        }
                    }
                    else if (iceHockeyTeam1.Any(x => x.isPuckControl))
                    {
                        if (iceHockeyTeam2[playerNumber].iceHockeyPositionSide == IceHockeyPositionSide.L && iceHockeyTeam2[playerNumber].iceHockeyPosition == IceHockeyPosition.FW)
                        {
                            var playericeHockeyTeam1 = this.iceHockeyTeam1.Where(x => x.iceHockeyPosition == IceHockeyPosition.DF
                                                            && x.iceHockeyPositionSide == IceHockeyPositionSide.R).First();

                            float distanciaX = Mathf.Abs(iceHockeyTeam2[playerNumber].GlobalPosition.X - playericeHockeyTeam1.GlobalPosition.X);
                            float distanciaZ = Mathf.Abs(iceHockeyTeam2[playerNumber].GlobalPosition.Z - playericeHockeyTeam1.GlobalPosition.Z);

                            if (iceHockeyTeam2[playerNumber].GlobalPosition.X < playericeHockeyTeam1.GlobalPosition.X && distanciaX > moveSpaceRandomAIMax)
                            {
                                inputLeftRight = InputLeftRight.Right;
                            }
                            else if (iceHockeyTeam2[playerNumber].GlobalPosition.X > playericeHockeyTeam1.GlobalPosition.X && distanciaX > moveSpaceRandomAIMax)
                            {
                                inputLeftRight = InputLeftRight.Left;
                            }
                            else
                            {
                                inputLeftRight = InputLeftRight.None;
                            }
                            if (iceHockeyTeam2[playerNumber].GlobalPosition.Z < playericeHockeyTeam1.GlobalPosition.Z && distanciaZ > moveSpaceRandomAIMax)
                            {
                                inputUpDown = InputUpDown.Down;
                            }
                            else if (iceHockeyTeam2[playerNumber].GlobalPosition.Z > playericeHockeyTeam1.GlobalPosition.Z && distanciaZ > moveSpaceRandomAIMax)
                            {
                                inputUpDown = InputUpDown.Up;
                            }
                            else
                            {
                                inputUpDown = InputUpDown.None;
                            }
                        }
                        else if (iceHockeyTeam2[playerNumber].iceHockeyPositionSide == IceHockeyPositionSide.R && iceHockeyTeam2[playerNumber].iceHockeyPosition == IceHockeyPosition.FW)
                        {
                            var playericeHockeyTeam1 = this.iceHockeyTeam1.Where(x => x.iceHockeyPosition == IceHockeyPosition.DF
                                                            && x.iceHockeyPositionSide == IceHockeyPositionSide.L).First();

                            float distanciaX = Mathf.Abs(iceHockeyTeam2[playerNumber].GlobalPosition.X - playericeHockeyTeam1.GlobalPosition.X);
                            float distanciaZ = Mathf.Abs(iceHockeyTeam2[playerNumber].GlobalPosition.Z - playericeHockeyTeam1.GlobalPosition.Z);

                            if (iceHockeyTeam2[playerNumber].GlobalPosition.X < playericeHockeyTeam1.GlobalPosition.X && distanciaX > moveSpaceRandomAIMax)
                            {
                                inputLeftRight = InputLeftRight.Right;
                            }
                            else if (iceHockeyTeam2[playerNumber].GlobalPosition.X > playericeHockeyTeam1.GlobalPosition.X && distanciaX > moveSpaceRandomAIMax)
                            {
                                inputLeftRight = InputLeftRight.Left;
                            }
                            else
                            {
                                inputLeftRight = InputLeftRight.None;
                            }
                            if (iceHockeyTeam2[playerNumber].GlobalPosition.Z < playericeHockeyTeam1.GlobalPosition.Z && distanciaZ > moveSpaceRandomAIMax)
                            {
                                inputUpDown = InputUpDown.Down;
                            }
                            else if (iceHockeyTeam2[playerNumber].GlobalPosition.Z > playericeHockeyTeam1.GlobalPosition.Z && distanciaZ > moveSpaceRandomAIMax)
                            {
                                inputUpDown = InputUpDown.Up;
                            }
                            else
                            {
                                inputUpDown = InputUpDown.None;
                            }
                        }
                    }
                    else if (iceHockeyTeam2.Any(x => x.isPuckControl))
                    {
                        if (iceHockeyTeam2.Where(x => x.isPuckControl).First().playerNumber == playerNumber)
                        {
                            float distanciaZ = Mathf.Abs(iceHockeyTeam2[playerNumber].GlobalPosition.Z - ((this.Goal2.GetGoalPosition(0).Z + this.Goal2.GetGoalPosition(2).Z) / 2.0f));
                            if (distanciaZ > moveSpaceRandomShootAI)
                            {
                                inputUpDown = InputUpDown.Down;

                                if (iceHockeyTeam2[playerNumber].iceHockeyPositionSide == IceHockeyPositionSide.L)
                                {
                                    if (moveSpaceRandomPositionAI == 0)
                                    {
                                        if (iceHockeyTeam2[playerNumber].GlobalPosition.X < -0.5f)
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
                                        if (iceHockeyTeam2[playerNumber].GlobalPosition.X > -2.0f)
                                        {
                                            inputLeftRight = InputLeftRight.Left;
                                        }
                                        else
                                        {
                                            inputLeftRight = InputLeftRight.None;
                                        }
                                    }
                                }
                                else
                                {
                                    if (moveSpaceRandomPositionAI == 0)
                                    {
                                        if (iceHockeyTeam2[playerNumber].GlobalPosition.X > 0.5f)
                                        {
                                            inputLeftRight = InputLeftRight.Left;
                                        }
                                        else
                                        {
                                            inputLeftRight = InputLeftRight.None;
                                        }
                                    }
                                    else
                                    {
                                        if (iceHockeyTeam2[playerNumber].GlobalPosition.X < 2.0f)
                                        {
                                            inputLeftRight = InputLeftRight.Right;
                                        }
                                        else
                                        {
                                            inputLeftRight = InputLeftRight.None;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                GoalShotAI();
                                inputUpDown = InputUpDown.None;
                            }
                        }
                        else
                        {
                            //GD.Print("C");
                        }
                    }
                }

            }

            moveTimeOppononent += moveTimeInc;
            if (moveTimeOppononent > moveTimeOppononentLimit)
            {
                moveTimeOppononent = 0.0f;
                if (iceHockeyTeam2[playerNumber].iceHockeyPlayerCollision.GetIsCollided)
                {

                    var noBlockingPlayers = DefineIfOpponentIsBlocking();
                    if (noBlockingPlayers.Count > 0 && iceHockeyTeam2.Any(x => x.isPuckControl))
                    {
                        var isMove = GenerateRandomBinary() == 0;
                        if (isMove)
                            MoveFromOpponent();
                        else
                            PuckPassAI(noBlockingPlayers.Last<int>());
                        //GD.Print(noBlockingPlayers.Last<int>());//<-
                    }
                    else
                    {
                        MoveFromOpponent();
                    }                                       
                }
            }

            moveTime += moveTimeInc;
            if (moveTime > moveTimeRandom)
            {
                GenerateRandomMove();
                GenerateRandomSpaceAI();
                GenerateRandomShootAI();
                GenerateRandomAttackPositionAI();
            }
            PlayAnimationLoop(animationPlayer, 2);
            MovePlayer(animationPlayer, playerNumber, iceHockeyTeam2);
        }
        private void MoveFromOpponent()
        {
            if ((int)iceHockeyTeam2[playerNumber].GlobalRotationDegrees.Y == 0)
            {
                inputUpDown = InputUpDown.None;
                inputLeftRight = GenerateRandomBinary() == 0 ? InputLeftRight.Right : InputLeftRight.Left;
            }
            else if ((int)iceHockeyTeam2[playerNumber].GlobalRotationDegrees.Y == 45)
            {
                inputUpDown = InputUpDown.Down;
                inputLeftRight = InputLeftRight.Left;
            }
            else if ((int)iceHockeyTeam2[playerNumber].GlobalRotationDegrees.Y == -45)
            {
                inputUpDown = InputUpDown.Down;
                inputLeftRight = InputLeftRight.Right;
            }
            else if ((int)iceHockeyTeam2[playerNumber].GlobalRotationDegrees.Y == 90)
            {
                inputUpDown = GenerateRandomBinary() == 0 ? InputUpDown.Down : InputUpDown.Up;
                inputLeftRight = InputLeftRight.None;
            }
            else if ((int)iceHockeyTeam2[playerNumber].GlobalRotationDegrees.Y == -90)
            {
                inputUpDown = GenerateRandomBinary() == 0 ? InputUpDown.Down : InputUpDown.Up;
                inputLeftRight = InputLeftRight.None;
            }
            else if ((int)iceHockeyTeam2[playerNumber].GlobalRotationDegrees.Y == -180)
            {
                inputUpDown = InputUpDown.None;
                inputLeftRight = GenerateRandomBinary() == 0 ? InputLeftRight.Right : InputLeftRight.Left;
            }
            else if ((int)iceHockeyTeam2[playerNumber].GlobalRotationDegrees.Y == -135)
            {
                inputUpDown = InputUpDown.Up;
                inputLeftRight = InputLeftRight.Right;
            }
            else if ((int)iceHockeyTeam2[playerNumber].GlobalRotationDegrees.Y == 135)
            {
                inputUpDown = InputUpDown.Up;
                inputLeftRight = InputLeftRight.Left;
            }
        }

        private List<int> DefineIfOpponentIsBlocking()
        {
            List<int> listNoBlocked = new List<int>();
            foreach (var obj in iceHockeyTeam2) 
            {
                if (obj != iceHockeyTeam2[playerNumber] && obj.iceHockeyPosition != IceHockeyPosition.GK)
                {
                    float angleRadians = new Vector2(
                                                obj.GlobalPosition.X,
                                                obj.GlobalPosition.Z
                                            ).AngleToPoint(new Vector2(
                                                iceHockeyTeam2[playerNumber].GlobalPosition.X,
                                                iceHockeyTeam2[playerNumber].GlobalPosition.Z
                                            ));

                    float hypotenuse = new Vector2(
                                                obj.GlobalPosition.X,
                                                obj.GlobalPosition.Z
                                            ).DistanceTo(new Vector2(
                                                iceHockeyTeam2[playerNumber].GlobalPosition.X,
                                                iceHockeyTeam2[playerNumber].GlobalPosition.Z
                                            )); ;

                    foreach (var objOp in iceHockeyTeam1)
                    {
                        for (float i = 0.0f; i < hypotenuse; i += 0.7f)
                        {
                            float catetoAdjacente = i * Mathf.Cos(angleRadians);
                            float catetoOposto = i * Mathf.Sin(angleRadians);

                            int xDir = 1;
                            if (obj.GlobalPosition.X < iceHockeyTeam2[playerNumber].GlobalPosition.X)
                                xDir = 1;
                            if (obj.GlobalPosition.X > iceHockeyTeam2[playerNumber].GlobalPosition.X)
                                xDir = -1;
                            int zDir = 1;
                            if (obj.GlobalPosition.Z < iceHockeyTeam2[playerNumber].GlobalPosition.Z)
                                zDir = 1;
                            if (obj.GlobalPosition.Z > iceHockeyTeam2[playerNumber].GlobalPosition.Z)
                                zDir = -1;

                            var pos = new Vector2(
                                                  obj.GlobalPosition.X + (xDir * Mathf.Abs(catetoAdjacente)),
                                                  obj.GlobalPosition.Z + (zDir * Mathf.Abs(catetoOposto))
                                                  );
                        
                            if (objOp.iceHockeyPosition != IceHockeyPosition.GK)
                            { 
                                var dist = new Vector2(objOp.GlobalPosition.X, objOp.GlobalPosition.Z).DistanceTo(pos);;

                                if (dist > 0.7f)
                                {
                                    if (!listNoBlocked.Any(x => x == obj.playerNumber))
                                        listNoBlocked.Add(obj.playerNumber);                                    
                                }
                                
                            }                            
                        }                        

                    }

                }
            }
            return listNoBlocked;
        }

        private void GoalShotAI()
        {
            Random rnd = new Random();
            var pos = rnd.Next(0, 4);            
            float angleRadians = new Vector2(Goal2.GetGoalPositionNode3d(pos).GlobalPosition.X, Goal2.GetGoalPositionNode3d(pos).GlobalPosition.Z)
             .AngleToPoint(new Vector2(puck.GlobalPosition.X, puck.GlobalPosition.Z));
            float hypotenuse = 0.5f;
            float catetoOposto = hypotenuse * Mathf.Sin(angleRadians);
            float catetoAdjacente = hypotenuse * Mathf.Cos(angleRadians);
            float catetoOpostoHeight = Goal2.GetGoalPositionNode3d(pos).GlobalPosition.Y;
            float dist = new Vector2(Goal2.GetGoalPositionNode3d(pos).GlobalPosition.X, Goal2.GetGoalPositionNode3d(pos).GlobalPosition.Z)
             .DistanceTo(new Vector2(puck.GlobalPosition.X, puck.GlobalPosition.Z));
            float catetoAdjacenteHeight = dist;
            double anguloRadianos = Math.Atan(catetoOpostoHeight / catetoAdjacenteHeight);
            float catetoOpostoHeight2 = (float)((double)hypotenuse * Mathf.Sin(anguloRadianos));
            float shootPwr = 0.0f;
            int xDir = 1;
            if (puck.GlobalPosition.X < Goal2.GetGoalPositionNode3d(pos).GlobalPosition.X)
                xDir = 1;
            if (puck.GlobalPosition.X > Goal2.GetGoalPositionNode3d(pos).GlobalPosition.X)
                xDir = -1;
            int zDir = 1;
            if (puck.GlobalPosition.Z < Goal2.GetGoalPositionNode3d(pos).GlobalPosition.Z)
                zDir = 1;
            if (puck.GlobalPosition.Z > Goal2.GetGoalPositionNode3d(pos).GlobalPosition.Z)
                zDir = -1;
            objRef.GlobalPosition = new Vector3(puck.GlobalPosition.X + ((xDir + shootPwr) * Mathf.Abs(catetoAdjacente)),
                puck.GlobalPosition.Y,
                puck.GlobalPosition.Z + (zDir * Mathf.Abs(catetoOposto)));

            var impulse = (objRef.GlobalPosition - puck.GlobalPosition);
            impulse = new Vector3(impulse.X,
                (pos == 0 || pos == 2 ? Goal2.GetGoalPositionNode3d(pos).GlobalPosition.Y : catetoOpostoHeight2), impulse.Z);
            puck.ApplyImpulse(impulse);
            
            this.iceHockeyTeam2[playerNumber].isPuckControl = false;
            this.iceHockeyTeam2[playerNumber].isSelected = false;
            this.iceHockeyTeam2[playerNumber].ShowHideIceHockeySeletion(false);
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
        private void MovePlayer(AnimationPlayer animationPlayer, int playerNumberIndex, List<Character> iceHockeyTeam)
        {                        
            var direction = Vector3.Zero;
            if (playerNumberIndex == -1)
            {
                if (playerNumber == 0 && isPuckControl)
                {
                    
                }
                else
                {
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
                }                                
            }
            else 
            {
                if (iceHockeyTeam[playerNumberIndex].iceHockeyPosition != IceHockeyPosition.GK)
                {
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
                }           
            }
            
            if (playerNumber != 0)
            {                
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
                if (playerNumber != 0)
                {
                    RotatePlayer();
                    PlayAnimationLoop(animationPlayer, 2);
                }                    
                DefinePuckShootPosition();                             
            }
            MoveShootGraphic(iceHockeyTeam);
        }

        private void MoveShootGraphic(List<Character> iceHockeyTeam)
        {
            try
            {                
                if (playerIndex != -1)
                {
                    Vector3 objectOfInterestPosition = iceHockeyTeam[playerIndex].GlobalPosition;
                    if (iceHockeyTeam[playerIndex].parentNode is not null)
                    {
                        Vector2 hudPosition = iceHockeyTeam[playerIndex].parentNode.GetViewport().GetCamera3D().UnprojectPosition(objectOfInterestPosition);
                        iceHockeyTeam[playerIndex].hockeyPower.Position = new Vector2(hudPosition.X - 45.0f, hudPosition.Y - 90.0f);
                    }                    
                }
            }
            catch (Exception ex) { }            
        }
        private void DefineWhoIsControllingThePuck()
        {
            
            if (iceHockeyTeam1.Where(x => x.isPuckControl).Any())
            {         
                playerIndex = iceHockeyTeam1.FindIndex(x => x.isPuckControl);                
                return;
            }
            if (iceHockeyTeam2.Where(x => x.isPuckControl).Any())
            {             
                playerIndex = iceHockeyTeam2.FindIndex(x => x.isPuckControl);                
                return;
            }
            if (!iceHockeyTeam1.Where(x => x.isPuckControl).Any() && !iceHockeyTeam2.Where(x => x.isPuckControl).Any())
            {                
                playerIndex = -1;                
                return;
            }
            
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
        public void SetIceHockeyTeams(ref List<Character> iceHockeyTeam1, ref List<Character> iceHockeyTeam2)
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

                int xDir = 1;
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
                    (shootPosition == 0 || shootPosition == 2 ? Goal1.GetGoalPositionNode3d(shootPosition).GlobalPosition.Y : catetoOpostoHeight2), impulse.Z);

                inputShoot = InputShoot.Shoot;
                isPuckControl = false;
                isSelected = false;
                this.iceHockeyTeam1[playerIndex].isPuckControl = false;
                this.iceHockeyTeam1[playerIndex].isSelected = false;

                this.iceHockeyTeam1[0].SetGoalKeeperShoot(this.animationPlayer, xDir, zDir);

                puck.ApplyImpulse(impulse);

                shootPower = 0.0f;
                this.iceHockeyTeam1[playerIndex].hockeyPowerControl.Size = new Vector2(0.0f, 18.0f);
                this.iceHockeyTeam1[playerIndex].hockeyPower.Hide();
                GetPlayerNextPuck();    
            }
        }
        private void PuckPassposition()
        {

            if (this.iceHockeyTeam1[playerIndex].iceHockeyPositionSide == IceHockeyPositionSide.M && this.iceHockeyTeam1[playerIndex].iceHockeyPosition == IceHockeyPosition.MF)
            {
                playerNumberIndexPass = 0;
                if (inputUpDown == InputUpDown.None && inputLeftRight == InputLeftRight.None)
                {
                    Random random = new Random();
                    var posPlayerPass = random.Next(0, 2);
                    if (posPlayerPass == 0)
                        playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.L);
                    else
                        playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.R);
                }
                else if (inputUpDown == InputUpDown.Up && inputLeftRight == InputLeftRight.None)
                {
                    Random random = new Random();
                    var posPlayerPass = random.Next(0, 2);
                    if (posPlayerPass == 0)
                        playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.L);
                    else
                        playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.R);
                }
                else if (inputUpDown == InputUpDown.None && inputLeftRight == InputLeftRight.Left)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.L);
                }
                else if (inputUpDown == InputUpDown.Up && inputLeftRight == InputLeftRight.Left)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.L);
                }
                else if (inputUpDown == InputUpDown.None && inputLeftRight == InputLeftRight.Right)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.R);
                }
                else if (inputUpDown == InputUpDown.Up && inputLeftRight == InputLeftRight.Right)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.R);
                }
                else if (inputUpDown == InputUpDown.Down && inputLeftRight == InputLeftRight.Left)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.DF && x.iceHockeyPositionSide == IceHockeyPositionSide.L);
                }
                else if (inputUpDown == InputUpDown.Down && inputLeftRight == InputLeftRight.Right)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.DF && x.iceHockeyPositionSide == IceHockeyPositionSide.R);
                }
                else if (inputUpDown == InputUpDown.Down && inputLeftRight == InputLeftRight.None)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.GK);
                }

            }
            else if (this.iceHockeyTeam1[playerIndex].iceHockeyPositionSide == IceHockeyPositionSide.L && this.iceHockeyTeam1[playerIndex].iceHockeyPosition == IceHockeyPosition.FW)
            {
                playerNumberIndexPass = 0;
                if (inputUpDown == InputUpDown.None && inputLeftRight == InputLeftRight.None)
                {
                    Random random = new Random();
                    var posPlayerPass = random.Next(0, 2);
                    if (posPlayerPass == 0)
                        playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.MF);
                    else
                        playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.R);
                }
                else if (inputUpDown == InputUpDown.Up && inputLeftRight == InputLeftRight.None)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.MF);
                }
                else if (inputUpDown == InputUpDown.None && inputLeftRight == InputLeftRight.Left)
                {
                    Random random = new Random();
                    var posPlayerPass = random.Next(0, 2);
                    if (posPlayerPass == 0)
                        playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.MF);
                    else
                        playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.R);
                }
                else if (inputUpDown == InputUpDown.Up && inputLeftRight == InputLeftRight.Left)
                {
                    Random random = new Random();
                    var posPlayerPass = random.Next(0, 2);
                    if (posPlayerPass == 0)
                        playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.MF);
                    else
                        playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.R);
                }
                else if (inputUpDown == InputUpDown.None && inputLeftRight == InputLeftRight.Right)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.R);
                }
                else if (inputUpDown == InputUpDown.Up && inputLeftRight == InputLeftRight.Right)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.R);
                }
                else if (inputUpDown == InputUpDown.Down && inputLeftRight == InputLeftRight.Right)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.MF);
                }
                else if (inputUpDown == InputUpDown.Down && inputLeftRight == InputLeftRight.None)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.MF);
                }
                else if (inputUpDown == InputUpDown.Down && inputLeftRight == InputLeftRight.Left)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.DF && x.iceHockeyPositionSide == IceHockeyPositionSide.L);
                }
            }
            else if (this.iceHockeyTeam1[playerIndex].iceHockeyPositionSide == IceHockeyPositionSide.R && this.iceHockeyTeam1[playerIndex].iceHockeyPosition == IceHockeyPosition.FW)
            {
                playerNumberIndexPass = 0;
                if (inputUpDown == InputUpDown.None && inputLeftRight == InputLeftRight.None)
                {
                    Random random = new Random();
                    var posPlayerPass = random.Next(0, 2);
                    if (posPlayerPass == 0)
                        playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.MF);
                    else
                        playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.L);
                }
                else if (inputUpDown == InputUpDown.Up && inputLeftRight == InputLeftRight.None)
                {
                    Random random = new Random();
                    var posPlayerPass = random.Next(0, 2);
                    if (posPlayerPass == 0)
                        playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.MF);
                    else
                        playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.L);
                }
                else if (inputUpDown == InputUpDown.None && inputLeftRight == InputLeftRight.Left)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.L);
                }
                else if (inputUpDown == InputUpDown.Up && inputLeftRight == InputLeftRight.Left)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.L);
                }
                else if (inputUpDown == InputUpDown.None && inputLeftRight == InputLeftRight.Right)
                {
                    Random random = new Random();
                    var posPlayerPass = random.Next(0, 2);
                    if (posPlayerPass == 0)
                        playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.MF);
                    else
                        playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.L);
                }
                else if (inputUpDown == InputUpDown.Up && inputLeftRight == InputLeftRight.Right)
                {
                    Random random = new Random();
                    var posPlayerPass = random.Next(0, 2);
                    if (posPlayerPass == 0)
                        playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.MF);
                    else
                        playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.L);
                }
                else if (inputUpDown == InputUpDown.Down && inputLeftRight == InputLeftRight.Left)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.MF);
                }
                else if (inputUpDown == InputUpDown.Down && inputLeftRight == InputLeftRight.None)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.MF);
                }
            }
            else if (this.iceHockeyTeam1[playerIndex].iceHockeyPositionSide == IceHockeyPositionSide.L && this.iceHockeyTeam1[playerIndex].iceHockeyPosition == IceHockeyPosition.DF)
            {
                playerNumberIndexPass = 0;
                if (inputUpDown == InputUpDown.None && inputLeftRight == InputLeftRight.None)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.MF);
                }
                else if (inputUpDown == InputUpDown.Up && inputLeftRight == InputLeftRight.None)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.L);
                }
                else if (inputUpDown == InputUpDown.Up && inputLeftRight == InputLeftRight.Left)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.L);
                }
                else if (inputUpDown == InputUpDown.Up && inputLeftRight == InputLeftRight.Right)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.MF);
                }
                else if (inputUpDown == InputUpDown.None && inputLeftRight == InputLeftRight.Right)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.DF && x.iceHockeyPositionSide == IceHockeyPositionSide.R);
                }
                else if (inputUpDown == InputUpDown.Down)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.GK);
                }
            }
            else if (this.iceHockeyTeam1[playerIndex].iceHockeyPositionSide == IceHockeyPositionSide.R && this.iceHockeyTeam1[playerIndex].iceHockeyPosition == IceHockeyPosition.DF)
            {
                playerNumberIndexPass = 0;
                if (inputUpDown == InputUpDown.None && inputLeftRight == InputLeftRight.None)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.MF);
                }
                else if (inputUpDown == InputUpDown.Up && inputLeftRight == InputLeftRight.None)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.R);
                }
                else if (inputUpDown == InputUpDown.Up && inputLeftRight == InputLeftRight.Left)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.MF);
                }
                else if (inputUpDown == InputUpDown.Up && inputLeftRight == InputLeftRight.Right)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.FW && x.iceHockeyPositionSide == IceHockeyPositionSide.R);
                }
                else if (inputUpDown == InputUpDown.None && inputLeftRight == InputLeftRight.Left)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.DF && x.iceHockeyPositionSide == IceHockeyPositionSide.L);
                }
                else if (inputUpDown == InputUpDown.Down)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.GK);
                }
            }
            else if (this.iceHockeyTeam1[playerIndex].iceHockeyPosition == IceHockeyPosition.GK)
            {
                playerNumberIndexPass = 0;
                if ((inputUpDown == InputUpDown.None && inputLeftRight == InputLeftRight.None) || inputUpDown == InputUpDown.Up || inputUpDown == InputUpDown.Down)
                {
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.MF);
                }
                else if (inputLeftRight == InputLeftRight.Left)
                {             
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.DF && x.iceHockeyPositionSide == IceHockeyPositionSide.L);
                }
                else if (inputLeftRight == InputLeftRight.Right)
                {                 
                    playerNumberIndexPass = this.iceHockeyTeam1.FindIndex(x => x.iceHockeyPosition == IceHockeyPosition.DF && x.iceHockeyPositionSide == IceHockeyPositionSide.R);
                }
            }
        }

        private void ChangePlayer()
        {            
            if (playerIndex == -1 || !this.iceHockeyTeam1[playerNumber].isPuckControl)
            {                
                if (changePlayerEnumSelect == ChangePlayerEnum.Press)
                {                    
                    Vector3 posPuck = puck.GlobalPosition;

                    var jogadorMaisProximoList = this.iceHockeyTeam1
                        .OrderBy(player => player.GlobalPosition.DistanceTo(posPuck)).Take(2).ToList();

                    foreach (var obj in this.iceHockeyTeam1)
                    {
                        obj.isSelected = false;
                        obj.ShowHideIceHockeySeletion(false);
                        if (obj.hockeyPower is not null)
                            obj.hockeyPower.Hide();
                    }

                    int index = 0;
                    if (playerNumber == jogadorMaisProximoList[1].playerNumber)
                        index = jogadorMaisProximoList[0].playerNumber;
                    else
                        index = jogadorMaisProximoList[1].playerNumber;
                    
                    this.iceHockeyTeam1[index].GetPlayerInput().ResetControlAndSelected();
                    this.iceHockeyTeam1[index].isSelected = true;
                    this.iceHockeyTeam1[index].GetPlayerInput().SetisSelected(ref this.iceHockeyTeam1[index].isSelected);                    
                    this.iceHockeyTeam1[index].ShowHideIceHockeySeletion(true);

                    changePlayerEnumSelect = ChangePlayerEnum.None;
                }
            }
        }

        public void PuckPass()
        {
            if (isSelected && isPuckControl && passPlayerEnum == PassPlayerEnum.Press)
            {                
                PuckPassposition();
                Vector3 impulse = ((new Vector3(this.iceHockeyTeam1[playerNumberIndexPass].GlobalPosition.X, 
                                    puck.GlobalPosition.Y, this.iceHockeyTeam1[playerNumberIndexPass].GlobalPosition.Z                                    
                                    ) - puck.GlobalPosition)) * 0.1f;

                inputShoot = InputShoot.None;
                isPuckControl = false;
                isSelected = false;
                this.iceHockeyTeam1[playerIndex].isPuckControl = false;
                this.iceHockeyTeam1[playerIndex].isSelected = false;
                this.iceHockeyTeam1[playerIndex].ShowHideIceHockeySeletion(false);
                if(this.iceHockeyTeam1[playerIndex].hockeyPower is not null)
                    this.iceHockeyTeam1[playerIndex].hockeyPower.Hide();

                this.iceHockeyTeam1[playerNumberIndexPass].isSelected = true;
                this.iceHockeyTeam1[playerNumberIndexPass].ShowHideIceHockeySeletion(true);                

                shootPower = 0.0f;
                puck.ApplyImpulse(impulse);
                passPlayerEnum = PassPlayerEnum.None;
            }
        }
        public void PuckPassAI(int playerAIId)
        {            
            Vector3 impulse = ((new Vector3(this.iceHockeyTeam2[playerAIId].GlobalPosition.X,
                                puck.GlobalPosition.Y, this.iceHockeyTeam2[playerAIId].GlobalPosition.Z
                                ) - puck.GlobalPosition)) * 0.1f;                
            isPuckControl = false;
            isSelected = false;
            this.iceHockeyTeam2[playerNumber].isPuckControl = false;
            this.iceHockeyTeam2[playerNumber].isSelected = false;
            this.iceHockeyTeam2[playerNumber].ShowHideIceHockeySeletion(false);

            if (this.iceHockeyTeam2[playerNumber].hockeyPower is not null)
                this.iceHockeyTeam2[playerNumber].hockeyPower.Hide();
            puck.ApplyImpulse(impulse);                        
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

        public void ResetControlAndSelected()
        {            
            foreach (var obj in iceHockeyTeam1)
            {
                obj.isSelected = false;
                obj.isPuckControl = false;
            }
            foreach (var obj in iceHockeyTeam2)
            {
                obj.isSelected = false;
                obj.isPuckControl = false;
            }
        }

        public void ResetControlAndSelectedTeam1()
        {
            foreach (var obj in iceHockeyTeam1)
            {
                obj.isSelected = false;
                obj.isPuckControl = false;
            }
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

        public void SetIsPlayerTeamPlayerNumber(bool isPlayerTeam, int playerNumber)
        {
            this.isPlayerTeam = isPlayerTeam;
            this.playerNumber = playerNumber;
        }
        private void GenerateRandomMove()
        {
            moveTime = 0;
            Random rnd = new Random();            
            moveTimeRandom = moveTimeMin + (float)rnd.NextDouble() * (moveTimeMax - moveTimeMin);            
        }
        private void GenerateRandomSpace()
        {            
            Random rnd = new Random();
            moveSpaceRandomMin = moveSpaceMin1 + (float)rnd.NextDouble() * (moveSpaceMin2 - moveSpaceMin1);
            moveSpaceRandomMax = moveSpaceMax1 + (float)rnd.NextDouble() * (moveSpaceMax2 - moveSpaceMax1);
        }
        private void GenerateRandomSpaceAI()
        {
            Random rnd = new Random();
            moveSpaceRandomAIMin = moveSpaceMinAI1 + (float)rnd.NextDouble() * (moveSpaceMinAI2 - moveSpaceMinAI1);
            moveSpaceRandomAIMax = moveSpaceMaxAI1 + (float)rnd.NextDouble() * (moveSpaceMaxAI2 - moveSpaceMaxAI1);
        }
        private void GenerateRandomShootAI()
        {
            Random rnd = new Random();
            moveSpaceRandomShootAI = moveSpaceRandomShootAIMin1 + (float)rnd.NextDouble() * (moveSpaceRandomShootAIMax1 - moveSpaceRandomShootAIMin1);            
        }
        private void GenerateRandomAttackPositionAI()
        {
            Random rnd = new Random();
            moveSpaceRandomPositionAI = rnd.Next(0, 2);            
        }
        private int GenerateRandomBinary()
        {
            Random rnd = new Random();
            return rnd.Next(0, 2);
        }
        public RigidBody3D GetPuck()
        {
            return this.puck;
        }
        public void SetGoalKeeperShoot(AnimationPlayer animationPlayer, int xDir, int zDir)
        {            
            if (xDir == 1 && zDir == 1)
                PlayAnimationOpponent(animationPlayer, 7);
            else if (xDir == -1 && zDir == 1)
                PlayAnimationOpponent(animationPlayer, 5);
            else if (xDir == 1 && zDir == -1)
                PlayAnimationOpponent(animationPlayer, 6);
            else if (xDir == -1 && zDir == -1)
                PlayAnimationOpponent(animationPlayer, 4);
        }

        public void PlayAnimationOpponent(AnimationPlayer animationPlayer, int animID)
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
        public void SetisCollidedWithTeamMate(bool isCollidedWithTeamMate, bool isMove)
        {
            this.isCollidedWithTeamMate = isCollidedWithTeamMate;
            this.isMove = isMove;
        }
    }
}
