using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.Model;

public class MainController
{
    #region Models
    MainModel mainModel = new MainModel();
    #endregion
    #region Variable
    Toggle toggleCountry = new Toggle();
    Toggle toggleSport = new Toggle();
    Toggle toggleDifficult = new Toggle();
    #endregion
    #region Get Set
    public string[] GetMenuControlsNameList
    {
        get
        {
            return mainModel.menuControlsNameList;
        }        
    }
    public string[] GetMenuButtonsNameList
    {
        get
        {
            return mainModel.menuButtonsNameList;
        }
    }
    public List<Control> GetSetMenuControlsNameList
    {
        set
        {
            mainModel.menuControlsList = value;
        }
        get 
        { 
            return mainModel.menuControlsList; 
        }
    }
    public List<Button> GetSetMenuButtonsList
    {
        set
        {
            mainModel.menuButtonsList = value;
        }
        get
        {
            return mainModel.menuButtonsList;
        }
    }    
    public string GetQuitName
    { 
        get { return mainModel.quitName; }
    }
    public Button GetSetQuitButton
    {
        set
        {
            mainModel.quitButton = value;
        }
        get
        {
            return mainModel.quitButton;
        }
    }
    public string GetMainScreenTitleLabel
    {
        get 
        { 
            return mainModel.mainScreenTitleLabel;
        }
    }
    public Label GetSetMainScreenTitle
    {
        get
        { 
            return mainModel.mainScreenTitle;
        }
        set
        {
            mainModel.mainScreenTitle = value;
        }
    }
    public string GetMainMenuModeTrainningButtonName
    {
        get
        {
            return mainModel.mainMenuModeTrainningButtonName;
        }
    }
    public string GetMainMenuModeGamesButtonName
    {
        get
        {
            return mainModel.mainMenuModeGamesButtonName;
        }
    }
    public Button GetSetMainMenuModeTrainningButton
    {
        get
        {
            return mainModel.mainMenuModeTrainningButton;
        }
        set
        {
            mainModel.mainMenuModeTrainningButton = value;
        }
    }
    public Button GetSetMainMenuModeGamesButton
    {
        get
        {
            return mainModel.mainMenuModeGamesButton;
        }
        set
        {
            mainModel.mainMenuModeGamesButton = value;
        }
    }
    public string[] GetMenuControlsGameModeTitleNameList
    {
        get
        {
            return mainModel.menuControlsGameModeTitleNameList;
        }
    }
    public string GetMainMenuModeCountrySelectionButtonName
    {
        get
        {
            return mainModel.mainMenuModeCountrySelectionButtonName;
        }
    }
    public List<Button> GetSetMenuModeCountryButtonsList
    {
        set
        {
            mainModel.menuModeCountryButtonsList = value;
        }
        get
        {
            return mainModel.menuModeCountryButtonsList;
        }
    }
    public string GetMainMenuGoToSportsButtonName
    {
        get
        {
            return mainModel.mainMenuGoToSportsButtonName;
        }
    }
    public Button GetSetMainMenuGoToSportsButton
    {
        get
        {
            return mainModel.mainMenuGoToSportsButton;
        }
        set
        {
            mainModel.mainMenuGoToSportsButton = value;
        }
    }
    public string GetGoToDifficultButtonName
    {
        get
        {
            return mainModel.goToDifficultButtonName;
        }
    }
    public string GetBackToCountryButtonName
    {
        get
        {
            return mainModel.backToCountryButtonName;
        }
    }
    public Button GetSetGoToDifficultButton
    {
        get
        {
            return mainModel.goToDifficultButton;
        }
        set
        {
            mainModel.goToDifficultButton = value;
        }
    }
    public Button GetSetBackToCountryButton
    {
        get
        {
            return mainModel.backToCountryButton;
        }
        set
        {
            mainModel.backToCountryButton = value;
        }
    }
    public string GetSportButtonName
    {
        get
        {
            return mainModel.sportButtonName;
        }
    }
    public List<Button> GetSetMenuModeSportButtonsList
    {
        set
        {
            mainModel.menuModeSportButtonsList = value;
        }
        get
        {
            return mainModel.menuModeSportButtonsList;
        }
    }
    public string GetGoToGamePlayButtonName
    {
        get
        {
            return mainModel.goToGamePlayButtonName;
        }
    }
    public string GetBackToSportButtonButtonName
    {
        get
        {
            return mainModel.backToSportButtonButtonName;
        }
    }
    public Button GetSetGoToGamePlayButton
    {
        get
        {
            return mainModel.goToGamePlayButton;
        }
        set
        {
            mainModel.goToGamePlayButton = value;
        }
    }
    public Button GetSetBackToSportButtonButton
    {
        get
        {
            return mainModel.backToSportButtonButton;
        }
        set
        {
            mainModel.backToSportButtonButton = value;
        }
    }
    public string GetDifficultButtonName
    {
        get
        {
            return mainModel.difficultButtonName;
        }
    }
    public List<Button> GetSetDifficultButtonButtonsList
    {
        set
        {
            mainModel.difficultButtonButtonsList = value;
        }
        get
        {
            return mainModel.difficultButtonButtonsList;
        }
    }
    public Control GetSetLoadingControl
    { 
        get 
        { 
            return mainModel.loadingControl; 
        }
        set 
        { 
            mainModel.loadingControl = value;
        }
    }
    #endregion
    #region Methods
    public void Init()
    {
        GameModeSingleton.country = 1;
        GameModeSingleton.sport = 1;
        GameModeSingleton.difficult = 1;
        ShowHidePanels(0);
        toggleCountry.SetButtonsListBuilder(mainModel.menuModeCountryButtonsList);
        toggleCountry.ToggleButton(0);
        toggleSport.SetButtonsListBuilder(mainModel.menuModeSportButtonsList);
        toggleSport.ToggleButton(0);
        toggleDifficult.SetButtonsListBuilder(mainModel.difficultButtonButtonsList);
        toggleDifficult.ToggleButton(1);
        ShowHideLoadingControl(false);
    }
    public void ShowHidePanels(int id)
    {     
        foreach (var menuControls in GetSetMenuControlsNameList)
        {            
            menuControls.Hide();
        }
        GetSetMenuControlsNameList[id].Show();        
    }
    public void SetTitleText(int id)
    {
        mainModel.mainScreenTitle.Text = mainModel.menuControlsTitleNameList[id];
    }
    public void ShowHideLoadingControl(bool show)
    {
        if(show)
            mainModel.loadingControl.Show();
        else
            mainModel.loadingControl.Hide();
    }
    #endregion
    #region Events
    public void SelectCountry(int id)
    {
        toggleCountry.ToggleButton(id - 1);
        GameModeSingleton.country = id;
    }
    public void SelectSport(int id)
    {     
        toggleSport.ToggleButton(id - 1);
        GameModeSingleton.sport = id;
    }
    public void SelectDiffcult(int id)
    {        
        toggleDifficult.ToggleButton(id);
    }
    #endregion
}
