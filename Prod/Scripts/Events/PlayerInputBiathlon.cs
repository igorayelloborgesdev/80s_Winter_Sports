using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.Controller;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Interfaces;
using WinterSports.Scripts.Model;
using WinterSports.Scripts.Static;
using static Character;
using static Godot.TextServer;
using static WinterSports.Scripts.Events.PlayerInputIceHockey;

namespace WinterSports.Scripts.Events
{
    public class PlayerInputBiathlon : IPlayerInput
    {
        #region Variables
        private bool isPause = false;
        private int keyPressedId = 0;
        private int indexSpeed = 0;
        private bool keyEnable = false;
        private float speed = 0.0f;
        private List<List<DirectionArrow>> directionArrowList = new List<List<DirectionArrow>>();
        private List<List<SpeedSkatingTrackDTO>> biathlonTrackDTOList = new List<List<SpeedSkatingTrackDTO>>();
        private int startPointId = 0;
        private TimerController timerGamePlayController = null;
        private CharacterBody3D characterBody3D = null;
        private Control pauseScreen = null;
        private Control finishSessionScreen = null;
        private string currentAnimation = "";
        private Character character = null;
        private int shootingTries = 5;
        private int shootingErrors = 0;
        private int windAngle = 0;
        private int windPower = 0;
        private float caX = 0.0f;
        private float coY = 0.0f;
        private List<TargetBoard> targetBoardList = new List<TargetBoard>();
        #endregion
        #region Constant        
        IDictionary<int, string> animName = new Dictionary<int, string>()
        {
            {1,"Idle"},
            {2,"CrossCountry_Run"}
        };
        private const float maxSpeed = 5.0f;
        private const double timeSpeedCurrentMin = 0.02;
        private const double timeSpeedCurrentMax = 0.10;
        private const double timeSpeedCurrentInc = 0.01;
        private float increment = 0.01f;
        #endregion
        #region Implements
        public void PlayerInput(AnimationPlayer animationPlayer, double delta = 0.0f, int positionID = 0,
            CrossCountryOvertake crossCountryOvertakeM = null, CrossCountryOvertake crossCountryOvertakeFR = null, CrossCountryOvertake crossCountryOvertakeFL = null) 
        {
            if (!isPause)
            {
                MovePlayerInput();
                MovePlayer(delta);                
                if (BiathlonStatic.isShooting) 
                {
                    PlayAnimation(animationPlayer, 1);
                }
                else
                {
                    if (biathlonTrackDTOList[BiathlonStatic.id].Count == startPointId && BiathlonStatic.isLapFinished)                        
                    {                        
                        PlayAnimation(animationPlayer, 1);
                    }
                    else
                    {                        
                        PlayAnimation(animationPlayer, 2);
                    }                    
                }                
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
        public bool GetPause()
        { 
            return isPause;
        }
        public void ShowHideFinishSessionScreen()
        {
            isPause = !isPause;
            Engine.TimeScale = isPause ? 0.0f : 1.0f;
            ShowHideFinishSessionScreenMenu(isPause);
        }
        public void Init(List<List<CrossCountryModel>> crossCountryModelAIList = null, int initLine = 0)
        {
            speed = (float)timeSpeedCurrentMax;
            timerGamePlayController = new TimerController();
            timerGamePlayController.Init();
            timerGamePlayController.StartTimer();            
            startPointId = 0;
            shootingTries = 5;
            shootingErrors = 0;
            windAngle = 0;
            windPower = 0;
            caX = 0.0f;
            coY = 0.0f;
        }
        public void Reset()
        {
            speed = (float)timeSpeedCurrentMax;
            foreach (var targetBoard in targetBoardList)
            {
                targetBoard.EnableAllTargetById();
            }
            targetBoardList.Clear();
        }
        public float GetSpeed()
        {            
            return (float)(-1.0f * ((speed - timeSpeedCurrentMin) - (timeSpeedCurrentMax - timeSpeedCurrentMin)));
        }
        public float GetMaxSpeed()
        {
            return (float)(timeSpeedCurrentMax - timeSpeedCurrentMin);
        }
        public void SetRailSpeedSkating(int startPointId, List<SpeedSkatingTrackDTO> speedSkatingTrackDTOList, List<DirectionArrow> directionArrowList)
        {
        }
        public void SetSkiJumpPoint(int[] flyPoints) { }        
        public void SetWindSkiJump(float angle, float power) { }

        public void SetSkiCollision(SkiCollision skiCollision) { }
        public void SetRailBiathlon(int startPointId, List<List<SpeedSkatingTrackDTO>> biathlonTrackDTOList, List<List<DirectionArrow>> directionArrowList)
        {
            this.startPointId = startPointId;
            this.biathlonTrackDTOList = biathlonTrackDTOList;
            this.directionArrowList = directionArrowList;
        }
        public void SetCharacter(Character character)
        { 
            this.character = character;
        }
        public float GetEnergy()
        {
            return 0.0f;
        }

        public bool GetIsFinished(){ return false; }
        public bool GetIsAccel() { return false; }
        public bool GetIsBreak() { return false; }
        public void SetIsAI(bool isAI){}
        public void SetCharacterIdCountry(int characterIdCountry) { }
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
        private void ShowHidePauseMenu(bool isPause)
        {
            if (isPause)
                pauseScreen.Show();
            else
                pauseScreen.Hide();
        }
        private void ShowHideFinishSessionScreenMenu(bool isPause)
        {
            if (isPause)
                finishSessionScreen.Show();
            else
                finishSessionScreen.Hide();
        }
        private void SpeedManager(int index)
        {            
            if (BiathlonStatic.isCollided && (BiathlonStatic.direction == index))
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
                directionArrowList[BiathlonStatic.id][BiathlonStatic.idY].SetBodyColor(colorId);
            }
            catch (Exception ex)
            {                                
                BiathlonStatic.idY = 0;
                BiathlonStatic.currentIndex = 0;
            }

        }
        private void SetPlayerScore(bool enable)
        {
            try
            {
                directionArrowList[BiathlonStatic.id][BiathlonStatic.idY].playerScore = enable;
            }
            catch (Exception ex)
            {
            }
        }
        private void SetEnableDisableArrow(bool enable)
        {
            try
            {
                directionArrowList[BiathlonStatic.id][BiathlonStatic.idY].enable = enable;
            }
            catch (Exception ex)
            {                
                BiathlonStatic.idY = 0;
                BiathlonStatic.currentIndex = 0;
            }
        }
        private void MovePlayer(double delta)
        {
            try
            {
                if (!BiathlonStatic.isShooting)
                {                    
                    if (timerGamePlayController.GetTimer() > DefineSpeed())
                    {
                        if (biathlonTrackDTOList[BiathlonStatic.id].Count == startPointId && !BiathlonStatic.isLapFinished)
                        {
                            BiathlonStatic.isShooting = true;
                            character.SpeedBoxShowHide(false);
                            character.MoveCameraPositionRotation(2);
                            character.statesSki = StatesSki.Shooting;
                            character.ShowHideTarget(true);
                            character.ShowHideControlSkiSpeedSkatingBiathlon(false);
                            character.ShowHideControlBiathlon(true);
                            character.SetShoots(shootingTries);
                            character.SetErrors(shootingErrors);
                            GenerateWind();
                            speed = 0.0f;
                        }
                        else
                            startPointId++;
                        this.characterBody3D.Position = new Vector3(biathlonTrackDTOList[BiathlonStatic.id][startPointId].position.X,
                            this.characterBody3D.Position.Y, biathlonTrackDTOList[BiathlonStatic.id][startPointId].position.Z);
                        var newPositionLookAt = Vector3.Zero;

                        if (startPointId + 1 == biathlonTrackDTOList[BiathlonStatic.id].Count)
                        {                                         
                            newPositionLookAt = new Vector3(biathlonTrackDTOList[BiathlonStatic.id][startPointId].position.X,
                                this.characterBody3D.Position.Y, biathlonTrackDTOList[BiathlonStatic.id][startPointId].position.Z);
                        }
                        else
                        {
                            newPositionLookAt = new Vector3(biathlonTrackDTOList[BiathlonStatic.id][startPointId + 1].position.X,
                                this.characterBody3D.Position.Y, biathlonTrackDTOList[BiathlonStatic.id][startPointId + 1].position.Z);
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
            }
            catch (Exception ex)
            {
            }
        }
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
                    if (!BiathlonStatic.isShooting)
                    {
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
                        if (!isPause)
                        {
                            if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[1].keyId))//Button 1 UP
                            {                                
                                character.MoveCameraX(false);                                                                
                            }
                            else if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[2].keyId))//Button 2 DOWN
                            {                             
                                character.MoveCameraX(true);                                                  
                            }
                            if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[3].keyId))//Button 3 LEFT
                            {
                                character.MoveCameraY(false);
                            }
                            else if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[4].keyId))//Button 4 RIGHT
                            {
                                character.MoveCameraY(true);
                            }
                            if (Input.IsKeyPressed((Key)ConfigSingleton.saveConfigDTO.keysControlArray[5].keyId) && !keyEnable)//Shoot
                            {
                                Shoot();
                                keyPressedId = ConfigSingleton.saveConfigDTO.keysControlArray[5].keyId;                                
                                keyEnable = true;
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
                            if (!BiathlonStatic.isShooting)
                            {
                                keyEnable = false;
                                SpeedManager(indexSpeed);
                                indexSpeed = 0;
                            }
                            else
                            {
                                if ((Key)keyPressedId == (Key)ConfigSingleton.saveConfigDTO.keysControlArray[5].keyId)
                                {
                                    keyEnable = false;                                    
                                }                                
                            }                            
                        }                        
                    }
                }
            }
            if (!isPause)
            {
                if (BiathlonStatic.isShooting)
                {
                    character.MoveCameraXWind(coY, windAngle);
                    character.MoveCameraYWind(caX, windAngle);
                }
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
        private void GetNotScore()
        {
            if (BiathlonStatic.isNotScore)
            {
                BiathlonStatic.isNotScore = false;
                speed += (float)timeSpeedCurrentInc;
            }

        }
        private void Shoot()
        {            
            if (character.GetTargetRayCast().IsColliding())
            {                
                var collider = character.GetTargetRayCast().GetCollider() as Node;                
                if (collider is not null)
                {                    
                    var target = collider.GetParent() as Target;
                    if (target.GetSetEnable)
                    {                        
                        var targetBoard = collider.GetParent().GetParent() as TargetBoard;                        
                        targetBoard.DisableTargetById(target.GetId);
                        target.GetSetEnable = false;
                        targetBoardList.Add(targetBoard);
                    }
                    else 
                    {
                        shootingErrors++;
                        character.SetErrors(shootingErrors);
                    }
                }                
            }
            else
            {
                shootingErrors++;
                character.SetErrors(shootingErrors);                
            }
            shootingTries--;
            character.SetShoots(shootingTries);
            DefineShootFinishing();
        }
        private void DefineShootFinishing()
        {
            if (shootingTries == 0)
            {                
                BiathlonStatic.id++;
                BiathlonStatic.idY = 0;
                BiathlonStatic.isShooting = false;
                BiathlonStatic.isCollided = false;
                BiathlonStatic.direction = 0;
                character.statesSki = StatesSki.Init;                
                character.ShowHideControlSkiSpeedSkatingBiathlon(true);
                character.ShowHideControlBiathlon(false);
                character.MoveCameraPositionRotation(1);
                character.ShowHideTarget(false);
                character.SpeedBoxShowHide(true);
                startPointId = 0;
                speed = (float)timeSpeedCurrentMax;
                shootingTries = 5;
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
            DefineWindUI((-1.0f * (float)windAngle), ((float)windPower)/100.0f);
        }
        private void DefineWindUI(float angle, float power)
        {
            character.SetWind(angle, power);
        }
        public void SetIceHockeyGoals(IceHockeyGoal Goal1, IceHockeyGoal Goal2)
        {

        }
        public void SetIceHockeyTeams(List<Character> iceHockeyTeam1, List<Character> iceHockeyTeam2) { }
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
        #endregion
    }
}
