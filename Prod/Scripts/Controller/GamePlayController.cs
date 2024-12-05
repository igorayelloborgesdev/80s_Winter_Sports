using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Interfaces;
using WinterSports.Scripts.Model;
using WinterSports.Scripts.Prefabs;
using WinterSports.Scripts.Singleton;
using WinterSports.Scripts.Static;
using static Character;

namespace WinterSports.Scripts.Controller
{
    public class GamePlayController
    {
        #region Controller Ski
        private TimerController timerController = null;
        private TimerController timerGamePlayController = null;
        private TimerController timerResetController = null;
        #endregion
        #region Model
        private GamePlayModel gamePlayModel = new GamePlayModel();
        private SpeedSkatingModel speedSkatingModel = new SpeedSkatingModel();
        #endregion
        #region Variables        
        private Character character = null;        
        private LugeSled lugeSled = null;
        private BobsleighSled bobsleighSled = null;
        private Vector3 initPosition = Vector3.Zero;
        private Vector3 initRotation = Vector3.Zero;
        private Control pauseScreen = null;
        private Control finishSessionScreen = null;
        private Label timeLabel = null;
        private NinePatchRect speedNinePatchRect = null;
        private NinePatchRect impulseNinePatchRect = null;
        private NinePatchRect impulseSkiJumpNinePatchRect = null;
        private Control readySetGoControl = null;
        private Label readySetGoLabel = null;
        private Label countryCodeLabel = null;
        private TextureRect countryFlagTextureRect = null;
        private List<DirectionArrow> directionArrowList = new List<DirectionArrow>();
        private List<List<DirectionArrow>> directionArrowBiathlonList = new List<List<DirectionArrow>>();
        private Label countryCodeLabelFinish = null;
        private TextureRect countryFlagTextureRectFinish = null;
        private Label timeScoreBestLabelFinish = null;
        private Label timeScoreLastLabelFinish = null;        
        private bool setScore = true;
        private Control controlSkiSpeedSkatingBiathlon = null;
        private Control controlBiathlon = null;
        private Control controlSkiJumpImpulseHorizontal = null;
        private Control windDirectionArrowHorizontal = null;
        private Control controlSkiJumpImpulseVertical = null;
        private Control windDirectionArrowVertical = null;
        private Label shoots = null;
        private Label errorLabelScore = null;
        private Label windDirection = null;
        private Control windDirectionArrow = null;
        private Control controlSkiSpeedSkatingScreen = null;
        private Control controlBiathlonScreen = null;
        private Control controlLugeImpulse = null;
        private Control controlSkiJumpImpulse = null;
        private Control controlSkiJump = null;
        private Label bestScoreLabel = null;
        private Label timeScoreLabel = null;
        private Label errorsScoreLabel = null;
        private Label lastScoreLabel = null;
        private Control controlSkiCrossCountry = null;
        private Label controlSkiCrossCountryTime = null;
        private NinePatchRect controlSkiCrossCountrySpeed = null;
        private NinePatchRect controlSkiCrossCountryEnergy = null;        
        private List<CrossCountryDTO> crossCountryDTOList = new List<CrossCountryDTO>();
        private List<Character> characterCrossCountryList = new List<Character>();        
        private int crossCountryPlayerPosition = 0;
        private Control controlSkiCrossCountryPosition = null;
        private Label[] crossCountryCountryPositionLabel = null;
        private Label[] crossCountryCountryCodeLabel = null;
        private TextureRect[] crossCountryCountryFlagTextureRect = null;
        private Control finishResultSessionControl = null;
        private List<Node> standingNode = new List<Node>();
        #endregion
        #region const
        private const float rectXSize = 225.0f;
        private const string flagResource = "res://Art//2d//flags//";
        private string[] prefabNameTimerList = { "skiTrack", "SpeedSkating", "Biathlon", "LugeBobsleigh", "Skijumping" };
        private string[] prefabNameCountryList = { "skiTrack", "SpeedSkating", "Biathlon", "LugeBobsleigh", "Skijumping" };
        #endregion
        #region Methods
        public void Init(string prefabName)
        {
            if (prefabName == "skiTrack")
                InitSki();
            if (prefabName == "SpeedSkating")
                InitSpeedSkating();
            if (prefabName == "Biathlon")
                InitBiathlon();
            if (prefabName == "LugeBobsleigh")
                InitLuge();
            if (prefabName == "Skijumping")
                InitSkiJump();
        }
        public void Update(double delta, string prefabName, int levelId)
        {
            if (prefabName == "skiTrack")
            {
                if (levelId == 11)
                {
                    UpdateSkiCrossCountry(delta);//<-
                }
                else
                {
                    UpdateSki(delta);
                }                                
            }            
            if (prefabName == "SpeedSkating")
                UpdateSpeedSkating(delta);
            if (prefabName == "Biathlon")
                UpdateBiathlon(delta);
            if (prefabName == "LugeBobsleigh")
                UpdateLuge(delta);
            if (prefabName == "Skijumping")
                UpdateSkiJump(delta);
        }
        public void UpdateSkiJumpRail(string prefabName, SkiJump skiJump)
        {
            if (character is not null)
            {
                if (prefabName == "Skijumping" && character.statesSki == Character.StatesSki.Running)
                {
                    skiJump.InstantiateRail(character.GetSetImpulseRail);
                    character.SetRailSpeedSkating(skiJump.GetStartPointId, skiJump.GetSkiJumpTrackDTOList);
                    character.SetSkiJumpPoint(skiJump.GetFlyPoints);
                }
            }            
        }
        private void UpdateSki(double delta) 
        {
            timerGamePlayController.TimerRunning(delta);
            if (timerGamePlayController.GetTimer() > 1.0f && this.character.statesSki == Character.StatesSki.Ready)
            {
                this.character.statesSki = Character.StatesSki.Set;
                readySetGoControl.Show();
                readySetGoLabel.Text = "Ready";
            }
            else if (timerGamePlayController.GetTimer() > 2.0f && this.character.statesSki == Character.StatesSki.Set)
            {
                this.character.statesSki = Character.StatesSki.Go;
                readySetGoLabel.Text = "Set";
            }
            else if (timerGamePlayController.GetTimer() > 3.0f && this.character.statesSki == Character.StatesSki.Go)
            {
                this.character.statesSki = Character.StatesSki.Init;
                timerGamePlayController.StopTimer();
                timerGamePlayController.ResetTimer();
                readySetGoLabel.Text = "Go";
                setScore = true;
            }
            if (this.character.statesSki == Character.StatesSki.Running)
            {
                timerController.StartTimer();
                readySetGoControl.Hide();
            }
            if (this.character.statesSki == Character.StatesSki.Finish && !SkiStatic.isCollided)
            {                
                SetTimeScore();
                timerController.StopTimer();
                TimeToReset(delta);
                ShowControlSkiSpeedSkatingScreen();
            }
            if (SkiStatic.isCollided)
            {                
                this.character.statesSki = Character.StatesSki.Disqualified;                
                SetTimeScore();
                TimeToReset(delta);
                ShowControlSkiSpeedSkatingScreen();
            }
            timerController.TimerRunning(delta);
            updateTimer();
            UpdateSpeedLabel();
        }
        private void UpdateSkiCrossCountry(double delta)
        {            
            timerGamePlayController.TimerRunning(delta);
            if (timerGamePlayController.GetTimer() > 1.0f && this.character.statesSki == Character.StatesSki.Ready)
            {
                this.character.statesSki = Character.StatesSki.Set;
                readySetGoControl.Show();
                readySetGoLabel.Text = "Ready";
            }
            else if (timerGamePlayController.GetTimer() > 2.0f && this.character.statesSki == Character.StatesSki.Set)
            {
                this.character.statesSki = Character.StatesSki.Go;
                readySetGoLabel.Text = "Set";
            }
            else if (timerGamePlayController.GetTimer() > 3.0f && this.character.statesSki == Character.StatesSki.Go)
            {
                this.character.statesSki = Character.StatesSki.Init;
                readySetGoLabel.Text = "Go";
                setScore = true;
                timerController.StartTimer();
                StartAI();
            }
            else if (timerGamePlayController.GetTimer() > 5.0f && this.character.statesSki == Character.StatesSki.Init)
            {
                readySetGoControl.Hide();
                timerGamePlayController.StopTimer();
                timerGamePlayController.ResetTimer();
                this.character.statesSki = Character.StatesSki.Running;
                crossCountryDTOList.Clear();
                CrossCountryStatic.isPause = false;
            }
            else if (this.character.statesSki == Character.StatesSki.Running && timerGamePlayController.GetTimer() < 5.0f)
            {
                foreach (var charObj in characterCrossCountryList)
                {
                    charObj.SetScore();
                    if (!charObj.GetIsRunFinished)
                    {
                        charObj.GetSetScore = timerController.GetTimer();                        
                    }
                    else
                    {
                        if (GameModeSingleton.country == charObj.GetSetCharacterIdCountry)
                        {
                            timerGamePlayController.StartTimer();                            
                        }
                    }
                }
            }
            else if (this.character.statesSki == Character.StatesSki.Running && timerGamePlayController.GetTimer() > 2.0f)
            {
                this.character.statesSki = Character.StatesSki.Finish;
                this.character.Pause();

                characterCrossCountryList = characterCrossCountryList.OrderBy(x => x.GetIsRunFinished).OrderBy(x => x.GetSetScore).OrderByDescending(x => x.GetSetCurrentPoint).ToList();

                GD.Print("-------------------------------");
                int count = 1;
                foreach (var charObj in characterCrossCountryList)
                {
                    GD.Print(count.ToString() + " - " + CountrySingleton.countryObjDTO.countryList[charObj.GetSetCharacterIdCountry - 1].Name + " : "
                        + charObj.GetIsRunFinished.ToString() + " : "
                        + charObj.GetSetScore.ToString() + " : "
                        + charObj.GetSetCurrentPoint.ToString());//<-
                    count++;
                }                    
            }

                //if (this.character.statesSki == Character.StatesSki.Running)
                //{

                //}
                //if (this.character.statesSki == Character.StatesSki.Finish && !SkiStatic.isCollided)
                //{
                //    SetTimeScore();
                //    timerController.StopTimer();
                //    TimeToReset(delta);
                //    ShowControlSkiSpeedSkatingScreen();
                //}
                //if (SkiStatic.isCollided)
                //{
                //    this.character.statesSki = Character.StatesSki.Disqualified;
                //    SetTimeScore();
                //    TimeToReset(delta);
                //    ShowControlSkiSpeedSkatingScreen();
                //}
            timerController.TimerRunning(delta);
            updateTimerCrossCountry();
            UpdateSpeedEnergyLabel();
            OrderCrossCountryPosition();
            SetPlayerPosition();
            SetPlayerPositionUI();
        }
        private void UpdateSpeedSkating(double delta)
        {            
            timerGamePlayController.TimerRunning(delta);
            if (timerGamePlayController.GetTimer() > 1.0f && this.character.statesSki == Character.StatesSki.Ready)
            {
                this.character.statesSki = Character.StatesSki.Set;
                readySetGoControl.Show();
                readySetGoLabel.Text = "Ready";
            }
            else if (timerGamePlayController.GetTimer() > 2.0f && this.character.statesSki == Character.StatesSki.Set)
            {
                this.character.statesSki = Character.StatesSki.Go;
                readySetGoLabel.Text = "Set";
            }
            else if (timerGamePlayController.GetTimer() < 4.0f && timerGamePlayController.GetTimer() > 3.0f && this.character.statesSki == Character.StatesSki.Go)
            {
                this.character.statesSki = Character.StatesSki.Init;                
                readySetGoLabel.Text = "Go";                
            }
            else if (timerGamePlayController.GetTimer() > 4.0f && this.character.statesSki == Character.StatesSki.Init)
            {
                timerGamePlayController.StopTimer();
                timerGamePlayController.ResetTimer();
                readySetGoControl.Hide();
            }            
            if (this.character.statesSki == Character.StatesSki.Init)
            {
                timerController.StartTimer();
                DefineLaps();
                DefineEndRun();
                setScore = true;
            }
            if (this.character.statesSki == Character.StatesSki.Finish)
            {
                timerController.StopTimer();
                SetTimeScore();
                TimeToReset(delta);
                ResetSpeedSkating();
                ShowControlSkiSpeedSkatingScreen();
            }
            timerController.TimerRunning(delta);
            updateTimer();
            UpdateSpeedLabel();
            CheckResetDirectionArrow();
        }
        private void UpdateBiathlon(double delta)
        {
            timerGamePlayController.TimerRunning(delta);
            if (timerGamePlayController.GetTimer() > 1.0f && this.character.statesSki == Character.StatesSki.Ready)
            {
                this.character.statesSki = Character.StatesSki.Set;
                readySetGoControl.Show();
                readySetGoLabel.Text = "Ready";
            }
            else if (timerGamePlayController.GetTimer() > 2.0f && this.character.statesSki == Character.StatesSki.Set)
            {
                this.character.statesSki = Character.StatesSki.Go;
                readySetGoLabel.Text = "Set";
            }
            else if (timerGamePlayController.GetTimer() < 4.0f && timerGamePlayController.GetTimer() > 3.0f && this.character.statesSki == Character.StatesSki.Go)
            {
                this.character.statesSki = Character.StatesSki.Init;
                readySetGoLabel.Text = "Go";
            }
            else if (timerGamePlayController.GetTimer() > 4.0f && this.character.statesSki == Character.StatesSki.Init)
            {
                timerGamePlayController.StopTimer();
                timerGamePlayController.ResetTimer();
                readySetGoControl.Hide();
            }
            if (this.character.statesSki == Character.StatesSki.Init)
            {
                timerController.StartTimer();
                DefineEndRunBiathlon();                
            }
            if (this.character.statesSki == Character.StatesSki.Init && timerController.GetTimerModel.states == TimerModel.States.Stop)
            {                
                timerController.UnstopTimer();             
            }
            if (this.character.statesSki == Character.StatesSki.Shooting)
            {
                timerController.StopTimer();                
            }
            if (this.character.statesSki == Character.StatesSki.Finish)
            {
                setScore = true;
                timerController.StopTimer();
                SetTimeScoreBiathlon();
                TimeToReset(delta);
                ResetBiathlon();
                ShowControlBiathlonScreen();
            }
            timerController.TimerRunning(delta);
            updateTimer();
            UpdateSpeedLabel();            
        }
        private void UpdateLuge(double delta)
        {
            if (this.lugeSled is not null)
            {
                if (this.lugeSled.GetCharacter.statesSki == Character.StatesSki.Ready)
                {
                    readySetGoControl.Show();
                    readySetGoLabel.Text = "Impulse";
                    var impulse = this.lugeSled.CalculateImpulsePercent();
                    var sizeXspeed = rectXSize * (impulse / this.lugeSled.GetMaxImpulse);
                    impulseNinePatchRect.Size = new Vector2(sizeXspeed, impulseNinePatchRect.Size.Y);
                }
                else if (this.lugeSled.GetCharacter.statesSki == Character.StatesSki.Running && !LugeStatic.isEndRun)
                {
                    readySetGoControl.Hide();
                    ShowHideControlLugeImpulse(false);
                    timerController.StartTimer();
                }
                else if (this.lugeSled.GetCharacter.statesSki == Character.StatesSki.Running && LugeStatic.isEndRun)
                {
                    timerController.StopTimer();
                    timerGamePlayController.StartTimer();
                    timerGamePlayController.TimerRunning(delta);
                }
                if (this.lugeSled.GetCharacter.statesSki == Character.StatesSki.Running && LugeStatic.isEndRun && timerGamePlayController.GetTimer() > 3.0f)
                {
                    this.lugeSled.GetCharacter.statesSki = Character.StatesSki.Finish;
                    timerGamePlayController.StopTimer();
                    setScore = true;
                    ShowControlSkiSpeedSkatingScreen();
                    SetTimeScoreLuge();
                    this.lugeSled.GetCharacter.ShowHideFinishSessionScreen();
                    ShowHideControlLugeImpulse(true);
                    ResetLuge();
                    this.lugeSled.MovePlayerToStartPosition();
                }
                if (this.lugeSled.GetCharacter.statesSki == Character.StatesSki.Finish)
                {
                    timerController.ResetTimer();
                    timerGamePlayController.ResetTimer();
                    this.lugeSled.GetCharacter.statesSki = Character.StatesSki.Ready;
                    this.lugeSled.Pause(true);
                }
                timerController.TimerRunning(delta);
                updateTimerLuge();
                UpdateSpeedLabelLuge();
            }
            if (bobsleighSled is not null)
            {
                if (this.bobsleighSled.GetCharacter[0].statesSki == Character.StatesSki.Ready)
                {
                    readySetGoControl.Show();
                    readySetGoLabel.Text = "Impulse";
                    var impulse = this.bobsleighSled.CalculateImpulsePercent();
                    var sizeXspeed = rectXSize * (impulse / this.bobsleighSled.GetMaxImpulse);
                    impulseNinePatchRect.Size = new Vector2(sizeXspeed, impulseNinePatchRect.Size.Y);
                }
                else if (this.bobsleighSled.GetCharacter[0].statesSki == Character.StatesSki.Running)
                {
                    readySetGoControl.Hide();
                    timerController.StartTimer();
                    var impulse = this.bobsleighSled.CalculateImpulsePercent();
                    var sizeXspeed = rectXSize * (impulse / this.bobsleighSled.GetMaxImpulse);
                    impulseNinePatchRect.Size = new Vector2(sizeXspeed, impulseNinePatchRect.Size.Y);
                    if (timerController.GetTimer() > 4.0f)
                    {
                        this.bobsleighSled.GetCharacter[0].statesSki = Character.StatesSki.JumpInCarBobsleigh;
                        ShowHideControlLugeImpulse(false);                        
                    }
                    else
                    {
                        this.bobsleighSled.RunAnim();
                    }
                }
                else if (this.bobsleighSled.GetCharacter[0].statesSki == Character.StatesSki.JumpInCarBobsleigh)
                {
                    this.bobsleighSled.JumpInCar();
                    if (this.bobsleighSled.GetIsJumpFinished)
                    {
                        this.bobsleighSled.GetCharacter[0].statesSki = Character.StatesSki.RunningBobsleigh;                        
                    }
                }
                else if (this.bobsleighSled.GetCharacter[0].statesSki == Character.StatesSki.RunningBobsleigh && LugeStatic.isEndRun)
                {
                    timerController.StopTimer();
                    timerGamePlayController.StartTimer();
                    timerGamePlayController.TimerRunning(delta);                 
                }
                if (this.bobsleighSled.GetCharacter[0].statesSki == Character.StatesSki.RunningBobsleigh && LugeStatic.isEndRun && timerGamePlayController.GetTimer() > 3.0f)
                {
                    this.bobsleighSled.GetCharacter[0].statesSki = Character.StatesSki.Finish;
                    timerGamePlayController.StopTimer();
                    setScore = true;
                    ShowControlSkiSpeedSkatingScreen();
                    SetTimeScoreBobsleigh();
                    this.bobsleighSled.GetCharacter[0].ShowHideFinishSessionScreen();
                    ShowHideControlLugeImpulse(true);
                    ResetBobsleigh();
                    this.bobsleighSled.MovePlayerToStartPosition();
                }
                if (this.bobsleighSled.GetCharacter[0].statesSki == Character.StatesSki.Finish)
                {
                    timerController.ResetTimer();
                    timerGamePlayController.ResetTimer();
                    this.bobsleighSled.GetCharacter[0].statesSki = Character.StatesSki.Ready;
                    this.bobsleighSled.Pause(true);
                }
                timerController.TimerRunning(delta);
                updateTimerLuge();
                UpdateSpeedLabelLuge();
            }                      
        }
        private void UpdateSkiJump(double delta)
        {
            if (this.character.statesSki == Character.StatesSki.Ready)
            {
                readySetGoControl.Show();
                readySetGoLabel.Text = "Impulse";
                var impulse = this.character.CalculateImpulsePercent();
                var sizeXspeed = rectXSize * (impulse / this.character.GetMaxImpulse);
                impulseSkiJumpNinePatchRect.Size = new Vector2(sizeXspeed, impulseSkiJumpNinePatchRect.Size.Y);
                SkiJumpStatic.impulseScore = impulse;
            }
            else if (this.character.statesSki == Character.StatesSki.Running)
            {
                readySetGoControl.Hide();
                controlSkiJumpImpulse.Hide();
                this.character.statesSki = Character.StatesSki.SkiJumpingDown;
            }
            else if (this.character.statesSki == Character.StatesSki.SkiJumpingFinish)
            {
                CalculateScore();
                ShowControlSkiSpeedSkatingScreen();
                SetFinishTimeLabelSkiJump();                
                this.character.statesSki = Character.StatesSki.Finish;
            }
            else if (this.character.statesSki == Character.StatesSki.Finish)
            {
                ResetSkiJump();
            }
        }
        private void TimeToReset(double delta)
        {
            timerResetController.StartTimer();
            timerResetController.TimerRunning(delta);
            if (timerResetController.GetTimer() > 3.0f)
            {
                Reset();
                timerResetController.ResetTimer();
                character.ShowHideFinishSessionScreen();
            }            
        }
        public void SetCharacter(Character character)
        {            
            this.character = character;
            this.character.Position = initPosition;
            this.character.Rotation = initRotation;
            this.character.GetSetCharacterIdCountry = CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].Id;
            this.character.GenerateBodyColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit1BodyColor);            
            this.character.GenerateArmsColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit1ArmsColor);
            this.character.GenerateHandsAndHeadColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].SkinColor);
            this.character.GenerateLegsColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit1LegsColor);
            this.character.GenerateBotsColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].BootsColor);
            this.character.GenerateHairColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].HairColor);
            this.character.SetIsAI(false);
            characterCrossCountryList.Add(character);
        }
        public void SetCharacterCrossCountryAI(Character characterAI, Vector3 initPositionAI, Vector3 initRotationAI, int id)
        {
            Character characterObj = characterAI;
            characterObj.Position = initPositionAI;
            characterObj.Rotation = initRotationAI;
            characterObj.GetSetCharacterIdCountry = CountrySingleton.countryObjDTO.countryList[id].Id;
            characterObj.GenerateBodyColor(CountrySingleton.countryObjDTO.countryList[id].kit1BodyColor);
            characterObj.GenerateArmsColor(CountrySingleton.countryObjDTO.countryList[id].kit1ArmsColor);
            characterObj.GenerateHandsAndHeadColor(CountrySingleton.countryObjDTO.countryList[id].SkinColor);
            characterObj.GenerateLegsColor(CountrySingleton.countryObjDTO.countryList[id].kit1LegsColor);
            characterObj.GenerateBotsColor(CountrySingleton.countryObjDTO.countryList[id].BootsColor);
            characterObj.GenerateHairColor(CountrySingleton.countryObjDTO.countryList[id].HairColor);
            characterObj.SetIsAI(true);
            characterCrossCountryList.Add(characterObj);            
        }
        public void SetCharacter(LugeSled lugeSled)
        {
            this.lugeSled = lugeSled;
            this.lugeSled.Position = initPosition;
            this.lugeSled.Rotation = initRotation;
            this.lugeSled.GetCharacter.GenerateBodyColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit1BodyColor);
            this.lugeSled.GetCharacter.GenerateArmsColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit1ArmsColor);
            this.lugeSled.GetCharacter.GenerateHandsAndHeadColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].SkinColor);
            this.lugeSled.GetCharacter.GenerateLegsColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit1LegsColor);
            this.lugeSled.GetCharacter.GenerateBotsColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].BootsColor);
            this.lugeSled.GetCharacter.GenerateHairColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].HairColor);
        }
        public void SetCharacter(BobsleighSled bobsleighSled)
        {
            this.bobsleighSled = bobsleighSled;
            this.bobsleighSled.Position = initPosition;
            this.bobsleighSled.Rotation = initRotation;
            foreach (var character in this.bobsleighSled.GetCharacter)
            {
                character.GenerateBodyColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit1BodyColor);
                character.GenerateArmsColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit1ArmsColor);
                character.GenerateHandsAndHeadColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].SkinColor);
                character.GenerateLegsColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit1LegsColor);
                character.GenerateBotsColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].BootsColor);
                character.GenerateHairColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].HairColor);
            }            
        }
        public void SetBobsleigh()
        {
            this.bobsleighSled.GetBobsleigh.GenerateColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit1BodyColor);                        
        }
        public void Reset()
        {
            timerController.ResetTimer();
            timerGamePlayController.ResetTimer();
            timerGamePlayController.StartTimer();
            this.character.statesSki = Character.StatesSki.Ready;
            this.character.Position = initPosition;
            this.character.Rotation = initRotation;
            character.Reset();
            SkiStatic.Reset();
        }
        public void ResetSpeedSkating() 
        {
            SpeedSkatingStatic.Reset();
            ResetDirectionArrow();
            speedSkatingModel.laps = 0;
        }
        public void ResetBiathlon()
        {
            BiathlonStatic.Reset();
            ResetDirectionArrowBiathlon();
            character.Reset();
        }
        public void ResetSkiJump()
        {            
            SkiJumpStatic.Reset();
            setScore = true;
            this.character.Position = initPosition;
            this.character.Rotation = initRotation;
            this.character.statesSki = Character.StatesSki.Ready;
            this.character.ResetSkiJump();
            ShowHideControlSkiJumpImpulse(true);
            ShowHideControlSkiJump(true);
        }
        public void SetDefaultPositionRotation(Vector3 initPosition, Vector3 initRotation) 
        {
            this.initPosition = initPosition;
            this.initRotation = initRotation;
        }
        public void SetPauseScreen(Control pauseScreen)
        { 
            this.pauseScreen = pauseScreen;
            character.SetPauseScreen = this.pauseScreen;
        }
        public void SetPauseScreenLuge(Control pauseScreen)
        {
            this.pauseScreen = pauseScreen;
            lugeSled.GetCharacter.SetPauseScreen = this.pauseScreen;
            lugeSled.SetPauseScreen = this.pauseScreen;
        }
        public void SetPauseScreenBobsleigh(Control pauseScreen)
        {
            this.pauseScreen = pauseScreen;
            foreach (var bobsleighSled in bobsleighSled.GetCharacter)
            {
                bobsleighSled.SetPauseScreen = this.pauseScreen;
            }            
            bobsleighSled.SetPauseScreen = this.pauseScreen;
        }
        public void SetFinishSessionScreen(Control finishSessionSession)
        {
            this.finishSessionScreen = finishSessionSession;
            character.SetFinishSessionScreen = this.finishSessionScreen;
        }
        public void SetFinishSessionScreenLuge(Control finishSessionSession)
        {
            this.finishSessionScreen = finishSessionSession;
            lugeSled.GetCharacter.SetFinishSessionScreen = this.finishSessionScreen;
        }
        public void SetFinishSessionScreenBobsleigh(Control finishSessionSession)
        {
            this.finishSessionScreen = finishSessionSession;
            foreach (var bobsleighSled in bobsleighSled.GetCharacter)
            {
                bobsleighSled.SetFinishSessionScreen = this.finishSessionScreen;
            }            
        }
        public void SetControlSkiSpeedSkatingBiathlon(Control controlSkiSpeedSkatingBiathlon)
        {
            this.controlSkiSpeedSkatingBiathlon = controlSkiSpeedSkatingBiathlon;
            character.SetControlSkiSpeedSkatingBiathlon = this.controlSkiSpeedSkatingBiathlon;
        }
        public void SetControlSkiCrossCountry(Control controlSkiCrossCountry, Control controlSkiCrossCountryPosition, 
            Label[] crossCountryCountryPositionLabel,
            Label[] crossCountryCountryCodeLabel,
            TextureRect[] crossCountryCountryFlagTextureRect,
            Control finishResultSessionControl)
        {
            this.controlSkiCrossCountry = controlSkiCrossCountry;
            character.SetControlSkiCrossCountry = this.controlSkiCrossCountry;
            this.controlSkiCrossCountryPosition = controlSkiCrossCountryPosition;
            this.crossCountryCountryPositionLabel = crossCountryCountryPositionLabel;
            this.crossCountryCountryCodeLabel = crossCountryCountryCodeLabel;            
            this.crossCountryCountryFlagTextureRect = crossCountryCountryFlagTextureRect;
            this.finishResultSessionControl = finishResultSessionControl;
            CreateStandingsTable(this.finishResultSessionControl);

            SetStandingsTable();
        }
        private void CreateStandingsTable(Control finishResultSessionControl)
        {
            var finishResultSessionControlObj = finishResultSessionControl.GetChildren();
            foreach (var child in finishResultSessionControlObj)
            {
                if (child.Name.ToString().Contains("PlayerPos"))
                {
                    standingNode.Add(child);                    
                }                
            }            
        }
        private void SetStandingsTable()
        {
            for(int i = 0; i < standingNode.Count; i++) 
            {
                var standing = standingNode[i].GetChildren();
                foreach(var obj in standing)
                {
                    if (obj.Name.ToString().Contains("Pos"))
                    {
                        var posLabel = obj as Label;
                        posLabel.Text = (i + 1).ToString();
                    }
                    if (obj.Name.ToString().Contains("CountryCode"))
                    {
                        var codeLabel = obj as Label;
                        codeLabel.Text = CountrySingleton.countryObjDTO.countryList[i].Code;
                    }
                    if (obj.Name.ToString().Contains("CountryFlag"))
                    {
                        var flagLabel = obj as TextureRect;
                        Texture textureResource = GD.Load<Texture>(flagResource + CountrySingleton.countryObjDTO.countryList[i].Code + ".png");
                        Texture2D texture2D = textureResource as Texture2D;
                        flagLabel.Texture = texture2D;                        
                    }
                    if (obj.Name.ToString().Contains("Score"))
                    {
                        var scoreLabel = obj as Label;
                        scoreLabel.Hide();//<-
                    }
                }
            }
            //CountrySingleton.countryObjDTO.countryList
            //GD.Print(standingNode.Count);
        }
        public void SetControlBiathlon(Control controlBiathlon)
        {
            this.controlBiathlon = controlBiathlon;
            character.SetControlBiathlon = this.controlBiathlon;
        }
        public void SetControlSkiJumpImpulseHorizontal(Control controlSkiJumpImpulseHorizontal, Control windDirectionArrowHorizontal)
        {
            this.controlSkiJumpImpulseHorizontal = controlSkiJumpImpulseHorizontal;
            this.windDirectionArrowHorizontal = windDirectionArrowHorizontal;
            character.SetControlSkiJumpImpulseHorizontal = this.controlSkiJumpImpulseHorizontal;
            character.SetWindDirectionArrowHorizontal = this.windDirectionArrowHorizontal;
        }
        public void SetControlSkiJumpImpulseVertical(Control controlSkiJumpImpulseVertical, Control windDirectionArrowVertical)
        {
            this.controlSkiJumpImpulseVertical = controlSkiJumpImpulseVertical;
            this.windDirectionArrowVertical = windDirectionArrowVertical;
            character.SetControlSkiJumpImpulseVertical = this.controlSkiJumpImpulseVertical;
            character.SetWindDirectionArrowVertical = this.windDirectionArrowVertical;
        }
        public void SetControlSkiSpeedSkatingBiathlonLuge(Control controlSkiSpeedSkatingBiathlon, Control controlBiathlon)
        {
            this.controlSkiSpeedSkatingBiathlon = controlSkiSpeedSkatingBiathlon;
            this.controlBiathlon = controlBiathlon;
            lugeSled.SetControlSkiSpeedSkatingBiathlon = controlSkiSpeedSkatingBiathlon;
            lugeSled.SetControlBiathlon = controlBiathlon;            
            lugeSled.ShowHideControlLuge();
        }
        public void SetControlSkiSpeedSkatingBiathlonBobsleigh(Control controlSkiSpeedSkatingBiathlon, Control controlBiathlon)
        {
            this.controlSkiSpeedSkatingBiathlon = controlSkiSpeedSkatingBiathlon;
            this.controlBiathlon = controlBiathlon;
            bobsleighSled.SetControlSkiSpeedSkatingBiathlon = controlSkiSpeedSkatingBiathlon;
            bobsleighSled.SetControlBiathlon = controlBiathlon;
            bobsleighSled.ShowHideControlLuge();
        }
        public void UnPause()
        {
            character.UnPause();
        }
        public void UnPauseLuge()
        {
            if(lugeSled is not null)
                lugeSled.UnPause();
            if (bobsleighSled is not null)
                bobsleighSled.UnPause();
        }
        public void ShowHideFinishSessionScreen() 
        {
            character.ShowHideFinishSessionScreen();
        }
        public void ShowHideFinishSessionScreenLuge()
        {
            if (this.lugeSled is not null)
            {
                this.lugeSled.GetCharacter.ShowHideFinishSessionScreen();
                this.lugeSled.Pause(false);
            }
            if (this.bobsleighSled is not null)
            {
                this.bobsleighSled.GetCharacter[0].ShowHideFinishSessionScreen();
                this.bobsleighSled.Pause(false);
            }
        }
        public void ShowControlSkiSpeedSkatingScreen()
        {            
            controlSkiSpeedSkatingScreen.Show();
            controlBiathlonScreen.Hide();
        }
        public void ShowControlBiathlonScreen()
        {
            controlSkiSpeedSkatingScreen.Hide();
            controlBiathlonScreen.Show();
        }
        #endregion
        #region Timer
        public void SetTimerLabel(string prefabName, Label timeLabel)
        {
            if (prefabNameTimerList.ToList().Contains(prefabName))
            {                
                this.timeLabel = timeLabel;
            }            
        }
        public void updateTimer()
        {            
            if (this.character.statesSki != Character.StatesSki.Finish && this.character.statesSki != Character.StatesSki.Disqualified)
            {                         
                timeLabel.Text = TimeSpan.FromSeconds(timerController.GetTimer()).ToString("mm':'ss':'fff");
            }
            if (this.character.statesSki == Character.StatesSki.Disqualified)
            {
                timeLabel.Text = "DSQ";
            }            
        }
        public void updateTimerCrossCountry()
        {
            if (this.character.statesSki != Character.StatesSki.Finish)
            {
                controlSkiCrossCountryTime.Text = TimeSpan.FromSeconds(timerController.GetTimer()).ToString("mm':'ss':'fff");
            }            
        }
        public void updateTimerLuge()
        {
            if (this.lugeSled is not null)
            {
                if (this.lugeSled.GetCharacter.statesSki != Character.StatesSki.Finish && this.lugeSled.GetCharacter.statesSki != Character.StatesSki.Disqualified)
                {
                    timeLabel.Text = TimeSpan.FromSeconds(timerController.GetTimer()).ToString("mm':'ss':'fff");
                }
            }
            if (this.bobsleighSled is not null)
            {
                if (this.bobsleighSled.GetCharacter[0].statesSki != Character.StatesSki.Finish && this.bobsleighSled.GetCharacter[0].statesSki != Character.StatesSki.Disqualified)
                {
                    timeLabel.Text = TimeSpan.FromSeconds(timerController.GetTimer()).ToString("mm':'ss':'fff");
                }
            }
        }
        private void InitTimer()
        {
            timerController = new TimerController();
            timerController.Init();            
            timerGamePlayController = new TimerController();
            timerGamePlayController.Init();
            timerResetController = new TimerController();
            timerResetController.Init();
            timerGamePlayController.StartTimer();
        }
        private void SetTimeScore()
        {
            if (setScore)
            {
                gamePlayModel.currentTimeScore = timerController.GetTimer();
                if (gamePlayModel.bestTimeScore != 0.0f && gamePlayModel.currentTimeScore < gamePlayModel.bestTimeScore && this.character.statesSki != Character.StatesSki.Disqualified)
                {                    
                    gamePlayModel.bestTimeScore = timerController.GetTimer();                    
                }
                if (gamePlayModel.bestTimeScore == 0.0f && this.character.statesSki != Character.StatesSki.Disqualified)
                {
                    gamePlayModel.bestTimeScore = timerController.GetTimer();                                     
                }
                SetFinishTimeLabel();
                setScore = false;
            }            
        }
        private void SetTimeScoreBobsleigh()
        {
            if (setScore)
            {
                gamePlayModel.currentTimeScore = timerController.GetTimer();
                if (gamePlayModel.bestTimeScore != 0.0f && gamePlayModel.currentTimeScore < gamePlayModel.bestTimeScore &&
                    this.bobsleighSled.GetCharacter[0].statesSki != Character.StatesSki.Disqualified)
                {
                    gamePlayModel.bestTimeScore = timerController.GetTimer();
                }
                if (gamePlayModel.bestTimeScore == 0.0f && this.bobsleighSled.GetCharacter[0].statesSki != Character.StatesSki.Disqualified)
                {
                    gamePlayModel.bestTimeScore = timerController.GetTimer();
                }
                SetFinishTimeLabelBobsleigh();
                setScore = false;
            }
        }
        private void SetTimeScoreLuge()
        {
            if (setScore)
            {
                gamePlayModel.currentTimeScore = timerController.GetTimer();
                if (gamePlayModel.bestTimeScore != 0.0f && gamePlayModel.currentTimeScore < gamePlayModel.bestTimeScore &&
                    this.lugeSled.GetCharacter.statesSki != Character.StatesSki.Disqualified)
                {
                    gamePlayModel.bestTimeScore = timerController.GetTimer();
                }
                if (gamePlayModel.bestTimeScore == 0.0f && this.lugeSled.GetCharacter.statesSki != Character.StatesSki.Disqualified)
                {
                    gamePlayModel.bestTimeScore = timerController.GetTimer();
                }
                SetFinishTimeLabelLuge();
                setScore = false;
            }
        }
        private void SetTimeScoreSkiJump(float score)
        {
            if (setScore)
            {
                gamePlayModel.currentTimeScore = score;
                if (gamePlayModel.bestTimeScore != 0.0f && gamePlayModel.currentTimeScore > gamePlayModel.bestTimeScore)
                {
                    gamePlayModel.bestTimeScore = score;
                }
                if (gamePlayModel.bestTimeScore == 0.0f)
                {
                    gamePlayModel.bestTimeScore = score;
                }
                SetFinishTimeLabelSkiJump();
                setScore = false;
            }
        }
        private void SetFinishTimeLabelLuge()
        {
            if (gamePlayModel.bestTimeScore == 0.0f)
            {
                timeScoreBestLabelFinish.Text = ("--:--:---");
            }
            else
            {
                timeScoreBestLabelFinish.Text = TimeSpan.FromSeconds(gamePlayModel.bestTimeScore).ToString("mm':'ss':'fff");
            }
            if (this.lugeSled.GetCharacter.statesSki == Character.StatesSki.Disqualified)
            {
                timeScoreLastLabelFinish.Text = ("--:--:---");
            }
            else
            {
                timeScoreLastLabelFinish.Text = TimeSpan.FromSeconds(gamePlayModel.currentTimeScore).ToString("mm':'ss':'fff");
            }
        }
        private void SetFinishTimeLabelBobsleigh()
        {
            if (gamePlayModel.bestTimeScore == 0.0f)
            {
                timeScoreBestLabelFinish.Text = ("--:--:---");
            }
            else
            {
                timeScoreBestLabelFinish.Text = TimeSpan.FromSeconds(gamePlayModel.bestTimeScore).ToString("mm':'ss':'fff");
            }
            if (this.bobsleighSled.GetCharacter[0].statesSki == Character.StatesSki.Disqualified)
            {
                timeScoreLastLabelFinish.Text = ("--:--:---");
            }
            else
            {
                timeScoreLastLabelFinish.Text = TimeSpan.FromSeconds(gamePlayModel.currentTimeScore).ToString("mm':'ss':'fff");
            }
        }
        private void SetTimeScoreBiathlon()
        {            
            if (setScore)
            {
                gamePlayModel.biathlonCurrentTimeScore = timerController.GetTimer();
                gamePlayModel.shootErrors = character.GetErrors();
                gamePlayModel.currentTimeScore = gamePlayModel.biathlonCurrentTimeScore + (10.0 * gamePlayModel.shootErrors);
                if (gamePlayModel.currentTimeScore < gamePlayModel.bestTimeScore)
                {
                    gamePlayModel.bestTimeScore = gamePlayModel.currentTimeScore;                    
                }
                if (gamePlayModel.bestTimeScore == 0.0f)
                {
                    gamePlayModel.bestTimeScore = gamePlayModel.currentTimeScore;
                }
                SetFinishTimeLabelBiathlon();
                setScore = false;
            }
        }
        private void SetFinishTimeLabel()
        {
            if (gamePlayModel.bestTimeScore == 0.0f)
            {
                timeScoreBestLabelFinish.Text = ("--:--:---");
            }
            else
            {                
                timeScoreBestLabelFinish.Text = TimeSpan.FromSeconds(gamePlayModel.bestTimeScore).ToString("mm':'ss':'fff");
            }
            if (this.character.statesSki == Character.StatesSki.Disqualified)
            {
                timeScoreLastLabelFinish.Text = ("--:--:---");
            }
            else
            {
                timeScoreLastLabelFinish.Text = TimeSpan.FromSeconds(gamePlayModel.currentTimeScore).ToString("mm':'ss':'fff");
            }
        }
        private void SetFinishTimeLabelBiathlon()
        {
            if (gamePlayModel.bestTimeScore == 0.0f)
            {
                bestScoreLabel.Text = ("--:--:---");
            }
            else
            {
                bestScoreLabel.Text = TimeSpan.FromSeconds(gamePlayModel.bestTimeScore).ToString("mm':'ss':'fff");
            }
            errorsScoreLabel.Text = gamePlayModel.shootErrors.ToString();
            timeScoreLabel.Text = TimeSpan.FromSeconds(gamePlayModel.biathlonCurrentTimeScore).ToString("mm':'ss':'fff");
            lastScoreLabel.Text = TimeSpan.FromSeconds(gamePlayModel.currentTimeScore).ToString("mm':'ss':'fff");
        }
        private void SetFinishTimeLabelSkiJump()
        {
            if (gamePlayModel.bestTimeScore == 0.0f)
            {
                timeScoreBestLabelFinish.Text = ("0");
            }
            else
            {
                timeScoreBestLabelFinish.Text = gamePlayModel.bestTimeScore.ToString();
            }
            timeScoreLastLabelFinish.Text = gamePlayModel.currentTimeScore.ToString();
        }
        #endregion
        #region Speed
        public void SetSpeedLabel(string prefabName, NinePatchRect speedNinePatchRect)
        {
            if (prefabNameTimerList.ToList().Contains(prefabName))
                this.speedNinePatchRect = speedNinePatchRect;
        }
        public void UpdateSpeedLabel()
        {
            var sizeXspeed = 0.0f;
            if (character.GetMaxSpeed > 0.0f)
                sizeXspeed = rectXSize * (character.GetSpeed / character.GetMaxSpeed);
            speedNinePatchRect.Size = new Vector2(sizeXspeed, speedNinePatchRect.Size.Y);            
        }
        public void UpdateSpeedEnergyLabel()
        {
            var sizeXspeed = 0.0f;
            if (character.GetMaxSpeed > 0.0f)
                sizeXspeed = rectXSize * (character.GetSpeed / character.GetMaxSpeed);
            controlSkiCrossCountrySpeed.Size = new Vector2(sizeXspeed, controlSkiCrossCountrySpeed.Size.Y);

            var sizeXenergy = 0.0f;            
            sizeXenergy = rectXSize * (character.GetEnergy / character.GetMaxSpeed);
            controlSkiCrossCountryEnergy.Size = new Vector2(sizeXenergy, controlSkiCrossCountryEnergy.Size.Y);
        }
        public void UpdateSpeedLabelLuge()
        {
            if (this.lugeSled is not null)
            {
                var sizeXspeed = 0.0f;
                if (this.lugeSled.GetMaxSpeed > 0.0f)
                    sizeXspeed = rectXSize * ((float)this.lugeSled.GetSpeed / (float)this.lugeSled.GetMaxSpeed);
                speedNinePatchRect.Size = new Vector2(sizeXspeed, speedNinePatchRect.Size.Y);
            }
            if (this.bobsleighSled is not null)
            {
                var sizeXspeed = 0.0f;
                if (this.bobsleighSled.GetMaxSpeed > 0.0f)
                    sizeXspeed = rectXSize * ((float)this.bobsleighSled.GetSpeed / (float)this.bobsleighSled.GetMaxSpeed);
                speedNinePatchRect.Size = new Vector2(sizeXspeed, speedNinePatchRect.Size.Y);
            }            
        }
        public void SetImpulseLabel(NinePatchRect impulseNinePatchRect, NinePatchRect impulseSkiJumpNinePatchRect)
        {
            this.impulseNinePatchRect = impulseNinePatchRect;
            this.impulseSkiJumpNinePatchRect = impulseSkiJumpNinePatchRect;
        }
        #endregion
        #region Country
        public void SetCountryUI(string prefabName, Label countryCodeLabel, TextureRect countryFlagTextureRect)
        {
            if (prefabNameCountryList.ToList().Contains(prefabName))
                SetCountryUISki(countryCodeLabel, countryFlagTextureRect);
        }
        private void SetCountryUISki(Label countryCodeLabel, TextureRect countryFlagTextureRect)
        {            
            this.countryCodeLabel = countryCodeLabel;
            this.countryCodeLabel.Text = CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].Code;
            this.countryFlagTextureRect = countryFlagTextureRect;
            Texture textureResource = GD.Load<Texture>(flagResource + CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].Code + ".png");
            Texture2D texture2D = textureResource as Texture2D;
            this.countryFlagTextureRect.Texture = texture2D;
        }
        public void SetCountryUIFinishScreen(string prefabName, Label countryCodeLabel, TextureRect countryFlagTextureRect)
        {
            if (prefabNameCountryList.ToList().Contains(prefabName))
            {
                this.countryCodeLabelFinish = countryCodeLabel;
                this.countryCodeLabelFinish.Text = CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].Code;
                this.countryFlagTextureRectFinish = countryFlagTextureRect;
                Texture textureResource = GD.Load<Texture>(flagResource + CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].Code + ".png");
                Texture2D texture2D = textureResource as Texture2D;
                this.countryFlagTextureRectFinish.Texture = texture2D;
            }                        
        }
        public void SetTimeScoreBestLastLabelFinish(Label timeScoreBestLabelFinish, Label timeScoreLastLabelFinish)
        {
            this.timeScoreBestLabelFinish = timeScoreBestLabelFinish;
            this.timeScoreLastLabelFinish = timeScoreLastLabelFinish;
        }
        public void SetTimeScreenControl(Control controlSkiSpeedSkatingScreen, Control controlBiathlonScreen, Control controlLugeImpulse, 
            Control controlSkiJumpImpulse, Control controlSkiJump)
        {
            this.controlSkiSpeedSkatingScreen = controlSkiSpeedSkatingScreen;
            this.controlBiathlonScreen = controlBiathlonScreen;
            this.controlLugeImpulse = controlLugeImpulse;
            this.controlSkiJumpImpulse = controlSkiJumpImpulse;
            this.controlSkiJump = controlSkiJump;
        }
        public void ShowHideControlLugeImpulse(bool isShow)
        {
            if(isShow)
                this.controlLugeImpulse.Show();
            else
                this.controlLugeImpulse.Hide();
        }
        public void ShowHideControlSkiJumpImpulse(bool isShow)
        {
            if (isShow)
                this.controlSkiJumpImpulse.Show();
            else
                this.controlSkiJumpImpulse.Hide();
        }
        public void ShowHideControlSkiJump(bool isShow)
        {
            if (isShow)
                this.controlSkiJump.Show();
            else
                this.controlSkiJump.Hide();
        }
        public void ShowHideControlSkiJumpImpulseHorizontal(bool isShow)
        {
            if (isShow)
                this.controlSkiJumpImpulseHorizontal.Show();
            else
                this.controlSkiJumpImpulseHorizontal.Hide();
        }
        public void ShowHideControlSkiJumpImpulseVertical(bool isShow)
        {
            if (isShow)
                this.controlSkiJumpImpulseVertical.Show();
            else
                this.controlSkiJumpImpulseVertical.Hide();
        }        
        public void SetTimeScoreBestLastLabelFinishBiathlon(Label BestScoreLabel, Label TimeScoreLabel, Label ErrorsScoreLabel, Label LastScoreLabel)
        {
            this.bestScoreLabel = BestScoreLabel;
            this.timeScoreLabel = TimeScoreLabel;
            this.errorsScoreLabel = ErrorsScoreLabel;
            this.lastScoreLabel = LastScoreLabel;
        }
        #endregion
        #region Warning
        public void SetReadySetGoControl(string prefabName, Control readySetGoControl, Label readySetGoLabel)
        {
            if (prefabNameTimerList.ToList().Contains(prefabName))
            {
                this.readySetGoControl = readySetGoControl;
                this.readySetGoLabel = readySetGoLabel;
            }            
        }
        #endregion
        #region Ski
        public void SetCharacterSportSki(GateStartFinish gateStart, GateStartFinish gateFinish)
        {
            this.character.SetStartGate = gateStart.GetArea3D();
            this.character.SetFinishGate = gateFinish.GetArea3D();
            this.character.MoveAndReScaleCharacter(0);
            this.character.ShowHideSkiItems();
        }
        public void SetCharacterSportSkiCrossCountryAI(GateStartFinish gateStart, GateStartFinish gateFinish)
        {
            characterCrossCountryList[characterCrossCountryList.Count - 1].SetStartGate = gateStart.GetArea3D();
            characterCrossCountryList[characterCrossCountryList.Count - 1].SetFinishGate = gateFinish.GetArea3D();
            characterCrossCountryList[characterCrossCountryList.Count - 1].MoveAndReScaleCharacter(0);
            characterCrossCountryList[characterCrossCountryList.Count - 1].ShowHideSkiItems();
        }
        private void InitSki()
        {            
            InitTimer();
            SkiStatic.Reset();
            setScore = true;
        }
        #endregion
        #region Speed Skating
        public void SetCharacterSpeedSkating()
        {                        
            this.character.MoveAndReScaleCharacter(1);
            this.character.ShowHideSpeedSkatingItems();                        
        }
        private void InitSpeedSkating()
        {
            InitTimer();
            speedSkatingModel.laps = 0;
        }
        public void SetRailSpeedSkating(int startPointId, List<SpeedSkatingTrackDTO> speedSkatingTrackDTOList)
        {            
            this.character.SetRailSpeedSkating(startPointId, speedSkatingTrackDTOList);
        }
        public void CheckResetDirectionArrow()
        {
            if (SpeedSkatingStatic.resetArrowCount)
            {                
                foreach (var directionArrow in directionArrowList)
                {
                    directionArrow.enable = true;
                    directionArrow.playerScore = false;
                    directionArrow.SetBodyColor(0);
                }
                SpeedSkatingStatic.resetArrowCount = false;
            }
        }
        public void ResetDirectionArrow()
        {            
            foreach (var directionArrow in directionArrowList)
            {
                directionArrow.enable = true;
                directionArrow.playerScore = false;
                directionArrow.SetBodyColor(0);
            }
            SpeedSkatingStatic.resetArrowCount = false;            
        }
        public void ResetDirectionArrowBiathlon()
        {
            for (int i = 0; i < directionArrowBiathlonList.Count; i++)
            {
                for (int j = 0; j < directionArrowBiathlonList[i].Count; j++)
                {
                    directionArrowBiathlonList[i][j].enable = true;
                    directionArrowBiathlonList[i][j].playerScore = false;
                    directionArrowBiathlonList[i][j].SetBodyColor(3);
                }                    
            }            
        }
        public void SetDirectionArrowList(List<DirectionArrow> aDirectionArrowList)
        {
            directionArrowList = aDirectionArrowList;
        }
        public void SetDirectionArrowList(List<List<DirectionArrow>> aDirectionArrowList)
        {
            directionArrowBiathlonList = aDirectionArrowList;            
        }
        public void SetLapCount(int lapCount)
        {
            speedSkatingModel.lapCount = lapCount;
        }
        public void DefineLaps()
        {            
            if (SpeedSkatingStatic.isLapFinished)
            {
                speedSkatingModel.laps++;
                SpeedSkatingStatic.isLapFinished = false;                
            }                        
        }
        public void DefineEndRun()
        {
            if (speedSkatingModel.laps == speedSkatingModel.lapCount)
            {                
                this.character.statesSki = Character.StatesSki.Finish;
            }
        }
        #endregion
        #region Biathlon
        private void InitBiathlon()
        {
            InitTimer();            
        }
        
        public void SetCharacterBiathlon()
        {
            this.character.MoveAndReScaleCharacter(1);
            this.character.ShowHideSkiPoleBiathlonItems();
        }
        public void SetRailBiathlon(int startPointId, List<List<SpeedSkatingTrackDTO>> biathlonTrackDTOList)
        {
            this.character.SetRailBiathlon(startPointId, biathlonTrackDTOList);
        }
        public void SetBiathlonUILabels(Label shoots, Label errorLabelScore, Label windDirection, Control windDirectionArrow)
        {
            this.shoots = shoots;
            this.errorLabelScore = errorLabelScore;
            this.windDirection = windDirection;
            this.windDirectionArrow = windDirectionArrow;
        }
        public void DefineEndRunBiathlon()
        {
            if (BiathlonStatic.isLapFinished)
            {                
                this.character.statesSki = Character.StatesSki.Finish;
            }
        }
        #endregion
        #region Luge
        private void InitLuge()
        {
            InitTimer();
        }
        public void ResetLuge()
        {
            timerController.ResetTimer();
            timerGamePlayController.ResetTimer();
            this.lugeSled.Reset();            
            this.lugeSled.GetCharacter.Reset();
            this.lugeSled.MovePlayerReset();
            LugeStatic.Reset();
            var impulse = this.lugeSled.CalculateImpulsePercent();
            var sizeXspeed = rectXSize * (impulse / this.lugeSled.GetMaxImpulse);
            impulseNinePatchRect.Size = new Vector2(sizeXspeed, impulseNinePatchRect.Size.Y);
        }
        #endregion
        #region Bobsleigh
        public void ResetBobsleigh()
        {
            timerController.ResetTimer();
            timerGamePlayController.ResetTimer();
            this.bobsleighSled.Reset();            
            this.bobsleighSled.MovePlayerReset();
            LugeStatic.Reset();
            var impulse = this.bobsleighSled.CalculateImpulsePercent();
            var sizeXspeed = rectXSize * (impulse / this.bobsleighSled.GetMaxImpulse);
            impulseNinePatchRect.Size = new Vector2(sizeXspeed, impulseNinePatchRect.Size.Y);
        }
        #endregion
        #region Ski jump
        private void InitSkiJump()
        {
            InitTimer();            
        }
        public void SetCharacterSportSkiJump()
        {            
            this.character.MoveAndReScaleCharacter(3);
            this.character.ShowHideSkiJumpItems();
        }
        private void CalculateScore()
        {                                    
            float impulseScore = SkiJumpStatic.impulseScore;
            float flyingScore = (100.0f * (1.0f - ((SkiJumpStatic.fliyngScore.Sum() / SkiJumpStatic.fliyngScore.Count()) / character.GetMaxRotateX)));
            float landingScore = (100.0f * (1.0f - (System.Math.Abs((SkiJumpStatic.landingScore) / character.GetMaxRotateX))));
            float skiJumpFinal = (float)Math.Round((impulseScore + flyingScore + landingScore), 0);
            SetTimeScoreSkiJump(skiJumpFinal);            
        }
        #endregion
        #region Cross Country
        public void SetCrossCountryLabel(Label controlSkiCrossCountryTime, NinePatchRect controlSkiCrossCountrySpeed, NinePatchRect controlSkiCrossCountryEnergy)
        {
            this.controlSkiCrossCountryTime = controlSkiCrossCountryTime;
            this.controlSkiCrossCountrySpeed = controlSkiCrossCountrySpeed;
            this.controlSkiCrossCountryEnergy = controlSkiCrossCountryEnergy;
        }
        private void SaveAI()//!<- Save AI Path
        {
            if (this.character.GetIsFinished())
            {
                this.character.statesSki = Character.StatesSki.Finish;
                var crossCountryObjDTO = new CrossCountryObjDTO();
                crossCountryObjDTO.CrossCountryDTOList = this.crossCountryDTOList;
                SaveLoad.SaveData<CrossCountryObjDTO>(crossCountryObjDTO, "Data//CrossCountryAI.json");
            }
            else 
            {
                if (crossCountryDTOList.Where(x => x.id == this.character.GetSkiCrossCountryDistance()).Any())
                {
                    var index = crossCountryDTOList.FindIndex(x => x.id == this.character.GetSkiCrossCountryDistance());
                    crossCountryDTOList[index].position = this.character.GlobalPosition;
                    crossCountryDTOList[index].speed = this.character.GetSpeed;
                    crossCountryDTOList[index].isAccel = this.character.GetIsAccel();
                    crossCountryDTOList[index].isBreak = this.character.GetIsBreak();
                }
                else 
                {
                    var crossCountryDTO = new CrossCountryDTO()
                    {
                        id = this.character.GetSkiCrossCountryDistance(),
                        position = this.character.GlobalPosition,
                        speed = this.character.GetSpeed,
                        isAccel = this.character.GetIsAccel(),
                        isBreak = this.character.GetIsBreak()
                    };
                    crossCountryDTOList.Add(crossCountryDTO);
                }                
            }            
        }
        private void StartAI()
        {
            foreach (var crossCountryDTO in this.characterCrossCountryList)
            {
                if (crossCountryDTO.GetSetCharacterIdCountry != GameModeSingleton.country)
                {
                    crossCountryDTO.statesSki = Character.StatesSki.Running;
                }                
            }
        }        
        private void OrderCrossCountryPosition()
        {
            characterCrossCountryList = characterCrossCountryList.OrderByDescending(x => x.GetSkiCrossCountryDistance()).ToList();            
        }
        private void SetPlayerPosition()
        {
            crossCountryPlayerPosition = characterCrossCountryList.IndexOf(characterCrossCountryList.Select(x => x)
                .Where(x => x.GetSetCharacterIdCountry == GameModeSingleton.country).First());
        }
        private void SetFlagCrossCountry(string code, int index)
        {
            Texture textureResource = GD.Load<Texture>(flagResource + code + ".png");
            Texture2D texture2D = textureResource as Texture2D;
            this.crossCountryCountryFlagTextureRect[index].Texture = texture2D;
        }

        private void SetPlayerPositionUI()
        {
            if (crossCountryPlayerPosition == 0)
            {
                this.crossCountryCountryPositionLabel[0].Text = (crossCountryPlayerPosition + 1).ToString();
                this.crossCountryCountryPositionLabel[1].Text = (crossCountryPlayerPosition + 2).ToString();
                this.crossCountryCountryPositionLabel[2].Text = (crossCountryPlayerPosition + 3).ToString();

                this.crossCountryCountryCodeLabel[0].Text = CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[0].GetSetCharacterIdCountry - 1].Code;
                this.crossCountryCountryCodeLabel[1].Text = CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[1].GetSetCharacterIdCountry - 1].Code;
                this.crossCountryCountryCodeLabel[2].Text = CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[2].GetSetCharacterIdCountry - 1].Code;

                SetFlagCrossCountry(CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[0].GetSetCharacterIdCountry - 1].Code, 0);
                SetFlagCrossCountry(CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[1].GetSetCharacterIdCountry - 1].Code, 1);
                SetFlagCrossCountry(CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[2].GetSetCharacterIdCountry - 1].Code, 2);
            }
            else if (crossCountryPlayerPosition == characterCrossCountryList.Count - 1)
            {
                this.crossCountryCountryPositionLabel[0].Text = (characterCrossCountryList.Count - 2).ToString();
                this.crossCountryCountryPositionLabel[1].Text = (characterCrossCountryList.Count - 1).ToString();
                this.crossCountryCountryPositionLabel[2].Text = (characterCrossCountryList.Count).ToString();

                this.crossCountryCountryCodeLabel[0].Text =
                    CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[characterCrossCountryList.Count - 3].GetSetCharacterIdCountry - 1].Code;
                this.crossCountryCountryCodeLabel[1].Text =
                    CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[characterCrossCountryList.Count - 2].GetSetCharacterIdCountry - 1].Code;
                this.crossCountryCountryCodeLabel[2].Text = 
                    CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[characterCrossCountryList.Count - 1].GetSetCharacterIdCountry - 1].Code;

                SetFlagCrossCountry(CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[characterCrossCountryList.Count - 3].GetSetCharacterIdCountry - 1].Code, 0);
                SetFlagCrossCountry(CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[characterCrossCountryList.Count - 2].GetSetCharacterIdCountry - 1].Code, 1);
                SetFlagCrossCountry(CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[characterCrossCountryList.Count - 1].GetSetCharacterIdCountry - 1].Code, 2);
            }
            else
            {
                this.crossCountryCountryPositionLabel[0].Text = (crossCountryPlayerPosition).ToString();
                this.crossCountryCountryPositionLabel[1].Text = (crossCountryPlayerPosition + 1).ToString();
                this.crossCountryCountryPositionLabel[2].Text = (crossCountryPlayerPosition + 2).ToString();

                this.crossCountryCountryCodeLabel[0].Text =
                    CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[crossCountryPlayerPosition - 1].GetSetCharacterIdCountry - 1].Code;
                this.crossCountryCountryCodeLabel[1].Text =
                    CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[crossCountryPlayerPosition].GetSetCharacterIdCountry - 1].Code;
                this.crossCountryCountryCodeLabel[2].Text =
                    CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[crossCountryPlayerPosition + 1].GetSetCharacterIdCountry - 1].Code;

                SetFlagCrossCountry(CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[crossCountryPlayerPosition - 1].GetSetCharacterIdCountry - 1].Code, 0);
                SetFlagCrossCountry(CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[crossCountryPlayerPosition].GetSetCharacterIdCountry - 1].Code, 1);
                SetFlagCrossCountry(CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[crossCountryPlayerPosition + 1].GetSetCharacterIdCountry - 1].Code, 2);
            }
        }
        public void ShowHideControlSkiCrossCountryPosition(bool isShow)
        {
            if(isShow)
                this.controlSkiCrossCountryPosition.Show();
            else
                this.controlSkiCrossCountryPosition.Hide();
        }
        #endregion
        #region Get Set
        public Button GetSetGoToMainMenu
        {
            get
            {
                return gamePlayModel.goToMainMenu;
            }
            set
            {
                gamePlayModel.goToMainMenu = value;
            }
        }
        public Button GetSetReturnMenu
        {
            get
            {
                return gamePlayModel.returnMenu;
            }
            set
            {
                gamePlayModel.returnMenu = value;
            }
        }
        public Button GetSetResetMenu
        {
            get
            {
                return gamePlayModel.resetMenu;
            }
            set
            {
                gamePlayModel.resetMenu = value;
            }
        }
        public Button GetSetBackMenuFinishButton
        {
            get
            {
                return gamePlayModel.backMenuFinishButton;
            }
            set
            {
                gamePlayModel.backMenuFinishButton = value;
            }
        }
        public Button GetSetReturnFinishButton
        {
            get
            {
                return gamePlayModel.returnFinishButton;
            }
            set
            {
                gamePlayModel.returnFinishButton = value;
            }
        }                
        #endregion
    }
}
