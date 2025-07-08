using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MainModel
{
    #region Const
    public string[] menuControlsNameList = { "MainMenuControl", "QuitControl", "CreditsControl", "LoadControl", "ConfigControl", "TutorialControl"};
    public string[] menuButtonsNameList = { "MainMenuButton", "ExitButton", "CreditsButton", "LoadButton", "ConfigButton", "TutorialButton"};
    public string[] menuControlsTitleNameList = { "Main Menu", "Quit", "Credits", "Load", "Config", "Tutorial", "Country", "Difficult", "Sport" };
    public string quitName = "QuitControl/QuitButton";
    public string mainScreenTitleLabel = "MainScreenTitle/MainScreenTitleLabel";
    public string mainMenuModeTrainningButtonName = "MainMenuControl/MainMenuModeTrainningButton";
    public string mainMenuModeGamesButtonName = "MainMenuControl/MainMenuModeGamesButton";
    public string[] menuControlsGameModeTitleNameList = { "CountryControl", "DifficultControl", "SportSelectControl" };
    public string mainMenuModeCountrySelectionButtonName = "CountryControl/CountryButton";
    public string mainMenuGoToSportsButtonName = "CountryControl/GoToSportsButton";
    public string goToDifficultButtonName = "SportSelectControl/GoToDifficultButton";
    public string backToCountryButtonName = "SportSelectControl/BackToCountryButton";
    public string sportButtonName = "SportSelectControl/SportButton";
    public string goToGamePlayButtonName = "DifficultControl/GoToGamePlay";
    public string backToSportButtonButtonName = "DifficultControl/BackToSportButton";
    public string difficultButtonName = "DifficultControl/DifficultButton";    
    #endregion
    #region Variables
    public List<Control> menuControlsList = null;
    public List<Button> menuButtonsList = null;
    public Button quitButton = null;
    public Label mainScreenTitle = null;
    public Button mainMenuModeTrainningButton = null;
    public Button mainMenuModeGamesButton = null;
    public List<Button> menuModeCountryButtonsList = null;
    public Button mainMenuGoToSportsButton = null;
    public Button goToDifficultButton = null;
    public Button backToCountryButton = null;
    public List<Button> menuModeSportButtonsList = null;
    public Button goToGamePlayButton = null;
    public Button backToSportButtonButton = null;
    public List<Button> difficultButtonButtonsList = null;
    public Control loadingControl = null;
    #endregion
}
