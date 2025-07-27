using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Model;
using WinterSports.Scripts.Singleton;

namespace WinterSports.Scripts.Controller
{
    
    public class GamesController
    {
        #region Variables exports
        private Control quitControl = null;
        private Control saveControl = null;
        private Control saveWarningControl = null;
        private Control sportsControl = null;
        private Control medalTableControl = null;
        private List<Control> screenListControl = new List<Control>();
        private Button exitButton = null;
        private Button saveButton = null;
        private Button sportsButton = null;
        private Button medalTable = null;
        private Button quitButton = null;
        private Button backButton = null;
        private GamesModel gamesModel = null;
        public Control loadingControl = null;
        private Button saveExitButton = null;
        private List<SaveGameDTO> saveGameDTOList = new List<SaveGameDTO>();
        private Button saveWarnExitButton = null;
        private Label saveWarnExitLabel = null;
        #endregion
        #region const
        private const string flagResource = "res://Art//2d//flags//";
        #endregion
        #region Methods
        public void SetControls(Control quitControl, Control saveControl, Control saveWarningControl, Control sportsControl, Control medalTableControl)
        { 
            this.quitControl = quitControl;
            this.saveControl = saveControl;
            this.saveWarningControl = saveWarningControl;
            this.sportsControl = sportsControl;
            this.medalTableControl = medalTableControl;
            this.screenListControl.Add(this.quitControl);
            this.screenListControl.Add(this.saveControl);
            this.screenListControl.Add(this.saveWarningControl);
            this.screenListControl.Add(this.sportsControl);
            this.screenListControl.Add(this.medalTableControl);
        }
        public void SetQuitButtons(Button quitButton, Button backButton)
        {
            this.quitButton = quitButton;
            this.backButton = backButton;
        }
        public void SetButtons(Button exitButton, Button saveButton,
                                Button sportsButton, Button medalTable)
        {
            this.exitButton = exitButton;
            this.saveButton = saveButton;
            this.sportsButton = sportsButton;
            this.medalTable = medalTable;
        }
        public void ShowScreen(int id)
        {
            foreach (var obj in screenListControl)
            { 
                obj.Hide();
            }
            screenListControl[id].Show();
        }
        public void SetGamesModel(GamesModel gamesModel)
        {
            this.gamesModel = gamesModel;
        }

