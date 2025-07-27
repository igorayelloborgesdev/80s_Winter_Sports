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
        private string prefabName;
        private List<Character> iceHockeyTeam1 = new List<Character>();
        private List<Character> iceHockeyTeam2 = new List<Character>();
        private TextureRect texture2DCountry1 = null;
        private TextureRect texture2DCountry2 = null;
        private Label countryCode1 = null;
        private Label countryCode2 = null;
        private Toggle toggleIceHockeyKitTeam1 = new Toggle();
        private Toggle toggleIceHockeyKitTeam2 = new Toggle();
        private List<Button> kitTeam1 = new List<Button>();
        private List<Button> kitTeam2 = new List<Button>();
        private CanvasItem jersey1_1 = null;
        private CanvasItem jersey1_2 = null;
        private CanvasItem short1_1 = null;
        private CanvasItem jersey2_1 = null;
        private CanvasItem jersey2_2 = null;
        private CanvasItem short2_1 = null;
        private Control selectTeamSessionControlIceHockey = null;
        private Control iceHockeyControl = null;
        private NinePatchRect HUDBG = null;
        private TextureRect countryFlag = null;
        private Label countryCode = null;
        private Node3D camera3DIceHockey = null;
        private RigidBody3D puck = null;
        private IceHockeyGoal Goal1 = null;
        private IceHockeyGoal Goal2 = null;
        private NinePatchRect hockeyPower = null;
        private Node parentNode = null;
        private Control hockeyPowerControl = null;
        private Control iceHockeyScoreControl = null;
        private Control iceHockeyEndGameControl = null;
        private TextureRect countryFlagIceHockey1TextureRect = null;
        private TextureRect countryFlagIceHockey2TextureRect = null;
        private Label countryIceHockey1CodeLabel = null;
        private Label countryIceHockey2CodeLabel = null;
        private Label countryIceHockey1ScoreLabel = null;
        private Label countryIceHockey2ScoreLabel = null;
        private Label timerIceHockeyLabel = null;
        private bool isChangeIceHockeyScoreBoard = true;
        private TextureRect countryFlag1TextureRect = null;
        private TextureRect countryFlag2TextureRect = null;
        private Label countryCode1Label = null;
        private Label countryCode2Label = null;
        private Label countryCodeScore1Label = null;
        private Label countryCodeScore2Label = null;
        private Button playMenuButtonFinish = null;
        private Button backGamesMenuButtonFinish = null;
        public List<CountryResultDTO> countryResultDTOList = new List<CountryResultDTO>();
        public List<List<TextureRect>> flagsIceHockeyBracket = new List<List<TextureRect>>();
        public TextureRect countryFlagGold = null;
        public TextureRect countryFlagBronze = null;

        public Control controlKit = null;
        private TextureRect texture2DCountry1Games = null;
        private TextureRect texture2DCountry2Games = null;
        private Label countryCode1Games = null;
        private Label countryCode2Games = null;
        private Toggle toggleIceHockeyKitTeam1Games = new Toggle();
        private Toggle toggleIceHockeyKitTeam2Games = new Toggle();
        private List<Button> kitTeam1Games = new List<Button>();
        private List<Button> kitTeam2Games = new List<Button>();
        private CanvasItem jersey1_1Games = null;
        private CanvasItem jersey1_2Games = null;
        private CanvasItem short1_1Games = null;
        private CanvasItem jersey2_1Games = null;
        private CanvasItem jersey2_2Games = null;
        private CanvasItem short2_1Games = null;
        private bool isPlayerQualified = false;
        #endregion
        #region const
        private const float rectXSize = 225.0f;
        private const string flagResource = "res://Art//2d//flags//";
        private string[] prefabNameTimerList = { "skiTrack", "SpeedSkating", "Biathlon", "LugeBobsleigh", "Skijumping", "IceHockeyRink"};
        private string[] prefabNameCountryList = { "skiTrack", "SpeedSkating", "Biathlon", "LugeBobsleigh", "Skijumping", "IceHockeyRink" };
        private Vector3[] iceHockeyInitPosition = { 
            new Vector3 (0.0f, 0.45f, 9.0f),
            new Vector3 (0.75f, 0.45f, 1.25f),
            new Vector3 (-0.75f, 0.45f, 1.25f),
            new Vector3 (0.0f, 0.45f, 0.5f),
            new Vector3 (-1.275f, 0.45f, 0.5f),
            new Vector3 (1.275f, 0.45f, 0.5f)
        };
        private Vector3 iceHockeyInitRotation = new Vector3(0.0f, 0.07f, 0.0f);
        private double[] skillArray = new double[] { 0, 10, 8, 6, 4, 2 };
        private double[] skillDifficultArray = new double[] { 5, 3, 1 };
        private const float iceHockeyMaxTimer = 120.0f;        
        #endregion
        #region Methods
        public void Init(string prefabName)
        {
            this.prefabName = prefabName;
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
            if (prefabName == "IceHockeyRink")
                InitIceHockey();
        }
        public void Update(double delta, string prefabName, int levelId)
        {
            if (prefabName == "skiTrack")
            {
                if (levelId == 11)
                {
                    UpdateSkiCrossCountry(delta);
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
            if (prefabName == "IceHockeyRink")
                UpdateSkiIcehockey(delta);
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
            if (timerGamePlayController is null)
                return;
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
                if (GameModeSingleton.gameMode == 0)
                    ShowControlSkiSpeedSkatingScreen();
                else
                {
                    ShowHideStandingsTable(true);
                    countryResultDTOList[countryResultDTOList.FindIndex(x => x.id == GameModeSingleton.country)].score = gamePlayModel.bestTimeScore;
                    countryResultDTOList[countryResultDTOList.FindIndex(x => x.id == GameModeSingleton.country)].isFinished = 1;
                    countryResultDTOList = countryResultDTOList.OrderByDescending(x => x.isFinished).ThenBy(x => x.score).ToList();
                    SetStandingsTableGames();
                }
                    
            }
            if (SkiStatic.isCollided)
            {                
                this.character.statesSki = Character.StatesSki.Disqualified;                
                SetTimeScore();
                TimeToReset(delta);
                if (GameModeSingleton.gameMode == 0)
                    ShowControlSkiSpeedSkatingScreen();
                else
                {
                    ShowHideStandingsTable(true);                    
                    countryResultDTOList = countryResultDTOList.OrderByDescending(x => x.isFinished).ThenBy(x => x.score).ToList();
                    SetStandingsTableGames();
                }
            }
            timerController.TimerRunning(delta);
            updateTimer();
            UpdateSpeedLabel();
        }
        private void SetStandingsTableGames(bool isSkyJump = false)
        {
            var index = GamesSingleton.sportSingleton.FindIndex(x => x.id == GameModeSingleton.sport);
            GamesSingleton.sportSingleton[index].isFinished = true;
            for (int i = 0; i < 3; i++)
            {
                GamesSingleton.sportSingleton[index].results[i] = countryResultDTOList[i].id;
            }            
            for (int i = 0; i < standingNode.Count; i++)
            {
                var standing = standingNode[i].GetChildren();
                foreach (var obj in standing)
                {
                    if (obj.Name.ToString().Contains("Pos"))
                    {
                        var posLabel = obj as Label;
                        posLabel.Text = (i + 1).ToString();
                    }
                    if (obj.Name.ToString().Contains("CountryCode"))
                    {
                        var codeLabel = obj as Label;
                        codeLabel.Text = CountrySingleton.countryObjDTO.countryList[countryResultDTOList[i].id - 1].Code;
                    }
                    if (obj.Name.ToString().Contains("CountryFlag"))
                    {
                        var flagLabel = obj as TextureRect;
                        Texture textureResource = GD.Load<Texture>(flagResource +
                            CountrySingleton.countryObjDTO.countryList[countryResultDTOList[i].id - 1].Code + ".png");
                        Texture2D texture2D = textureResource as Texture2D;
                        flagLabel.Texture = texture2D;
                    }
                    if (obj.Name.ToString().Contains("Score"))
                    {
                        var scoreLabel = obj as Label;
                        scoreLabel.Text = isSkyJump ? ((int)countryResultDTOList[i].score).ToString() : TimeSpan.FromSeconds(countryResultDTOList[i].score).ToString("mm':'ss':'fff");
                    }
                }
            }
        }
        public void ShowControlFinishStandings()
        {
            controlSkiSpeedSkatingScreen.Hide();
            controlBiathlonScreen.Hide();
        }
        private void UpdateSkiCrossCountry(double delta)
        {            
            timerGamePlayController.TimerRunning(delta);
            if (this.character.statesSki == Character.StatesSki.Ready)
            {
                ShowHideStandingsTable(false);
            }
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
            else if (this.character.statesSki == Character.StatesSki.Running)
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
                            this.character.statesSki = Character.StatesSki.Finish;                            
                        }
                    }
                }
            }
            else if (this.character.statesSki == Character.StatesSki.Finish && timerGamePlayController.GetTimer() > 1.0f)
            {
                this.character.statesSki = Character.StatesSki.End;                
                characterCrossCountryList = characterCrossCountryList.OrderBy(x => x.GetIsRunFinished)
                    .OrderBy(x => x.GetSetScore).OrderByDescending(x => x.GetSetCurrentPoint).ToList();

                foreach (var charObj in characterCrossCountryList) 
                {
                    charObj.statesSki = Character.StatesSki.End;
                }
                this.character.OnlyPause();
                ShowHideStandingsTable(true);
                if (GameModeSingleton.gameMode == 1)                   
                {                    
                    var index = GamesSingleton.sportSingleton.FindIndex(x => x.id == GameModeSingleton.sport);
                    GamesSingleton.sportSingleton[index].isFinished = true;
                    for (int i = 0; i < 3; i++)
                    {
                        GamesSingleton.sportSingleton[index].results[i] = characterCrossCountryList[i].GetSetCharacterIdCountry;
                    }
                }
                SetStandingsTable();
            }
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
                if (GameModeSingleton.gameMode == 0)
                    ShowControlSkiSpeedSkatingScreen();
                else
                {
                    ShowHideStandingsTable(true);
                    countryResultDTOList[countryResultDTOList.FindIndex(x => x.id == GameModeSingleton.country)].score = gamePlayModel.bestTimeScore;
                    countryResultDTOList[countryResultDTOList.FindIndex(x => x.id == GameModeSingleton.country)].isFinished = 1;
                    countryResultDTOList = countryResultDTOList.OrderByDescending(x => x.isFinished).ThenBy(x => x.score).ToList();
                    SetStandingsTableGames();
                }
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
                if (GameModeSingleton.gameMode == 0)
                    ShowControlBiathlonScreen();
                else
                {
                    ShowHideStandingsTable(true);
                    countryResultDTOList[countryResultDTOList.FindIndex(x => x.id == GameModeSingleton.country)].score = gamePlayModel.bestTimeScore;
                    countryResultDTOList[countryResultDTOList.FindIndex(x => x.id == GameModeSingleton.country)].isFinished = 1;
                    countryResultDTOList = countryResultDTOList.OrderByDescending(x => x.isFinished).ThenBy(x => x.score).ToList();
                    SetStandingsTableGames();
                }
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
                    SetTimeScoreLuge();                    
                    if (GameModeSingleton.gameMode == 0)
                        ShowControlSkiSpeedSkatingScreen();
                    else
                    {
                        ShowHideStandingsTable(true);
                        countryResultDTOList[countryResultDTOList.FindIndex(x => x.id == GameModeSingleton.country)].score = gamePlayModel.bestTimeScore;
                        countryResultDTOList[countryResultDTOList.FindIndex(x => x.id == GameModeSingleton.country)].isFinished = 1;
                        countryResultDTOList = countryResultDTOList.OrderByDescending(x => x.isFinished).ThenBy(x => x.score).ToList();
                        SetStandingsTableGames();
                    }

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
                    SetTimeScoreBobsleigh();
                    if (GameModeSingleton.gameMode == 0)
                        ShowControlSkiSpeedSkatingScreen();
                    else
                    {
                        ShowHideStandingsTable(true);
                        countryResultDTOList[countryResultDTOList.FindIndex(x => x.id == GameModeSingleton.country)].score = gamePlayModel.bestTimeScore;
                        countryResultDTOList[countryResultDTOList.FindIndex(x => x.id == GameModeSingleton.country)].isFinished = 1;
                        countryResultDTOList = countryResultDTOList.OrderByDescending(x => x.isFinished).ThenBy(x => x.score).ToList();
                        SetStandingsTableGames();
                    }

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

                if (GameModeSingleton.gameMode == 0)
                    ShowControlSkiSpeedSkatingScreen();
                else
                {
                    ShowHideStandingsTable(true);
                    countryResultDTOList[countryResultDTOList.FindIndex(x => x.id == GameModeSingleton.country)].score = gamePlayModel.bestTimeScore;
                    countryResultDTOList[countryResultDTOList.FindIndex(x => x.id == GameModeSingleton.country)].isFinished = 1;
                    countryResultDTOList = countryResultDTOList.OrderByDescending(x => x.isFinished).ThenByDescending(x => x.score).ToList();
                    SetStandingsTableGames(true);
                }
                
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
            this.character.GetSetInitPosition = initPosition;
            this.character.GetSetInitRotation = initRotation;
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
            characterObj.GetSetInitPosition = initPositionAI;
            characterObj.GetSetInitRotation = initRotationAI;
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
        public void SetIceHockeyCharacter(Character character, bool isTeam1 = true) 
        {
            character.HideAllNonIceHockeyItems();
            character.SetisPlayerTeam(isTeam1);
            character.SetisSelected(false);
            if (isTeam1)                            
                iceHockeyTeam1.Add(character);            
            else            
                iceHockeyTeam2.Add(character);                     
        }

        public void HideIceHockeyCharacter()
        {
            foreach (var obj in iceHockeyTeam1)
            {
                obj.Hide();
            }
            foreach (var obj in iceHockeyTeam2)
            {
                obj.Hide();
            }            
            for (int i = 0; i < iceHockeyTeam1.Count; i++)
            {
                iceHockeyTeam1[i].Show();                                
                iceHockeyTeam1[i].isSelected = false;
                iceHockeyTeam1[i].isPuckControl = false;
            }
            for (int i = 0; i < iceHockeyTeam2.Count; i++)
            {
                iceHockeyTeam2[i].Show();
                iceHockeyTeam2[i].isSelected = false;
                iceHockeyTeam2[i].isPuckControl = false;
            }
            iceHockeyTeam1[3].hockeyPower = this.hockeyPower;
            iceHockeyTeam1[3].parentNode = this.parentNode;
            iceHockeyTeam1[3].hockeyPowerControl = this.hockeyPowerControl;
            iceHockeyTeam1[3].hockeyPowerControl.Size = new Vector2(0.0f, 18.0f);
            iceHockeyTeam1[3].isSelected = true;
            iceHockeyTeam1[3].isPuckControl = true;            
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
        public void ResetCrossCountry()
        {
            timerController.ResetTimer();
            timerGamePlayController.StopTimer();
            timerGamePlayController.ResetTimer();
            timerGamePlayController.StartTimer();
            this.character.statesSki = Character.StatesSki.Ready;
            foreach (var obj in characterCrossCountryList)
            {
                obj.statesSki = StatesSki.Ready;
                obj.Position = obj.GetSetInitPosition;
                obj.Rotation = obj.GetSetInitRotation;
                obj.ResetCrossCountry();
            }            
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
        public void SetPauseScreenIceHockey(Control pauseScreen, ref Character character)
        {
            this.pauseScreen = pauseScreen;
            character.SetPauseScreen = this.pauseScreen;
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
                        codeLabel.Text = CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[i].GetSetCharacterIdCountry - 1].Code;
                    }
                    if (obj.Name.ToString().Contains("CountryFlag"))
                    {
                        var flagLabel = obj as TextureRect;
                        Texture textureResource = GD.Load<Texture>(flagResource + 
                            CountrySingleton.countryObjDTO.countryList[characterCrossCountryList[i].GetSetCharacterIdCountry - 1].Code + ".png");
                        Texture2D texture2D = textureResource as Texture2D;
                        flagLabel.Texture = texture2D;                        
                    }
                    if (obj.Name.ToString().Contains("Score"))
                    {
                        var scoreLabel = obj as Label;
                        scoreLabel.Hide();
                    }
                }
            }            
        }
        private void ShowHideStandingsTable(bool isShow)
        { 
            if(isShow)
                finishResultSessionControl.Show();
            else
                finishResultSessionControl.Hide();
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
            if(this.prefabName == "IceHockeyRink")
                iceHockeyTeam1[0].UnPause();
            else
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
                
                if (gamePlayModel.bestTimeScore > 0.0 &&
                    gamePlayModel.bestTimeScore < AISingleton.aiDTO.aiObjDTOList[AISingleton.aiDTO.aiObjDTOList.FindIndex(x => x.id == GameModeSingleton.sport)].score)
                {
                    AISingleton.aiDTO.aiObjDTOList[AISingleton.aiDTO.aiObjDTOList.FindIndex(x => x.id == GameModeSingleton.sport)].score = gamePlayModel.bestTimeScore;
                    SaveLoad.SaveData<AIDTO>(AISingleton.aiDTO, "Data//AIGen.json");
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
                if (gamePlayModel.bestTimeScore > 0.0 &&
    gamePlayModel.bestTimeScore < AISingleton.aiDTO.aiObjDTOList[AISingleton.aiDTO.aiObjDTOList.FindIndex(x => x.id == GameModeSingleton.sport)].score)
                {
                    AISingleton.aiDTO.aiObjDTOList[AISingleton.aiDTO.aiObjDTOList.FindIndex(x => x.id == GameModeSingleton.sport)].score = gamePlayModel.bestTimeScore;
                    SaveLoad.SaveData<AIDTO>(AISingleton.aiDTO, "Data//AIGen.json");
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
                if (gamePlayModel.bestTimeScore > 0.0 &&
    gamePlayModel.bestTimeScore < AISingleton.aiDTO.aiObjDTOList[AISingleton.aiDTO.aiObjDTOList.FindIndex(x => x.id == GameModeSingleton.sport)].score)
                {
                    AISingleton.aiDTO.aiObjDTOList[AISingleton.aiDTO.aiObjDTOList.FindIndex(x => x.id == GameModeSingleton.sport)].score = gamePlayModel.bestTimeScore;
                    SaveLoad.SaveData<AIDTO>(AISingleton.aiDTO, "Data//AIGen.json");
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

                if (gamePlayModel.bestTimeScore > 0.0 &&
                    gamePlayModel.bestTimeScore < AISingleton.aiDTO.aiObjDTOList[AISingleton.aiDTO.aiObjDTOList.FindIndex(x => x.id == GameModeSingleton.sport)].score)
                {
                    AISingleton.aiDTO.aiObjDTOList[AISingleton.aiDTO.aiObjDTOList.FindIndex(x => x.id == GameModeSingleton.sport)].score = gamePlayModel.bestTimeScore;
                    SaveLoad.SaveData<AIDTO>(AISingleton.aiDTO, "Data//AIGen.json");
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
            ShowHockeyPower(false);
            ShowHidehockeyScoreControl(false);
            ShowHideIceHockeyEndGameControl(false);
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
            ShowHockeyPower(false);
            ShowHidehockeyScoreControl(false);
            ShowHideIceHockeyEndGameControl(false);
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
            ShowHockeyPower(false);
            ShowHidehockeyScoreControl(false);
            ShowHideIceHockeyEndGameControl(false);
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
            ShowHockeyPower(false);
            ShowHidehockeyScoreControl(false);
            ShowHideIceHockeyEndGameControl(false);
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
            ShowHockeyPower(false);
            ShowHidehockeyScoreControl(false);
            ShowHideIceHockeyEndGameControl(false);
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
        #region Ice Hockey
        public void SetSelectionFlagsTexts(TextureRect texture2DCountry1, TextureRect texture2DCountry2, Label countryCode1, Label countryCode2)
        {
            this.texture2DCountry1 = texture2DCountry1;
            this.texture2DCountry2 = texture2DCountry2;
            this.countryCode1 = countryCode1;
            this.countryCode2 = countryCode2;
        }

        public void SetSelectionFlagsTextsGames(TextureRect texture2DCountry1, TextureRect texture2DCountry2, Label countryCode1, Label countryCode2)
        {
            this.texture2DCountry1Games = texture2DCountry1;
            this.texture2DCountry2Games = texture2DCountry2;
            this.countryCode1Games = countryCode1;
            this.countryCode2Games = countryCode2;
        }

        public void InitIceHockey()
        {
            InitTimer();
            Texture textureResource1 = GD.Load<Texture>(flagResource + CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].Code + ".png");
            Texture2D texture2D1 = textureResource1 as Texture2D;
            this.texture2DCountry1.Texture = texture2D1;
            Texture textureResource2 = GD.Load<Texture>(flagResource + CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam].Code + ".png");
            Texture2D texture2D2 = textureResource2 as Texture2D;
            this.texture2DCountry2.Texture = texture2D2;
            countryCode1.Text = CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].Code;
            countryCode2.Text = CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam].Code;

            if (GameModeSingleton.gameMode == 0)
            {
                ShowHideSelectTeamSessionControlIceHockey(true);
                ShowHideiceHockeyControl(false);
            }
            else
            {

                gamePlayModel.iceHockeyCountry = CountrySingleton.countryObjDTO.countryList.Select(x => x).ToList();
                gamePlayModel.iceHockeyCountry = gamePlayModel.iceHockeyCountry.OrderByDescending(x => x.sportSkill[GameModeSingleton.sport - 1]).ToList();
                gamePlayModel.pot1 = gamePlayModel.iceHockeyCountry.Take(4).ToList();
                gamePlayModel.pot2 = gamePlayModel.iceHockeyCountry.Skip(4).Take(4).ToList();
                gamePlayModel.pot3 = gamePlayModel.iceHockeyCountry.Skip(8).Take(8).ToList();
                for (int i = 0; i < 4; i++)
                {
                    int nextBracket = 0;
                    gamePlayModel.bracketList.Add(new List<Bracket>());
                    if (i == 0)
                    {
                        gamePlayModel.bracketList[i] = new List<Bracket>();
                        for (int j = 0; j < 8; j++)
                        {
                            gamePlayModel.bracketList[i].Add(new Bracket() { id = j, nextBracketId = nextBracket });
                            if(j % 2 != 0)
                                nextBracket++;
                        }
                    }
                    else if (i == 1)
                    {
                        gamePlayModel.bracketList[i] = new List<Bracket>();
                        for (int j = 0; j < 4; j++)
                        {
                            gamePlayModel.bracketList[i].Add(new Bracket() { id = j, nextBracketId = nextBracket });
                            if (j % 2 != 0)
                                nextBracket++;
                        }
                    }
                    else if (i == 2)
                    {
                        gamePlayModel.bracketList[i] = new List<Bracket>();
                        for (int j = 0; j < 2; j++)
                        {
                            gamePlayModel.bracketList[i].Add(new Bracket() { id = j, nextBracketId = nextBracket });
                            if (j % 2 != 0)
                                nextBracket++;
                        }
                    }
                    else if (i == 3)
                    {
                        gamePlayModel.bracketList[i] = new List<Bracket>();
                        for (int j = 0; j < 2; j++)
                        {
                            gamePlayModel.bracketList[i].Add(new Bracket() { id = j, nextBracketId = nextBracket });
                            if (j % 2 != 0)
                                nextBracket++;
                        }
                    }
                }
                var rng = new RandomNumberGenerator();
                rng.Randomize();
                for (int i = 0; i < 4; i++)
                {
                    int index = rng.RandiRange(0, gamePlayModel.pot1.Count - 1);
                    var sorteado = gamePlayModel.pot1[index];
                    gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][i * 2].team1 = sorteado.Id;
                    gamePlayModel.pot1.RemoveAt(index);
                }
                for (int i = 0; i < 4; i++)
                {
                    int index = rng.RandiRange(0, gamePlayModel.pot2.Count - 1);
                    var sorteado = gamePlayModel.pot2[index];
                    gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][(i * 2) + 1].team1 = sorteado.Id;
                    gamePlayModel.pot2.RemoveAt(index);
                }
                for (int i = 0; i < 8; i++)
                {
                    int index = rng.RandiRange(0, gamePlayModel.pot3.Count - 1);
                    var sorteado = gamePlayModel.pot3[index];
                    gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][i].team2 = sorteado.Id;
                    gamePlayModel.pot3.RemoveAt(index);
                }

                SetIceHockeyBracket();
                ShowHideSelectTeamSessionControlIceHockey(false);
                ShowHideiceHockeyControl(true);
            }
            
            readySetGoControl.Hide();
            finishSessionScreen.Hide();
            controlSkiSpeedSkatingBiathlon.Hide();
            controlBiathlon.Hide();
            windDirectionArrow.Hide();
            controlSkiSpeedSkatingScreen.Hide();
            controlBiathlonScreen.Hide();
            controlLugeImpulse.Hide();
            controlSkiJumpImpulse.Hide();
            controlSkiJump.Hide();            
            controlSkiJumpImpulseHorizontal.Hide();
            windDirectionArrowHorizontal.Hide();
            controlSkiJumpImpulseVertical.Hide();
            windDirectionArrowVertical.Hide();
            controlSkiCrossCountry.Hide();
            controlSkiCrossCountryPosition.Hide();
            finishResultSessionControl.Hide();
            HUDBG.Hide();
            countryFlag.Hide();
            countryCode.Hide();
            SetIceHockeyInitPosition();
            SetIceHockeyInitRotation();
            ShowHidehockeyScoreControl(false);
            ShowHideIceHockeyEndGameControl(false);
        }

        private void SetIceHockeyBracket()
        {                        
            try
            {
                SetFlagsToBrackets();
                ShowHideBracket();
                SimulateIceHockeyScore();
            }
            catch (Exception ex)
            {                
            }            
        }
        private void SetIceHockeyStandings()
        {
            var index = GamesSingleton.sportSingleton.FindIndex(x => x.id == GameModeSingleton.sport);
            GamesSingleton.sportSingleton[index].isFinished = true;
            GamesSingleton.sportSingleton[index].results[0] = gamePlayModel.bracketList[3][0].teamWinner;
            GamesSingleton.sportSingleton[index].results[1] = gamePlayModel.bracketList[3][0].teamWinner == gamePlayModel.bracketList[3][0].team1 ? gamePlayModel.bracketList[3][0].team2 : gamePlayModel.bracketList[3][0].team1;
            GamesSingleton.sportSingleton[index].results[2] = gamePlayModel.bracketList[3][1].teamWinner;
        }

        private void SetFlagsToBrackets()
        {
            if (gamePlayModel.iceHockeyRound == 4)
            {
                countryFlagGold.Show();
                Texture textureResourceGold = GD.Load<Texture>(flagResource +
                    CountrySingleton.countryObjDTO.countryList[gamePlayModel.bracketList[3][0].teamWinner - 1].Code + ".png");
                Texture2D texture2DGold = textureResourceGold as Texture2D;
                this.countryFlagGold.Texture = texture2DGold;

                countryFlagBronze.Show();
                Texture textureResourceBronze = GD.Load<Texture>(flagResource +
                    CountrySingleton.countryObjDTO.countryList[gamePlayModel.bracketList[3][1].teamWinner - 1].Code + ".png");
                Texture2D texture2DBronze = textureResourceBronze as Texture2D;
                this.countryFlagBronze.Texture = texture2DBronze;

                ShowHideControlKit(false);
                SetIceHockeyStandings();
            }
            else
            {
                int count = 0;
                int countbracket = 0;
                for (int i = 0; i < this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound].Count; i++)
                {
                    Texture textureResource1 = GD.Load<Texture>(flagResource +
                        CountrySingleton.countryObjDTO.countryList[gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][countbracket].team1 - 1].Code + ".png");
                    Texture2D texture2D1 = textureResource1 as Texture2D;
                    this.countryFlag1TextureRect.Texture = texture2D1;
                    this.flagsIceHockeyBracket[gamePlayModel.iceHockeyRound][count].Texture = this.countryFlag1TextureRect.Texture;
                    count++;

                    Texture textureResource2 = GD.Load<Texture>(flagResource +
                        CountrySingleton.countryObjDTO.countryList[gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][countbracket].team2 - 1].Code + ".png");
                    Texture2D texture2D2 = textureResource2 as Texture2D;
                    this.countryFlag2TextureRect.Texture = texture2D2;
                    this.flagsIceHockeyBracket[gamePlayModel.iceHockeyRound][count].Texture = this.countryFlag2TextureRect.Texture;
                    count++;
                    countbracket++;
                }
            }                        
        }
        private void SimulateIceHockeyScore()
        {
            if (gamePlayModel.iceHockeyRound < 4)
            {
                isPlayerQualified = false;
                for (int i = 0; i < this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound].Count; i++)
                {
                    if (GameModeSingleton.country != this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][i].team1 &&
                        GameModeSingleton.country != this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][i].team2)
                    {
                        var team1Skill = CountrySingleton.countryObjDTO.countryList.Where
                            (x => x.Id == this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][i].team1).First().sportSkill[GameModeSingleton.sport - 1];

                        var team2Skill = CountrySingleton.countryObjDTO.countryList.Where
                            (x => x.Id == this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][i].team2).First().sportSkill[GameModeSingleton.sport - 1];

                        Random rand = new Random();
                        float randomFloat1 = (float)(rand.NextDouble() * (float)team1Skill);
                        float randomFloat2 = (float)(rand.NextDouble() * (float)team2Skill);
                        var idWinner = randomFloat1 > randomFloat2 ?
                            this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][i].team1 : this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][i].team2;

                        this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][i].teamWinner = idWinner;

                        if (gamePlayModel.iceHockeyRound < 4)
                        {
                            if (gamePlayModel.iceHockeyRound == 2)
                            {
                                var idLoser = randomFloat1 < randomFloat2 ?
                                    this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][i].team1 : this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][i].team2;

                                if (this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][i].id % 2 == 0)
                                {
                                    this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound + 1][0].team1 = idWinner;
                                    this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound + 1][1].team1 = idLoser;
                                }
                                else
                                {
                                    this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound + 1][0].team2 = idWinner;
                                    this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound + 1][1].team2 = idLoser;
                                }
                            }
                            else if (gamePlayModel.iceHockeyRound < 2)
                            {
                                if (this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][i].id % 2 == 0)
                                {
                                    this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound + 1][
                                        this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][i].nextBracketId].team1 = idWinner;
                                }
                                else
                                {
                                    this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound + 1][
                                        this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][i].nextBracketId].team2 = idWinner;
                                }
                            }
                        }

                    }
                    else
                    {
                        this.gamePlayModel.iceHockeyplayerBracket = i;
                        GameModeSingleton.countryOppositeHockeyTeam = GameModeSingleton.country == this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][i].team1 ?
                                this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][i].team2 : this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][i].team1;
                        SetIceHockeyGamesUniformSetUp();
                        isPlayerQualified = true;
                    }

                }
            }
            else
            {
                
            }
        }

        private void SetIceHockeyGamesUniformSetUp()
        {            
            Texture textureResource1 = GD.Load<Texture>(flagResource + CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].Code + ".png");
            Texture2D texture2D1 = textureResource1 as Texture2D;
            this.texture2DCountry1Games.Texture = texture2D1;
            countryCode1Games.Text = CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].Code;
            SetIceHockeyKitEventGames(0, 0);

            Texture textureResource2 = GD.Load<Texture>(flagResource + CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam - 1].Code + ".png");
            Texture2D texture2D2 = textureResource2 as Texture2D;
            this.texture2DCountry2Games.Texture = texture2D2;
            countryCode2Games.Text = CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam - 1].Code;
            SetIceHockeyKitEventGames(0, 1);            
        }

        private void ShowHideBracket()
        {
            if (gamePlayModel.iceHockeyRound < 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i > gamePlayModel.iceHockeyRound)
                    {
                        for (int j = 0; j < this.flagsIceHockeyBracket[i].Count; j++)
                        {
                            this.flagsIceHockeyBracket[i][j].Hide();
                        }
                    }
                    else
                    {
                        for (int j = 0; j < this.flagsIceHockeyBracket[i].Count; j++)
                        {
                            this.flagsIceHockeyBracket[i][j].Show();
                        }
                    }

                }
                countryFlagGold.Hide();
                countryFlagBronze.Hide();
            }                    
        }

        private void SetIceHockeyInitPosition()
        {
            for (int i = 0; i < iceHockeyTeam1.Count; i++)
            {                
                iceHockeyTeam1[i].Position = iceHockeyInitPosition[iceHockeyTeam1[i].playerNumber];
            }
            for (int i = 0; i < iceHockeyTeam2.Count; i++)
            {
                iceHockeyTeam2[i].Position = new Vector3(
                    iceHockeyInitPosition[iceHockeyTeam2[i].playerNumber].X, 
                    iceHockeyInitPosition[iceHockeyTeam2[i].playerNumber].Y, 
                    -1.0f * iceHockeyInitPosition[iceHockeyTeam2[i].playerNumber].Z);
            }            
        }
        private void SetIceHockeyInitRotation()
        {            
            for (int i = 0; i < iceHockeyTeam1.Count; i++)
            {
                iceHockeyTeam1[i].LookAt(iceHockeyInitRotation);
                iceHockeyTeam1[i].RotateObjectLocal(Vector3.Up, Mathf.DegToRad(180.0f));
                iceHockeyTeam1[i].Rotation = new Vector3(0.0f, iceHockeyTeam1[i].Rotation.Y, 0.0f);
            }
            for (int i = 0; i < iceHockeyTeam2.Count; i++)
            {
                iceHockeyTeam2[i].LookAt(iceHockeyInitRotation);
                iceHockeyTeam2[i].RotateObjectLocal(Vector3.Up, Mathf.DegToRad(180.0f));
                iceHockeyTeam2[i].Rotation = new Vector3(0.0f, iceHockeyTeam2[i].Rotation.Y, 0.0f);
            }
        }
        public void SelectIceHockeyTeam2(int id)
        {
            GameModeSingleton.countryOppositeHockeyTeam = id;
            Texture textureResource2 = GD.Load<Texture>(flagResource + CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam].Code + ".png");
            Texture2D texture2D2 = textureResource2 as Texture2D;
            this.texture2DCountry2.Texture = texture2D2;
            countryCode2.Text = CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam].Code;
            SetIceHockeyKitEvent(0, 1);
        }
        public void SetIceHockeyKitButton(List<Button> kitTeam1, List<Button> kitTeam2)
        { 
            this.kitTeam1 = kitTeam1;
            this.kitTeam2 = kitTeam2;
            toggleIceHockeyKitTeam1.SetButtonsListBuilder(this.kitTeam1);
            toggleIceHockeyKitTeam2.SetButtonsListBuilder(this.kitTeam2);

            toggleIceHockeyKitTeam1.ToggleButton(0);
            toggleIceHockeyKitTeam2.ToggleButton(0);
        }
        public void SetIceHockeyKit(CanvasItem jersey1_1, CanvasItem jersey1_2, CanvasItem short1_1, CanvasItem jersey2_1, CanvasItem jersey2_2, CanvasItem short2_1)
        {
            this.jersey1_1 = jersey1_1;
            this.jersey1_2 = jersey1_2;
            this.short1_1 = short1_1;
            this.jersey2_1 = jersey2_1;
            this.jersey2_2 = jersey2_2;
            this.short2_1 = short2_1;
        }

        public void SetIceHockeyKitButtonGames(List<Button> kitTeam1, List<Button> kitTeam2)
        {
            this.kitTeam1Games = kitTeam1;
            this.kitTeam2Games = kitTeam2;
            toggleIceHockeyKitTeam1Games.SetButtonsListBuilder(this.kitTeam1Games);
            toggleIceHockeyKitTeam2Games.SetButtonsListBuilder(this.kitTeam2Games);

            toggleIceHockeyKitTeam1Games.ToggleButton(0);
            toggleIceHockeyKitTeam2Games.ToggleButton(0);
        }
        public void SetIceHockeyKitGames(CanvasItem jersey1_1, CanvasItem jersey1_2, CanvasItem short1_1, CanvasItem jersey2_1, CanvasItem jersey2_2, CanvasItem short2_1)
        {
            this.jersey1_1Games = jersey1_1;
            this.jersey1_2Games = jersey1_2;
            this.short1_1Games = short1_1;
            this.jersey2_1Games = jersey2_1;
            this.jersey2_2Games = jersey2_2;
            this.short2_1Games = short2_1;
        }
        public void SetIceHockeyKitEvent(int kitId, int teamId)
        {
            if (teamId == 0)
            {
                toggleIceHockeyKitTeam1.ToggleButton(kitId);
                SetKitColor(kitId, teamId);
            }
            else
            {
                toggleIceHockeyKitTeam2.ToggleButton(kitId);
                SetKitColor(kitId, teamId);
            }            
        }
        public void SetIceHockeyKitEventGames(int kitId, int teamId)
        {
            if (teamId == 0)
            {
                toggleIceHockeyKitTeam1Games.ToggleButton(kitId);
                SetKitColorGames(kitId, teamId);
            }
            else
            {
                toggleIceHockeyKitTeam2Games.ToggleButton(kitId);
                SetKitColorGames(kitId, teamId);
            }
        }
        private void SetKitColorGames(int kitId, int teamId)
        {
            if (teamId == 0)
            {
                jersey1_1Games.Modulate = new Color(kitId == 0 ? CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit2BodyColor : CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit3BodyColor);
                jersey1_2Games.Modulate = new Color(kitId == 0 ? CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit2ArmsColor : CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit3ArmsColor);
                short1_1Games.Modulate = new Color(kitId == 0 ? CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit2LegsColor : CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit3LegsColor);
            }
            else
            {
                jersey2_1Games.Modulate = new Color(kitId == 0 ? CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam - 1].kit2BodyColor : CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam - 1].kit3BodyColor);
                jersey2_2Games.Modulate = new Color(kitId == 0 ? CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam - 1].kit2ArmsColor : CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam - 1].kit3ArmsColor);
                short2_1Games.Modulate = new Color(kitId == 0 ? CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam - 1].kit2LegsColor : CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam - 1].kit3LegsColor);
            }
        }
        private void SetKitColor(int kitId, int teamId)
        {
            if (teamId == 0)
            {
                jersey1_1.Modulate = new Color(kitId == 0 ? CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit2BodyColor : CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit3BodyColor);
                jersey1_2.Modulate = new Color(kitId == 0 ? CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit2ArmsColor : CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit3ArmsColor);
                short1_1.Modulate = new Color(kitId == 0 ? CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit2LegsColor : CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].kit3LegsColor);
            }
            else
            {
                jersey2_1.Modulate = new Color(kitId == 0 ? CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam].kit2BodyColor : CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam].kit3BodyColor);
                jersey2_2.Modulate = new Color(kitId == 0 ? CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam].kit2ArmsColor : CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam].kit3ArmsColor);
                short2_1.Modulate = new Color(kitId == 0 ? CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam].kit2LegsColor : CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam].kit3LegsColor);
            }
        }
        public void SetSelectTeamSessionControlIceHockey(Control selectTeamSessionControlIceHockey)
        {
            this.selectTeamSessionControlIceHockey = selectTeamSessionControlIceHockey;
        }
        public void SetIceHockeyControl(Control iceHockeyControl)
        {
            this.iceHockeyControl = iceHockeyControl;
        }        
        public void SetAllControlsToHideIceHockey(Control readySetGoControl,
                                                Control finishSessionScreen,
                                                Control controlSkiSpeedSkatingBiathlon,
                                                Control controlBiathlon,
                                                Control windDirectionArrow,
                                                Control controlSkiSpeedSkatingScreen,
                                                Control controlBiathlonScreen,
                                                Control controlLugeImpulse,
                                                Control controlSkiJumpImpulse,
                                                Control controlSkiJump,
                                                Control controlSkiJumpImpulseHorizontal,
                                                Control windDirectionArrowHorizontal,
                                                Control controlSkiJumpImpulseVertical,
                                                Control windDirectionArrowVertical,
                                                Control controlSkiCrossCountry,
                                                Control controlSkiCrossCountryPosition,
                                                Control finishResultSessionControl,
                                                NinePatchRect HUDBG, TextureRect countryFlag, Label countryCode)
        {
            this.readySetGoControl = readySetGoControl;
            this.finishSessionScreen = finishSessionScreen;
            this.controlSkiSpeedSkatingBiathlon = controlSkiSpeedSkatingBiathlon;
            this.controlBiathlon = controlBiathlon;
            this.windDirectionArrow = windDirectionArrow;
            this.controlSkiSpeedSkatingScreen = controlSkiSpeedSkatingScreen;
            this.controlBiathlonScreen = controlBiathlonScreen;
            this.controlLugeImpulse = controlLugeImpulse;
            this.controlSkiJumpImpulse = controlSkiJumpImpulse;
            this.controlSkiJump = controlSkiJump;
            this.controlSkiJumpImpulseHorizontal = controlSkiJumpImpulseHorizontal;
            this.windDirectionArrowHorizontal = windDirectionArrowHorizontal;
            this.controlSkiJumpImpulseVertical = controlSkiJumpImpulseVertical;
            this.windDirectionArrowVertical = windDirectionArrowVertical;
            this.controlSkiCrossCountry = controlSkiCrossCountry;
            this.controlSkiCrossCountryPosition = controlSkiCrossCountryPosition;
            this.finishResultSessionControl = finishResultSessionControl;
            this.HUDBG = HUDBG;
            this.countryFlag = countryFlag;
            this.countryCode = countryCode;
        }
        public void SetHockeyPower(NinePatchRect hockeyPower)
        { 
            this.hockeyPower = hockeyPower;
        }
        public void SetHockeyPowerControl(Control hockeyPowerControl)
        {
            this.hockeyPowerControl = hockeyPowerControl; 
        }

        public void SetHockeyScoreControl(Control iceHockeyScoreControl)
        {
            this.iceHockeyScoreControl = iceHockeyScoreControl;
        }

        public void SetIceHockeyEndGameControl(Control iceHockeyEndGameControl)
        {
            this.iceHockeyEndGameControl = iceHockeyEndGameControl;
        }

        private void ShowHockeyPower(bool isShow)
        {
            if (isShow)
            {
                this.hockeyPower.Show();
                return;
            }
            this.hockeyPower.Hide();
        }

        public void SetParentNode(Node parentNode)
        {
            this.parentNode = parentNode;
        }        
        public void PlayIceHockey()
        {
            SetIceHockeyPlayerColors();
            if (GameModeSingleton.gameMode == 0)
                ShowHideSelectTeamSessionControlIceHockey(false);
            else
                ShowHideiceHockeyControl(false);
            IceHockeyStatic.statesIceHockey = IceHockeyStatic.StatesIceHockey.Init;
            IceHockeySelectedPlayer();
            ShowHidehockeyScoreControl(true);
            SetIceHockeyScoreBoard();
        }
        private void SetIceHockeyScoreBoard()
        {
            if (GameModeSingleton.gameMode == 0)
            {
                Texture textureResource = GD.Load<Texture>(flagResource + CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].Code + ".png");
                Texture2D texture2D = textureResource as Texture2D;
                this.countryFlagIceHockey1TextureRect.Texture = texture2D;
                Texture textureResource2 = GD.Load<Texture>(flagResource + CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam].Code + ".png");
                Texture2D texture2D2 = textureResource2 as Texture2D;
                this.countryFlagIceHockey2TextureRect.Texture = texture2D2;

                this.countryIceHockey1CodeLabel.Text = CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].Code;
                this.countryIceHockey2CodeLabel.Text = CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam].Code;
                this.countryIceHockey1ScoreLabel.Text = (0).ToString();
                this.countryIceHockey2ScoreLabel.Text = (0).ToString();
            }
            else
            {
                Texture textureResource = GD.Load<Texture>(flagResource + CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].Code + ".png");
                Texture2D texture2D = textureResource as Texture2D;
                this.countryFlagIceHockey1TextureRect.Texture = texture2D;
                Texture textureResource2 = GD.Load<Texture>(flagResource + CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam - 1].Code + ".png");
                Texture2D texture2D2 = textureResource2 as Texture2D;
                this.countryFlagIceHockey2TextureRect.Texture = texture2D2;

                this.countryIceHockey1CodeLabel.Text = CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].Code;
                this.countryIceHockey2CodeLabel.Text = CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam - 1].Code;
                this.countryIceHockey1ScoreLabel.Text = (0).ToString();
                this.countryIceHockey2ScoreLabel.Text = (0).ToString();
            }
            
        }
        private void SetIceHockeyFinalBoard()
        {
            Texture textureResource = GD.Load<Texture>(flagResource + CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].Code + ".png");
            Texture2D texture2D = textureResource as Texture2D;
            this.countryFlag1TextureRect.Texture = texture2D;
            Texture textureResource2 = GD.Load<Texture>(flagResource + CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam].Code + ".png");
            Texture2D texture2D2 = textureResource2 as Texture2D;
            this.countryFlag2TextureRect.Texture = texture2D2;

            this.countryCode1Label.Text = CountrySingleton.countryObjDTO.countryList[GameModeSingleton.country - 1].Code;
            this.countryCode2Label.Text = CountrySingleton.countryObjDTO.countryList[GameModeSingleton.countryOppositeHockeyTeam].Code;
            this.countryCodeScore1Label.Text = (IceHockeyStatic.score1).ToString();
            this.countryCodeScore2Label.Text = (IceHockeyStatic.score2).ToString();
        }
        public void ResetIceHockey()
        {
            IceHockeyStatic.Reset();
            ShowHideSelectTeamSessionControlIceHockey(true);
            ShowHideIceHockeyEndGameControl(false);
        }        
        public void ShowHideSelectTeamSessionControlIceHockey(bool isShow)
        {
            if(isShow)
                this.selectTeamSessionControlIceHockey.Show();
            else
                this.selectTeamSessionControlIceHockey.Hide();
        }
        public void ShowHideiceHockeyControl(bool isShow)
        {
            if (isShow)
                this.iceHockeyControl.Show();
            else
                this.iceHockeyControl.Hide();
        }
        private void SetIceHockeyPlayerColors()
        {
            if (GameModeSingleton.gameMode == 0)
            {
                SetIceHockeyPlayerColorsTeams(iceHockeyTeam1, CountrySingleton.countryObjDTO, GameModeSingleton.country - 1,
                    jersey1_1.Modulate, jersey1_2.Modulate, short1_1.Modulate);
                SetIceHockeyPlayerColorsTeams(iceHockeyTeam2, CountrySingleton.countryObjDTO, GameModeSingleton.countryOppositeHockeyTeam,
                    jersey2_1.Modulate, jersey2_2.Modulate, short2_1.Modulate);
            }
            else
            {
                SetIceHockeyPlayerColorsTeams(iceHockeyTeam1, CountrySingleton.countryObjDTO, GameModeSingleton.country - 1,
                        jersey1_1Games.Modulate, jersey1_2Games.Modulate, short1_1Games.Modulate);
                SetIceHockeyPlayerColorsTeams(iceHockeyTeam2, CountrySingleton.countryObjDTO, GameModeSingleton.countryOppositeHockeyTeam - 1,
                    jersey2_1Games.Modulate, jersey2_2Games.Modulate, short2_1Games.Modulate);
            }
            
        }
        private void SetIceHockeyPlayerColorsTeams(List<Character> characters, CountryObjDTO countryObjDTO, int id, Color jersey1, Color jersey2, Color short1)
        {
            foreach (var character in characters)
            {
                character.GenerateBodyColor(jersey1);
                character.GenerateArmsColor(jersey2);                
                character.GenerateLegsColor(short1);
                character.GenerateHandsAndHeadColor(countryObjDTO.countryList[id].SkinColor);
                character.GenerateBotsColor(countryObjDTO.countryList[id].BootsColor);
                character.GenerateHairColor(countryObjDTO.countryList[id].HairColor);                
            }
        }
        public void ShowHideIceHockeyStick(bool isShow)
        {
            character.ShowHideIceHockeyStick(isShow);
            character.ShowHideIceHockeyGoalKeeper(isShow);
        }
        private void UpdateSkiIcehockey(double delta)
        {
            if (IceHockeyStatic.statesIceHockey == IceHockeyStatic.StatesIceHockey.Select)
            {

            }
            if (IceHockeyStatic.statesIceHockey == IceHockeyStatic.StatesIceHockey.Init)
            {
                timerGamePlayController.TimerRunning(delta);
                if (timerGamePlayController.GetTimer() > 0.5f && IceHockeyStatic.statesIceHockeyStart == IceHockeyStatic.StatesIceHockeyStart.Ready)
                {
                    IceHockeyStatic.statesIceHockeyStart = IceHockeyStatic.StatesIceHockeyStart.Set;
                    readySetGoControl.Show();
                    readySetGoLabel.Text = "Ready";
                    DefineWhoIsControllingThePuck();
                }
                else if (timerGamePlayController.GetTimer() > 1.0f && IceHockeyStatic.statesIceHockeyStart == IceHockeyStatic.StatesIceHockeyStart.Set)
                {
                    IceHockeyStatic.statesIceHockeyStart = IceHockeyStatic.StatesIceHockeyStart.Go;
                    readySetGoControl.Show();
                    readySetGoLabel.Text = "Set";
                }
                else if (timerGamePlayController.GetTimer() > 1.5f && IceHockeyStatic.statesIceHockeyStart == IceHockeyStatic.StatesIceHockeyStart.Go)
                {
                    IceHockeyStatic.statesIceHockeyStart = IceHockeyStatic.StatesIceHockeyStart.InGame;
                    readySetGoControl.Show();
                    readySetGoLabel.Text = "Go";
                    timerGamePlayController.StopTimer();
                    timerGamePlayController.ResetTimer();
                    timerController.StartTimer();
                    timerController.UnstopTimer();
                }
                else if (IceHockeyStatic.statesIceHockeyStart == IceHockeyStatic.StatesIceHockeyStart.InGame)
                {
                    readySetGoControl.Hide();
                    SetIceHockeyTeams();
                    DefineWhoIsControllingThePuck();
                    timerController.TimerRunning(delta);
                    var regressiveTimer = iceHockeyMaxTimer - timerController.GetTimer();
                    if (regressiveTimer <= 0.0f && IceHockeyStatic.score1 != IceHockeyStatic.score2)
                    {
                        IceHockeyStatic.statesIceHockey = IceHockeyStatic.StatesIceHockey.Finish;
                        if (GameModeSingleton.gameMode == 0)
                            SetIceHockeyFinalBoard();
                        else
                        {
                            var idWinner = IceHockeyStatic.score1 > IceHockeyStatic.score2 ? GameModeSingleton.country : GameModeSingleton.countryOppositeHockeyTeam;
                            if (gamePlayModel.iceHockeyRound == 2)
                            {
                                var idLoser = IceHockeyStatic.score1 < IceHockeyStatic.score2 ? GameModeSingleton.country : GameModeSingleton.countryOppositeHockeyTeam;

                                if (this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][this.gamePlayModel.iceHockeyplayerBracket].id % 2 == 0)
                                {
                                    this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound + 1][0].team1 = idWinner;
                                    this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound + 1][1].team1 = idLoser;
                                }
                                else
                                {
                                    this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound + 1][0].team2 = idWinner;
                                    this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound + 1][1].team2 = idLoser;
                                }

                            }
                            else if (gamePlayModel.iceHockeyRound < 2)
                            {
                                this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][this.gamePlayModel.iceHockeyplayerBracket].teamWinner = idWinner;
                                if (gamePlayModel.iceHockeyRound < 4)
                                {
                                    if (this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][this.gamePlayModel.iceHockeyplayerBracket].id % 2 == 0)
                                    {
                                        this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound + 1][
                                            this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][this.gamePlayModel.iceHockeyplayerBracket].nextBracketId].team1 = idWinner;
                                    }
                                    else
                                    {
                                        this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound + 1][
                                            this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][this.gamePlayModel.iceHockeyplayerBracket].nextBracketId].team2 = idWinner;
                                    }
                                }
                            }
                            else if (gamePlayModel.iceHockeyRound == 3)
                            {
                                var idLoser = IceHockeyStatic.score1 < IceHockeyStatic.score2 ? GameModeSingleton.country : GameModeSingleton.countryOppositeHockeyTeam;                                
                                this.gamePlayModel.bracketList[gamePlayModel.iceHockeyRound][this.gamePlayModel.iceHockeyplayerBracket].teamWinner = idWinner;                                                                    
                            }

                            gamePlayModel.iceHockeyRound++;                            
                            SimulateIceHockeyScore();
                            SetFlagsToBrackets();
                            while (!isPlayerQualified && gamePlayModel.iceHockeyRound != 4)
                            {
                                gamePlayModel.iceHockeyRound++;
                                SimulateIceHockeyScore();
                                ShowHideBracket();
                                SetFlagsToBrackets();
                            }                            
                        }
                    }                        
                    this.timerIceHockeyLabel.Text = regressiveTimer >= 0.0f ? TimeSpan.FromSeconds(regressiveTimer).ToString("mm':'ss") : "G.Goal";
                }
            }
            if (IceHockeyStatic.statesIceHockey == IceHockeyStatic.StatesIceHockey.Goal && timerGamePlayController.GetTimer() <= 1.5f)
            {
                if (isChangeIceHockeyScoreBoard)
                {
                    isChangeIceHockeyScoreBoard = false;
                    if (IceHockeyStatic.isScorePlayer)
                    {
                        IceHockeyStatic.score1++;
                        this.countryIceHockey1ScoreLabel.Text = (IceHockeyStatic.score1).ToString();
                    }
                    else
                    {
                        IceHockeyStatic.score2++;
                        this.countryIceHockey2ScoreLabel.Text = (IceHockeyStatic.score2).ToString();
                    }
                }
                timerController.StopTimer();
                timerGamePlayController.StartTimer();
                timerGamePlayController.TimerRunning(delta);
            }
            else if (IceHockeyStatic.statesIceHockey == IceHockeyStatic.StatesIceHockey.Goal && timerGamePlayController.GetTimer() > 1.5f)
            {
                isChangeIceHockeyScoreBoard = true;
                ResetIceHockeyAfterGoal();
                timerGamePlayController.StopTimer();
                timerGamePlayController.ResetTimer();
                timerGamePlayController.StartTimer();
                IceHockeyStatic.statesIceHockey = IceHockeyStatic.StatesIceHockey.Init;
                IceHockeyStatic.statesIceHockeyStart = IceHockeyStatic.StatesIceHockeyStart.Ready;
            }
            if (IceHockeyStatic.statesIceHockey == IceHockeyStatic.StatesIceHockey.Finish)
            {
                timerGamePlayController.StopTimer();
                timerGamePlayController.ResetTimer();
                timerGamePlayController.StartTimer();
                timerController.StopTimer();
                timerController.ResetTimer();
                timerController.StartTimer();
                ShowHidehockeyScoreControl(false);
                if (GameModeSingleton.gameMode == 0)
                    ShowHideIceHockeyEndGameControl(true);
                else
                {
                    ShowHideBracket();
                    ShowHideiceHockeyControl(true);              
                }                
                ResetIceHockeyAfterGoal();
            }
            
        }
        private void ResetIceHockeyAfterGoal()
        {
            puck.Position = new Vector3(0.0f, 5.0f, 0.0f);
            foreach (var obj in iceHockeyTeam1)
            {
                obj.isPuckControl = false;
            }
            iceHockeyTeam1[3].isPuckControl = true;
            foreach (var obj in iceHockeyTeam2)
            {
                obj.isPuckControl = false;
            }
            SetIceHockeyInitPosition();            
        }

        private void DefineWhoIsControllingThePuck()
        {
            Character characterPuck = null;
            int playerIndex = -1;
            if (iceHockeyTeam1.Where(x => x.isPuckControl).Any())
            {
                characterPuck = iceHockeyTeam1.Where(x => x.isPuckControl).First();
                playerIndex = iceHockeyTeam1.FindIndex(x => x.isPuckControl);
                ShowHidehockeyPowerControl(true);                
            }
            if (iceHockeyTeam2.Where(x => x.isPuckControl).Any())
            {
                characterPuck = iceHockeyTeam2.Where(x => x.isPuckControl).First();
                playerIndex = iceHockeyTeam2.FindIndex(x => x.isPuckControl);
                ShowHidehockeyPowerControl(false);                
            }
            if (!iceHockeyTeam1.Where(x => x.isPuckControl).Any() && !iceHockeyTeam2.Where(x => x.isPuckControl).Any())
            {
                ShowHidehockeyPowerControl(false);                
            }
            if (characterPuck is not null)
            {                
                characterPuck.SetPuckRefPosition();                
            }
            ControlsCameraIceHockey(characterPuck);            
            SetHockeyPower(characterPuck, playerIndex);
        }        
        private void SetHockeyPower(Character characterPuck, int playerIndex)
        {
            if (characterPuck is not null)
            {                
                iceHockeyTeam1[playerIndex].hockeyPower = this.hockeyPower;
                iceHockeyTeam1[playerIndex].hockeyPower.Show();
                iceHockeyTeam1[playerIndex].parentNode = this.parentNode;
                iceHockeyTeam1[playerIndex].hockeyPowerControl = this.hockeyPowerControl;                
            }
        }

        private void ControlsCameraIceHockey(Character characterPuck)
        {
            if(characterPuck is not null)
                this.camera3DIceHockey.Position = new Vector3(this.camera3DIceHockey.Position.X, this.camera3DIceHockey.Position.Y, characterPuck.Position.Z + 3.5f);
            else
                this.camera3DIceHockey.Position = new Vector3(this.camera3DIceHockey.Position.X, this.camera3DIceHockey.Position.Y, this.puck.Position.Z + 3.5f);
            LimitCameraIceHockey();
        }
        private void LimitCameraIceHockey()
        { 
            if(this.camera3DIceHockey.Position.Z > 12.0f)
                this.camera3DIceHockey.Position = new Vector3(this.camera3DIceHockey.Position.X, this.camera3DIceHockey.Position.Y, 12.0f);
            if (this.camera3DIceHockey.Position.Z < -3.5f)
                this.camera3DIceHockey.Position = new Vector3(this.camera3DIceHockey.Position.X, this.camera3DIceHockey.Position.Y, -3.5f);
        }

        public void IceHockeySelectedPlayer()
        {
            foreach (var obj in iceHockeyTeam1)
            {
                obj.ShowHideIceHockeySeletion(obj.isSelected);
            }
        }
        public void SetPuckToCharacter(RigidBody3D puck)
        {
            foreach (var obj in iceHockeyTeam1)
            {
                obj.SetPuckOriginalTransform(puck);
            }
            foreach (var obj in iceHockeyTeam2)
            {
                obj.SetPuckOriginalTransform(puck);
            }            
            this.puck = puck;
        }
        public void SetCamera3DIceHockey(Node3D camera3DIceHockey)
        {
            this.camera3DIceHockey = camera3DIceHockey;
        }
        public void SetIceHockeyGoal(IceHockey iceHockey)
        {
            Goal1 = iceHockey.GetIceHockeyGoal(0);
            Goal2 = iceHockey.GetIceHockeyGoal(1);
            foreach (var obj in iceHockeyTeam1)
            {
                obj.SetIceHockeyGoal(Goal1, Goal2);                
            }
            foreach (var obj in iceHockeyTeam2)
            {
                obj.SetIceHockeyGoal(Goal1, Goal2);
            }
        }        
        public void SetIceHockeyTeams()
        {            
            foreach (var obj in iceHockeyTeam1)
            {                
                obj.SetIceHockeyTeams(ref iceHockeyTeam1, ref iceHockeyTeam2);                
            }
            foreach (var obj in iceHockeyTeam2)
            {
                obj.SetIceHockeyTeams(ref iceHockeyTeam1, ref iceHockeyTeam2);
            }            
        }
        public void ShowHidehockeyPowerControl(bool isShow)
        {            
            if (hockeyPowerControl is not null)
            {
                Control obj = hockeyPowerControl.GetParent<Control>();
                if (isShow)
                {
                    obj.Show();                    
                }
                else
                {
                    obj.Hide();                    
                }
                obj.Hide();
            }
        }
        public void ShowHidehockeyScoreControl(bool isShow)
        {
            if (isShow)
            {
                iceHockeyScoreControl.Show();
            }
            else
            {
                iceHockeyScoreControl.Hide();
            }
        }
        public void ShowHideIceHockeyEndGameControl(bool isShow)
        {
            if (isShow)
            {
                iceHockeyEndGameControl.Show();
            }
            else
            {
                iceHockeyEndGameControl.Hide();
            }
        }
        public void SetIceHockeyScoreBoard(TextureRect countryFlagIceHockey1TextureRect, TextureRect countryFlagIceHockey2TextureRect, 
            Label countryIceHockey1CodeLabel, Label countryIceHockey2CodeLabel, Label countryIceHockey1ScoreLabel, Label countryIceHockey2ScoreLabel, Label timerIceHockeyLabel)
        {
            this.countryFlagIceHockey1TextureRect = countryFlagIceHockey1TextureRect;
            this.countryFlagIceHockey2TextureRect = countryFlagIceHockey2TextureRect;
            this.countryIceHockey1CodeLabel = countryIceHockey1CodeLabel;
            this.countryIceHockey2CodeLabel = countryIceHockey2CodeLabel;
            this.countryIceHockey1ScoreLabel = countryIceHockey1ScoreLabel;
            this.countryIceHockey2ScoreLabel = countryIceHockey2ScoreLabel;
            this.timerIceHockeyLabel = timerIceHockeyLabel;
        }

        public void SetIceHockeyEndGame(TextureRect countryFlag1TextureRect, TextureRect countryFlag2TextureRect,
            Label countryCode1Label, Label countryCode2Label, Label countryCodeScore1Label, Label countryCodeScore2Label,
            Button playMenuButtonFinish)
        {
            this.countryFlag1TextureRect = countryFlag1TextureRect;
            this.countryFlag2TextureRect = countryFlag2TextureRect;
            this.countryCode1Label = countryCode1Label;
            this.countryCode2Label = countryCode2Label;
            this.countryCodeScore1Label = countryCodeScore1Label;
            this.countryCodeScore2Label = countryCodeScore2Label;            
            this.playMenuButtonFinish = playMenuButtonFinish;
        }

        public void SetAIResults()
        {
            if (GameModeSingleton.gameMode == 1)
            {
                if (GameModeSingleton.sport < 10)
                {
                    foreach (var obj in CountrySingleton.countryObjDTO.countryList)
                    {
                        countryResultDTOList.Add(new CountryResultDTO() { id = obj.Id, score = 0.0, isFinished = GameModeSingleton.country == obj.Id ? 0 : 1 });
                    }
                    foreach (var obj in countryResultDTOList)
                    {
                        if (obj.id != GameModeSingleton.country)
                        {
                            var skill = CountrySingleton.countryObjDTO.countryList.Where(x => x.Id == obj.id).First().sportSkill[GameModeSingleton.sport - 1];
                            var skillPerc = skillArray[skill];
                            var score = AISingleton.aiDTO.aiObjDTOList.Where(x => x.id == GameModeSingleton.sport).First().score;
                            var maxScore = ((skillPerc * score) / 100.0) * skillDifficultArray[GameModeSingleton.difficult];
                            double randomScore = GD.Randf() * maxScore;
                            obj.score = score + randomScore;
                        }
                    }
                    countryResultDTOList = countryResultDTOList.OrderByDescending(x => x.isFinished).ThenBy(x => x.score).ToList();
                }
                else if (GameModeSingleton.sport == 10)
                {
                    foreach (var obj in CountrySingleton.countryObjDTO.countryList)
                    {
                        countryResultDTOList.Add(new CountryResultDTO() { id = obj.Id, score = 0.0, isFinished = GameModeSingleton.country == obj.Id ? 0 : 1 });
                    }
                    foreach (var obj in countryResultDTOList)
                    {
                        if (obj.id != GameModeSingleton.country)
                        {
                            var skill = CountrySingleton.countryObjDTO.countryList.Where(x => x.Id == obj.id).First().sportSkill[GameModeSingleton.sport - 1];
                            var skillPerc = skillArray[skill];
                            var score = 300;
                            var maxScore = ((skillPerc * score) / 100.0) * skillDifficultArray[GameModeSingleton.difficult];
                            double randomScore = GD.Randf() * maxScore;
                            obj.score = score - randomScore;
                        }
                    }
                    countryResultDTOList = countryResultDTOList.OrderByDescending(x => x.isFinished).ThenBy(x => x.score).ToList();
                }
                else if (GameModeSingleton.sport == 11)
                {

                }
            }
        }
        public void ShowHideControlKit(bool isShow)
        { 
            if(isShow)
                controlKit.Show();
            else
                controlKit.Hide();
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
        public Button GetSetBackMenuFinishButtonStandings
        {
            get
            {
                return gamePlayModel.backMenuFinishButtonStandings;
            }
            set
            {
                gamePlayModel.backMenuFinishButtonStandings = value;
            }
        }
        public Button GetSetReturnFinishButtonStandings
        {
            get
            {
                return gamePlayModel.returnFinishButtonStandings;
            }
            set
            {
                gamePlayModel.returnFinishButtonStandings = value;
            }
        }
        public Button GetSetReturnBackGamesMenuButtonFinish
        {
            get
            {
                return this.backGamesMenuButtonFinish;
            }
            set
            {
                this.backGamesMenuButtonFinish = value;
            }
        }
        public List<Button> GetSetIceHockeyFlagSelectButton
        {
            get
            {
                return gamePlayModel.iceHockeyFlagSelectButton;
            }
            set
            {
                gamePlayModel.iceHockeyFlagSelectButton = value;
            }
        }
        public List<Button> GetSetKitTeam1
        {
            get
            {
                return kitTeam1;
            }
            set
            {
                kitTeam1 = value;
            }
        }
        public List<Button> GetSetKitTeam2
        {
            get
            {
                return kitTeam2;
            }
            set
            {
                kitTeam2 = value;
            }
        }
        public List<Button> GetSetKitTeam1Games
        {
            get
            {
                return kitTeam1Games;
            }
            set
            {
                kitTeam1Games = value;
            }
        }
        public List<Button> GetSetKitTeam2Games
        {
            get
            {
                return kitTeam2Games;
            }
            set
            {
                kitTeam2Games = value;
            }
        }
        public Button GetSetPlayMenuButtonFinish
        {
            get
            {
                return playMenuButtonFinish;
            }
            set
            {
                playMenuButtonFinish = value;
            }
        }

        public GamePlayModel GetGamePlayModel
        {
            get 
            {
                return gamePlayModel;
            }
        }
        #endregion
    }
}
