using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.Controller;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Interfaces;
using WinterSports.Scripts.Model;
using WinterSports.Scripts.Static;
using static WinterSports.Scripts.Events.PlayerInputIceHockey;

namespace WinterSports.Scripts.Events
{
    public class PlayerInputSkiJump : IPlayerInput
    {
        #region Variables
        private bool isPause = false;
        private TimerController timerGamePlayController = null;
        private Character character = null;
        private CharacterBody3D characterBody3D = null;
        private int startPointId = 0;
        private int currentPointId = 0;
        private List<SpeedSkatingTrackDTO> skiJumpTrackDTOList = new List<SpeedSkatingTrackDTO>();
        private int windAngle = 0;
        private int windPower = 0;
        private float caX = 0.0f;
        private float coY = 0.0f;
        private float angle = 0.0f;
        private float power = 0.0f;
        private int[] flyPoints = null;
        private bool isLandPlayed = false;
        private Control finishSessionScreen = null;
        private Control pauseScreen = null;
        #endregion
        #region Constants
        private const float maxTime = 0.05f;
        private const float inc = 1.1f;
        private const float windInc = 1.0f;
        IDictionary<int, string> animName = new Dictionary<int, string>()
        {
            {1,"Idle_Ski_Jump"},
            {2,"Idle_Ski_Jump_Run"},
            {3,"Idle_Ski_Jump_Jumping"},
            {4,"Idle_Ski_Jump_Landing"}
        };
        #endregion
        #region Implements
        public void PlayerInput(AnimationPlayer animationPlayer, double delta = 0.0f, int positionID = 0,
            CrossCountryOvertake crossCountryOvertakeM = null, CrossCountryOvertake crossCountryOvertakeFR = null, CrossCountryOvertake crossCountryOvertakeFL = null)
        {
            if (!isPause)
            {
                MovePlayerInput();
                MovePlayer(delta);
                WindDirection(animationPlayer);
            }            
        }
        public void PlayAnimation(AnimationPlayer animationPlayer, int animID)
        {
            if (animID == 4)
            {
                if (!isLandPlayed)
                {
                    animationPlayer.Play(animName[animID]);
                    isLandPlayed = true;
                }                
            }            
            else
                animationPlayer.Play(animName[animID]);
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
        public void SetSkiCollision(SkiCollision skiCollision) { }
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
        public void Init(List<List<CrossCountryModel>> crossCountryModelAIList = null, int initLine = 0)
        {
            timerGamePlayController = new TimerController();
            timerGamePlayController.Init();
            timerGamePlayController.StartTimer();
            GenerateWind();
        }
        public void Reset()
        {
            startPointId = 0;
            currentPointId = 0;
            skiJumpTrackDTOList = new List<SpeedSkatingTrackDTO>();
            windAngle = 0;
            windPower = 0;
            caX = 0.0f;
            coY = 0.0f;
            angle = 0.0f;
            power = 0.0f;
            GenerateWind();
        }
        public float GetSpeed()
        {
            return 0.0f;
        }
        public float GetMaxSpeed()
        {
            return 0.0f;
        }
        public void SetSkiJumpPoint(int[] flyPoints) 
        {
            this.flyPoints = flyPoints;            
        }
        public void SetWindSkiJump(float angle, float power) 
        {            
            this.angle = angle;
            this.power = power;
        }
        public void SetRailSpeedSkating(int startPointId, List<SpeedSkatingTrackDTO> speedSkatingTrackDTOList, List<DirectionArrow> directionArrowList)
        {
            this.startPointId = startPointId;
            currentPointId = startPointId;
            skiJumpTrackDTOList = speedSkatingTrackDTOList;
        }
        public void SetRailBiathlon(int startPointId, List<List<SpeedSkatingTrackDTO>> speedSkatingTrackDTOList, List<List<DirectionArrow>> directionArrowList)
        { }
        public void SetCharacter(Character character)
        {
            this.character = character;
        }
        public bool GetPause()
        {
            return isPause;
        }
        public float GetEnergy()
        {
            return 0.0f;
        }
        public bool GetIsFinished() { return false; }
        public bool GetIsAccel() { return false; }
        public bool GetIsBreak() { return false; }
        public void SetIsAI(bool isAI) { }

        public void SetCharacterIdCountry(int characterIdCountry){}
        public int GetLinePosition()
        {
            return 0;
        }
        public bool GetIsAI()
        {
            return false;
        }
        public void OnlyPause()
        {
            isPause = !isPause;
            Engine.TimeScale = isPause ? 0.0f : 1.0f;
        }
        #endregion
        #region Methods
        private void MovePlayerInput()
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
                    if (character.statesSki == Character.StatesSki.Ready)
                    {
                        if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[5].keyId))//Button 1
                        {                         
                            character.statesSki = Character.StatesSki.Running;
                        }
                    }
                    if (this.character.statesSki == Character.StatesSki.SkiJumpingDown)
                    {
                        if (currentPointId > this.flyPoints[0] && currentPointId <= this.flyPoints[1])
                        {
                            if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[3].keyId))//Button 3 LEFT
                            {
                                character.RotateZ(true, inc);                                
                            }
                            else if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[4].keyId))//Button 4 RIGHT
                            {
                                character.RotateZ(false, inc);                                
                            }
                        }
                        else if (currentPointId > this.flyPoints[1] && currentPointId <= this.flyPoints[2])
                        {
                            if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[1].keyId))//Button 1 UP
                            {
                                character.RotateX(false, inc);                                
                            }
                            else if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[2].keyId))//Button 2 DOWN
                            {
                                character.RotateX(true, inc);
                            }
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
                    if (character.statesSki == Character.StatesSki.Ready)
                    {
                        if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[5].keyId))
                        {                            
                            character.statesSki = Character.StatesSki.Running;
                        }
                    }
                    if (this.character.statesSki == Character.StatesSki.SkiJumpingDown)
                    {
                        if (currentPointId > this.flyPoints[0] && currentPointId <= this.flyPoints[1])
                        {
                            if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[3].keyId))//Button 3 LEFT
                            {
                                character.RotateZ(true, inc);
                            }
                            else if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[4].keyId))//Button 4 RIGHT
                            {
                                character.RotateZ(false, inc);
                            }
                        }
                        else if (currentPointId > this.flyPoints[1] && currentPointId <= this.flyPoints[2])
                        {
                            if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[1].keyId))//Button 1 UP
                            {
                                character.RotateX(false, inc);
                            }
                            else if (Input.IsJoyButtonPressed(joystickInput, (JoyButton)ConfigSingleton.saveConfigDTO.keysControlArray[2].keyId))//Button 2 DOWN
                            {
                                character.RotateX(true, inc);
                            }
                        }                        
                    }
                }
            }            
        }
        public void PlayerInputAI(AnimationPlayer animationPlayer, double delta = 0.0f)
        {

        }
        private void WindDirection(AnimationPlayer animationPlayer)
        {

            if (this.character.statesSki == Character.StatesSki.Ready)
            {
                character.ShoHideControlSkiJumpImpulseHorizontal(false);
                character.ShoHideControlSkiJumpImpulseVertical(false);
            }            
            if (this.character.statesSki == Character.StatesSki.SkiJumpingDown)
            {                
                if (currentPointId > this.flyPoints[0] && currentPointId <= this.flyPoints[1])
                {             
                    var incWind = MoveCameraZWind(power, -1.0f * angle);
                    character.RotateZWind(incWind);
                    character.ShoHideControlSkiJumpImpulseHorizontal(true);
                    character.ShoHideControlSkiJumpImpulseVertical(false);
                    PlayAnimation(animationPlayer, 2);
                }
                else if (currentPointId > this.flyPoints[1] && currentPointId <= this.flyPoints[2])
                {
                    var incWind = MoveCameraXWind(power, -1.0f * angle);
                    character.RotateXWind(incWind);
                    character.ShoHideControlSkiJumpImpulseHorizontal(false);
                    character.ShoHideControlSkiJumpImpulseVertical(true);
                    PlayAnimation(animationPlayer, 3);
                }
                else if (currentPointId > this.flyPoints[2])
                {                    
                    character.ShoHideControlSkiJumpImpulseVertical(false);
                    PlayAnimation(animationPlayer, 4);
                }
            }
        }
        private void MovePlayer(double delta)
        {
            try
            {
                if (character.statesSki == Character.StatesSki.Running)
                {
                    character.GetSetImpulseRail = character.CalculateImpulsePercent();                    
                }
                else if (this.character.statesSki == Character.StatesSki.SkiJumpingDown)
                {                    
                    if (timerGamePlayController.GetTimer() > maxTime)
                    {                        
                        currentPointId++;
                        this.character.Position = new Vector3(
                            skiJumpTrackDTOList[currentPointId].position.X,
                            skiJumpTrackDTOList[currentPointId].position.Y, 
                            skiJumpTrackDTOList[currentPointId].position.Z);

                        var newPositionLookAt = Vector3.Zero;

                        if (currentPointId + 1 == skiJumpTrackDTOList.Count)
                        {
                            newPositionLookAt = new Vector3(
                                skiJumpTrackDTOList[currentPointId].position.X,
                                skiJumpTrackDTOList[currentPointId].position.Y,
                                skiJumpTrackDTOList[currentPointId].position.Z);
                        }
                        else
                        {
                            newPositionLookAt = new Vector3(
                                skiJumpTrackDTOList[currentPointId + 1].position.X,
                                skiJumpTrackDTOList[currentPointId + 1].position.Y,
                                skiJumpTrackDTOList[currentPointId + 1].position.Z);
                        }
                        if (this.characterBody3D.Position != newPositionLookAt)
                        {                            
                            this.characterBody3D.LookAt(newPositionLookAt, Vector3.Up);
                            this.characterBody3D.RotateObjectLocal(Vector3.Up, Mathf.DegToRad(180.0f));
                        }
                        timerGamePlayController.ResetTimerRunning();
                    }
                    timerGamePlayController.TimerRunning(delta);                                            
                    if (currentPointId > skiJumpTrackDTOList.Count - 10)
                    {
                        ShowHideFinishSessionScreen();
                        character.statesSki = Character.StatesSki.SkiJumpingFinish;
                    }                    
                }
            }
            catch (Exception ex)
            { 
            }
        }
        private void GenerateWind()
        {
            Random rnd = new Random();
            windAngle = rnd.Next(0, 361);
            Random rndWindPower = new Random();
            windPower = rndWindPower.Next(0, 100);
            DefineShootingWindForce();
        }
        private void DefineShootingWindForce()
        {
            var angle = Mathf.DegToRad((float)windAngle);
            var hyp = windPower;
            var cos = Math.Cos(angle);
            caX = (float)(cos * hyp);
            var sin = Math.Sin(angle);
            coY = (float)(sin * hyp);
            DefineWindUI((-1.0f * (float)windAngle), ((float)windPower) / 100.0f);
        }
        private void DefineWindUI(float angle, float power)
        {
            character.SetWindSkiJump(angle, power);
        }
        public float MoveCameraXWind(float moveInc, float windAngle)
        {
            var moveIncNormal = (windAngle > 180.0f && windAngle <= 360.0f ? 1.0f : -1.0f) * MathF.Abs(moveInc);
            return ((moveIncNormal / windInc));            
        }

        public float MoveCameraZWind(float moveInc, float windAngle)
        {            
            var moveIncNormal = (windAngle > 90.0f && windAngle <= 270.0f ? 1.0f : -1.0f) * MathF.Abs(moveInc);
            return ((moveIncNormal / windInc));            
        }
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
        public void SetIceHockeyGoals(IceHockeyGoal Goal1, IceHockeyGoal Goal2)
        {

        }
        public void SetIceHockeyTeams(ref List<Character> iceHockeyTeam1, ref List<Character> iceHockeyTeam2) { }
        public void SetPuck(RigidBody3D puck) { }
        public void SetisSelected(ref bool isSelected) { }
        public void SetisPuckControl(ref bool isPuckControl) { }
        public bool GetisSelected() { return false; }
        public bool GetisPuckControl() { return false; }
        public InputShoot GetinputShoot()
        {
            return InputShoot.None;
        }
        public void SetObj<T>(T obj)
        {
            
        }
        public void SetIsPlayerTeamPlayerNumber(bool isPlayerTeam, int playerNumber)
        {

        }
        public RigidBody3D GetPuck()
        {
            return null;
        }
        public void ResetControlAndSelected()
        { }
        #endregion
    }
}
