using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Interfaces;
using WinterSports.Scripts.Model;
using WinterSports.Scripts.Prefabs;
using WinterSports.Scripts.Singleton;
using WinterSports.Scripts.Static;

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
        private Vector3 initPosition = Vector3.Zero;
        private Vector3 initRotation = Vector3.Zero;
        private Control pauseScreen = null;
        private Control finishSessionScreen = null;
        private Label timeLabel = null;
        private NinePatchRect speedNinePatchRect = null;
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
        #endregion
        #region const
        private const float rectXSize = 225.0f;
        private const string flagResource = "res://Art//2d//flags//";
        private string[] prefabNameTimerList = { "skiTrack", "SpeedSkating", "Biathlon" };
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
        }
        public void Update(double delta, string prefabName)
        {
            if (prefabName == "skiTrack")
                UpdateSki(delta);
            if (prefabName == "SpeedSkating")
                UpdateSpeedSkating(delta);
            if (prefabName == "Biathlon")
                UpdateBiathlon(delta);
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
            }
            if (SkiStatic.isCollided)
            {                
                this.character.statesSki = Character.StatesSki.Disqualified;                
                SetTimeScore();
                TimeToReset(delta);
            }
            timerController.TimerRunning(delta);
            updateTimer();
            UpdateSpeedLabel();
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
            }
            if (SkiStatic.isCollided)
            {
                this.character.statesSki = Character.StatesSki.Disqualified;
                SetTimeScore();
                TimeToReset(delta);
                ResetSpeedSkating();                
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
            }
            if (this.character.statesSki == Character.StatesSki.Shooting)
            {
                timerController.StopTimer();
            }
            if (this.character.statesSki == Character.StatesSki.Finish)
            {
                timerController.StopTimer();
                SetTimeScore();
                TimeToReset(delta);
                ResetBiathlon();
            }
            timerController.TimerRunning(delta);
            updateTimer();
            UpdateSpeedLabel();

            GD.Print(this.character.statesSki);
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
            this.character.GenerateBodyColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit1BodyColor);            
            this.character.GenerateArmsColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit1ArmsColor);
            this.character.GenerateHandsAndHeadColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].SkinColor);
            this.character.GenerateLegsColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit1LegsColor);
            this.character.GenerateBotsColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].BootsColor);
            this.character.GenerateHairColor(CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].HairColor);
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
            SpeedSkatingStatic.Reset();
            ResetDirectionArrowBiathlon();            
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
        public void SetFinishSessionScreen(Control finishSessionSession)
        {
            this.finishSessionScreen = finishSessionSession;
            character.SetFinishSessionScreen = this.finishSessionScreen;
        }
        public void UnPause()
        {
            character.UnPause();
        }
        public void ShowHideFinishSessionScreen() 
        {
            character.ShowHideFinishSessionScreen();
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
        #endregion
        #region Country
        public void SetCountryUI(string prefabName, Label countryCodeLabel, TextureRect countryFlagTextureRect)
        {
            if (prefabNameTimerList.ToList().Contains(prefabName))
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
            if (prefabNameTimerList.ToList().Contains(prefabName))
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
