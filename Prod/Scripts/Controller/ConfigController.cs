using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ConfigController
{
    #region Models
    ConfigModel configModel = new ConfigModel();
    #endregion
    #region Variable
    Toggle toggleInputKeyboardJoystick = new Toggle();
    ModalSimple modalSimple = null;
    #endregion
    #region Builder
    public void ModalSimpleBuilder(ModalSimple aModalSimple)
    {
        modalSimple = aModalSimple;
    }
    #endregion
    #region Get Set
    public string[] GetMenuConfigNameList
    {
        get
        {
            return configModel.menuConfigNameList;
        }
    }    
    public List<Button> GetSetMenuConfigList
    {
        set
        {
            configModel.menuConfigList = value;
        }
        get
        {
            return configModel.menuConfigList;
        }
    }
    public string[] GetMenuConfigInputNameLabelList
    {
        get
        {
            return configModel.menuConfigInputNameLabelList;
        }
    }
    public List<Button> GetSetMenuConfigInputList
    {
        set
        {
            configModel.menuConfigInputList = value;
        }
        get
        {
            return configModel.menuConfigInputList;
        }
    }    
    public List<Label> GetSetMenuConfigLabel
    {
        set
        {
            configModel.menuConfigLabelList = value;
        }
        get
        {
            return configModel.menuConfigLabelList;
        }
    }
    public int GetSetKeyboardJoystick
    {
        set
        {
            configModel.keyboardJoystick = value;
        }
        get
        {
            return configModel.keyboardJoystick;
        }
    }
    public string GetSaveButtonName
    {
        get
        {
            return configModel.configSaveButtonName;
        }
    }
    public string GetRestoreButtonName
    {
        get
        {
            return configModel.configRestoreButtonName;
        }
    }
    public Button GetSetConfigSaveButton
    { 
        get 
        { 
            return configModel.configSaveButton; 
        }
        set 
        { 
            configModel.configSaveButton = value;
        }
    }
    public Button GetSetConfigRestoreButton
    {
        get
        {
            return configModel.configRestoreButton;
        }
        set
        {
            configModel.configRestoreButton = value;
        }
    }
    public ModalSimple GetSetModalSimple
    {
        get 
        {
            return modalSimple;
        }
        set 
        {
            modalSimple = value;
        }
    }
    #endregion
    #region Events
    public void AssignControl(int id)
    {
        ResetAllColors();
        configModel.isAssign = true;
        configModel.configId = id;
        ChangeColor(id);

    }
    public void AssignConfigKeyboardJoyStickControl(int id)
    {
        toggleInputKeyboardJoystick.ToggleButton(id);
        GetSetKeyboardJoystick = id;        
    }
    public void SaveConfig()
    {
        SaveConfigMethod();
    }
    public void RestoreConfigEvent()
    {
        RestoreConfig();
    }
    public void ConfigSaveCloseModal()
    {
        modalSimple.CloseModal();
    }
    #endregion
    #region Methods
    public void Init()
    {        
        toggleInputKeyboardJoystick.SetButtonsListBuilder(configModel.menuConfigInputList);
        AssignConfigKeyboardJoyStickControl(0);
        modalSimple.CloseModal();
        SetConfigButtons();
    }
    public void GetKeyPress() 
    {
        if (configModel.isAssign)
        {
            KeyObj keyObj = null;
            if (GetSetKeyboardJoystick == 0)
                keyObj = KeyboardInput.GetKeyPressed();
            else
                keyObj = JoystickInput.GetJoyPressed();
            if (keyObj is not null)
            {
                configModel.keysControlArray[configModel.configId] = keyObj;
                ConfigSingleton.saveConfigDTO.keysControlArray[configModel.configId] = keyObj;
                configModel.isAssign = false;                
                ChangeColor(configModel.configId);
                ChangeButtonText(configModel.configId, configModel.keysControlArray[configModel.configId].keyName);
                ConfigSingleton.saveConfigDTO.keysControlArray[configModel.configId] = keyObj;
            }                
        }
    }    
    public void DismissAssign()
    {
        configModel.isAssign = false;
        ChangeColor(configModel.configId);
    }
    private void ChangeColor(int id)
    {
        if(id > -1)
            GetSetMenuConfigList[id].GetChild<NinePatchRect>(0).Modulate = new Color(configModel.isAssign ? configModel.colorSelected : configModel.colorNormal);
    }
    private void ResetAllColors()
    {
        foreach (var menuConfigObj in GetSetMenuConfigList)
        {
            menuConfigObj.GetChild<NinePatchRect>(0).Modulate = new Color(configModel.colorNormal);
        }
    }
    private void ChangeButtonText(int id, string buttonName)
    {
        GetSetMenuConfigList[id].GetChild<NinePatchRect>(0).GetChild<Label>(0).Text = buttonName;
    }
    private void SaveConfigMethod()
    {
        var saveConfigDTO = new SaveConfigDTO()
        {
            keyboardJoystick = configModel.keyboardJoystick,
            keysControlArray = configModel.keysControlArray.ToList()
        };
        var isSuccess = SaveLoad.SaveConfig<SaveConfigDTO>(saveConfigDTO);
        modalSimple.SetTitle(configModel.saveConfigModalTitle);
        modalSimple.SetContext(isSuccess ? configModel.saveConfigConfirmModalContext : configModel.saveConfigErrorModalContext);        
        modalSimple.OpenModal();
    }
    private void SetConfigButtons()
    {        
        if (ConfigSingleton.saveConfigDTO is null)
        {            
            SetConfigButtonsDefault();
        }
        else
        {
            SetConfigButtonsLoaded();
        }
    }
    private void SetConfigButtonsDefault()
    {
        toggleInputKeyboardJoystick.ToggleButton(ConfigDefaultInputs.keyboardJoystick);
        GetSetKeyboardJoystick = ConfigDefaultInputs.keyboardJoystick;
        for (int i = 0; i < ConfigDefaultInputs.keysControlArray.Count; i++)
        {            
            configModel.keysControlArray[i] = ConfigDefaultInputs.keysControlArray[i];
            ChangeButtonText(i, configModel.keysControlArray[i].keyName);
        }
        InitSaveConfigDTO();
    }
    private void SetConfigButtonsLoaded()
    {
        toggleInputKeyboardJoystick.ToggleButton(ConfigSingleton.saveConfigDTO.keyboardJoystick);
        GetSetKeyboardJoystick = ConfigSingleton.saveConfigDTO.keyboardJoystick;
        for (int i = 0; i < ConfigSingleton.saveConfigDTO.keysControlArray.Count; i++)
        {
            configModel.keysControlArray[i] = ConfigSingleton.saveConfigDTO.keysControlArray[i];
            ChangeButtonText(i, configModel.keysControlArray[i].keyName);
        }
    }
    private void InitSaveConfigDTO()
    {
        ConfigSingleton.saveConfigDTO = new SaveConfigDTO();
        ConfigSingleton.saveConfigDTO.keyboardJoystick = ConfigDefaultInputs.keyboardJoystick;        
        for (int i = 0; i < ConfigDefaultInputs.keysControlArray.Count; i++)
        {            
            ConfigSingleton.saveConfigDTO.keysControlArray.Add(ConfigDefaultInputs.keysControlArray[i]);
        }
    }
    private void RestoreConfig()
    {
        SetConfigButtonsDefault();
        SaveLoad.DeleteConfigFile();
    }
    #endregion
}
