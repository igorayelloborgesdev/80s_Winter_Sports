using Godot;
using System;
using WinterSports.Scripts.DTO;
using WinterSports.Scripts.Singleton;

public class SplashController
{
    #region Variable
    private SplashModel splashModel = new SplashModel();
    #endregion
    #region Methods
    public float Timer(float delta, float time)
    {
        return time += delta;
    }
    public bool TimerCheck(float delta)
    {
        splashModel.time = Timer(delta, splashModel.time);
        if (splashModel.time > splashModel.timeToProceedToMainMenu)
            return true;
        return false;
    }
    public void LoadConfig()
    {
        ConfigSingleton.saveConfigDTO = SaveLoad.LoadConfig<SaveConfigDTO>();        
    }
    public void LoadCountryData()
    {
        CountrySingleton.countryObjDTO = SaveLoad.LoadData<CountryObjDTO>();        
    }
    public void LoadLevelData()
    {
        LevelSingleton.levelObjDTO = SaveLoad.LoadData<LevelObjDTO>("Data//level.json");
    }
    public void Init()
    {
        ConfigDefaultInputs.Init();        
    }
    public void LoadAIData()
    {
        AISingleton.crossCountryObjDTO = SaveLoad.LoadData<CrossCountryObjDTO>("Data//CrossCountryAI.json");
    }
    #endregion
}
