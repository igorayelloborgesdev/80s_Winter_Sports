using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ConfigModel
{
    #region Const
    public string[] menuConfigNameList = { 
        "ConfigControl/ConfigPauseButton", 
        "ConfigControl/ConfigUpButton", 
        "ConfigControl/ConfigDownButton", 
        "ConfigControl/ConfigLeftButton", 
        "ConfigControl/ConfigRightButton", 
        "ConfigControl/ConfigButton1Button", 
        "ConfigControl/ConfigButton2Button" };
    public string[] menuConfigNameLabelList = { 
        "ConfigControl/ConfigPauseButton/ConfigPauseNinePatchRect/ConfigPauseTitleLabel",
        "ConfigControl/ConfigUpButton/ConfigUpNinePatchRect/ConfigUpButton",
        "ConfigControl/ConfigDownButton/ConfigDownNinePatchRect/ConfigDownButton",
        "ConfigControl/ConfigLeftButton/ConfigLeftNinePatchRect/ConfigLeftButton",
        "ConfigControl/ConfigRightButton/ConfigRightNinePatchRect/ConfigRightButton",
        "ConfigControl/ConfigButton1Button/ConfigButton1NinePatchRect/ConfigButton1Button",
        "ConfigControl/ConfigButton2Button/ConfigButton2NinePatchRect/ConfigButton2Button" };
    public string colorNormal = "009bf0";
    public string colorSelected = "e7a706";
    public string[] menuConfigInputNameLabelList = {
        "ConfigControl/ConfigButtonKeyboard",
        "ConfigControl/ConfigButtonJoystick"
    };
    public string configSaveButtonName = "ConfigControl/ConfigSaveButton";
    public string saveConfigModalTitle = "Save";
    public string saveConfigConfirmModalContext = "Config saved";
    public string saveConfigErrorModalContext = "Error";
    public string configRestoreButtonName = "ConfigControl/ConfigRestoreButton";
    #endregion
    #region Variables
    public string simpleModalTitleLabelName { get; set; }
    public string simpleModalContextLabelName { get; set; }
    public string simpleModalButtonName { get; set; }
    public List<Button> menuConfigList = null;
    public List<Button> menuConfigInputList = null;
    public List<Label> menuConfigLabelList = null;
    public bool isAssign = false;
    public int configId = -1;
    public KeyObj[] keysControlArray = new KeyObj[7];
    //0 - Pause
    //1 - Up
    //2 - Down
    //3 - Left
    //4 - Right
    //5 - Button 1
    //6 - Button 2
    public int keyboardJoystick = 0;
    //0 - Keyboard
    //1 - Joystick
    public Button configSaveButton = null;
    public Button configRestoreButton = null;
    #endregion
}