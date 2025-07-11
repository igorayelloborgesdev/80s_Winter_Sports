using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.Model;

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
        #endregion
    }
}
