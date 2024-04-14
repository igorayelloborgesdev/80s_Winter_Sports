using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.Interfaces;
using WinterSports.Scripts.Model;
using WinterSports.Scripts.Prefabs;
using WinterSports.Scripts.Singleton;

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
        #endregion
        #region Variables        
        private Character character = null;
        private Vector3 initPosition = Vector3.Zero;
        private Vector3 initRotation = Vector3.Zero;
        private Control pauseScreen = null;
        private Label timeLabel = null;
        private NinePatchRect speedNinePatchRect = null;
        private Control readySetGoControl = null;
        private Label readySetGoLabel = null;
        private Label countryCodeLabel = null;
        private TextureRect countryFlagTextureRect = null;
        #endregion
        #region const
        private const float rectXSize = 225.0f;
        private const string flagResource = "res://Art//2d//flags//";
        #endregion
        #region Methods
        public void Init(string prefabName)
        {
            if (prefabName == "skiTrack")
                InitSki();
            if (prefabName == "SpeedSkating")
                InitSpeedSkating();//<-

        }
        public void Update(double delta)
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
            }                        
            if (this.character.statesSki == Character.StatesSki.Running)
            {
                timerController.StartTimer();
                readySetGoControl.Hide();
            }                
            if (this.character.statesSki == Character.StatesSki.Finish)
            {
                timerController.StopTimer();
                TimeToReset(delta);                
            }
            if (SkiStatic.isCollided)
            {
                this.character.statesSki = Character.StatesSki.Disqualified;                
                TimeToReset(delta);                
            }            
            timerController.TimerRunning(delta);
            updateTimer();
            UpdateSpeedLabel();
        }
        private void TimeToReset(double delta)
        {
            timerResetController.StartTimer();
            timerResetController.TimerRunning(delta);
            if (timerResetController.GetTimer() > 3.0f)
            {
                Reset();
                timerResetController.ResetTimer();
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
        public void UnPause()
        {
            character.UnPause();
        }
        #endregion
        #region Timer
        public void SetTimerLabel(string prefabName, Label timeLabel)
        {
            if (prefabName == "skiTrack")
                this.timeLabel = timeLabel;
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
        #endregion
        #region Speed
        public void SetSpeedLabel(string prefabName, NinePatchRect speedNinePatchRect)
        {
            if (prefabName == "skiTrack")
                this.speedNinePatchRect = speedNinePatchRect;
        }
        public void UpdateSpeedLabel()
        {
            var sizeXspeed = rectXSize * (character.GetSpeed / character.GetMaxSpeed);
            speedNinePatchRect.Size = new Vector2(sizeXspeed, speedNinePatchRect.Size.Y);            
        }
        #endregion
        #region Country
        public void SetCountryUI(string prefabName, Label countryCodeLabel, TextureRect countryFlagTextureRect)
        {
            if (prefabName == "skiTrack")
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
        #endregion
        #region Warning
        public void SetReadySetGoControl(string prefabName, Control readySetGoControl, Label readySetGoLabel)
        {
            if (prefabName == "skiTrack")
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
        #endregion
    }
}