        public void HideAllMedalInfo()
        {
            for (int i = 0; i < this.gamesModel.flagGame.Count; i++)
            {
                this.gamesModel.flagGame[i][0].Hide();
                this.gamesModel.flagGame[i][1].Hide();
                this.gamesModel.flagGame[i][2].Hide();
                this.gamesModel.flagGameLabel[i][0].Hide();
                this.gamesModel.flagGameLabel[i][1].Hide();
                this.gamesModel.flagGameLabel[i][2].Hide();
            }            
        }
        public void SetMedalTable()
        {            
            for (int i = 0; i < CountrySingleton.countryObjDTO.countryList.Count; i++)
            {
                var country = new CountryMedalTableModel() { id = CountrySingleton.countryObjDTO.countryList[i].Id };
                this.gamesModel.countryMedalTableModel.Add(country);
            }
            foreach (var obj in GamesSingleton.sportSingleton)
            {
                if (obj.isFinished)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (j == 0)
                        {
                            this.gamesModel.countryMedalTableModel[this.gamesModel.countryMedalTableModel.FindIndex(x => x.id == obj.results[j])].gold++;
                        }
                        else if (j == 1)
                        {
                            this.gamesModel.countryMedalTableModel[this.gamesModel.countryMedalTableModel.FindIndex(x => x.id == obj.results[j])].silver++;
                        }
                        else if (j == 2)
                        {
                            this.gamesModel.countryMedalTableModel[this.gamesModel.countryMedalTableModel.FindIndex(x => x.id == obj.results[j])].bronze++;
                        }                        
                        
                    }                    
                }
            }
            foreach (var obj in this.gamesModel.countryMedalTableModel)
            {
                obj.total = obj.gold + obj.silver + obj.bronze;
            }
            gamesModel.countryMedalTableModel = gamesModel.countryMedalTableModel.OrderByDescending(x => x.bronze).OrderByDescending(x => x.silver).OrderByDescending(x => x.gold).ToList();
            SetMedalTableGraph();
        }
        private void SetMedalTableGraph()
        {
            for (int i = 0; i < gamesModel.countryMedalTableModel.Count; i++)
            {
                gamesModel.sportPosLabel[i].Text = (i + 1).ToString();
                gamesModel.sportGoldLabel[i].Text = gamesModel.countryMedalTableModel[i].gold.ToString();
                gamesModel.sportSilverLabel[i].Text = gamesModel.countryMedalTableModel[i].silver.ToString();
                gamesModel.sportBronzeLabel[i].Text = gamesModel.countryMedalTableModel[i].bronze.ToString();
                gamesModel.sportTotalLabel[i].Text = gamesModel.countryMedalTableModel[i].total.ToString();

                var index = CountrySingleton.countryObjDTO.countryList.FindIndex(x => x.Id == gamesModel.countryMedalTableModel[i].id);
                Texture textureResource = GD.Load<Texture>(flagResource +
                        CountrySingleton.countryObjDTO.countryList[index].Code + ".png");
                Texture2D texture2D = textureResource as Texture2D;
                this.gamesModel.countryTextureRect[i].Texture = texture2D;
                this.gamesModel.sportCodeLabel[i].Text = CountrySingleton.countryObjDTO.countryList[index].Code;
            }
        }

        public void SetResults()
        {
            for (int i = 0; i < this.gamesModel.flagGame.Count; i++)
            {
                if (GamesSingleton.sportSingleton[i].isFinished)
                {
                    this.gamesModel.flagGame[i][0].Show();
                    this.gamesModel.flagGame[i][1].Show();
                    this.gamesModel.flagGame[i][2].Show();
                    this.gamesModel.flagGameLabel[i][0].Show();
                    this.gamesModel.flagGameLabel[i][1].Show();
                    this.gamesModel.flagGameLabel[i][2].Show();
                    for (int j = 0; j < 3; j++)
                    {
                        var index = CountrySingleton.countryObjDTO.countryList.FindIndex(x => x.Id == GamesSingleton.sportSingleton[i].results[j]);
                        Texture textureResource = GD.Load<Texture>(flagResource +
                        CountrySingleton.countryObjDTO.countryList[index].Code + ".png");
                        Texture2D texture2D = textureResource as Texture2D;
                        this.gamesModel.flagGame[i][j].Texture = texture2D;
                        this.gamesModel.flagGameLabel[i][j].Text = CountrySingleton.countryObjDTO.countryList[index].Code;
                    }
                }                
            }
        }

        public void ShowHideLoadingControl(bool show)
        {
            if (show)
                this.loadingControl.Show();
            else
                this.loadingControl.Hide();
        }

        public void InitSaveGame()
        {
            saveGameDTOList = SaveLoad.CheckSaveGameFile<SaveGameDTO>();
            for (int i = 0; i < gamesModel.saveLabel.Count; i++)
            {
                gamesModel.saveLabel[i].Text = "EMPTY";
            }
            foreach (var obj in saveGameDTOList)
            {
                gamesModel.saveLabel[obj.id].Text = obj.dateTime.ToString();
            }
        }

        public bool SaveGame(int id)
        {
            try
            {
                var saveGameDTO = new SaveGameDTO();
                saveGameDTO.id = id;
                saveGameDTO.difficult = GameModeSingleton.difficult;
                saveGameDTO.country = GameModeSingleton.country;
                saveGameDTO.dateTime = DateTime.Now.ToString();
                saveGameDTO.sportSingleton = GamesSingleton.sportSingleton;
                var result = SaveLoad.SaveGameData<SaveGameDTO>(saveGameDTO, id.ToString());
                if (result)
                {
                    InitSaveGame();
                    this.saveWarnExitLabel.Text = "Game saved";
                }
                else 
                {
                    this.saveWarnExitLabel.Text = "ERROR!";
                }
                ShowScreen(2);
                return result;
            }catch (Exception e)
            {
                return false;
            }            
        }

        #endregion
        #region Get Set
        public Button GetExitButton
        { 
            get { return this.exitButton; }
            set { this.exitButton = value; }
        }
        public Button GetSaveButton
        {
            get { return this.saveButton; }
            set { this.saveButton = value; }
        }
        public Button GetSportsButton
        {
            get { return this.sportsButton; }
            set { this.sportsButton = value; }
        }
        public Button GetMedalTable
        {
            get { return this.medalTable; }
            set { this.medalTable = value; }
        }
        public Button GetQuitButton
        {
            get { return this.quitButton; }
            set { this.quitButton = value; }
        }
        public Button GetBackButton
        {
            get { return this.backButton; }
            set { this.backButton = value; }
        }
        public Control GetSetLoadingControl
        {
            get
            {
                return this.loadingControl;
            }
            set
            {
                this.loadingControl = value;
            }
        }
        public Button GetSetsaveExitButton
        {
            get 
            { 
                return this.saveExitButton; 
            }
            set 
            { 
                this.saveExitButton = value; 
            }
        }

        public Button GetSetsaveWarnExitButton
        {
            set { 
                this.saveWarnExitButton = value;
            }
            get {
                return saveWarnExitButton;
            }
        }
        public Label GetSetsaveWarnExitLabel
        {
            set
            {
                this.saveWarnExitLabel = value;
            }
            get
            {
                return saveWarnExitLabel;
            }
        }
        
        #endregion
    }
}
