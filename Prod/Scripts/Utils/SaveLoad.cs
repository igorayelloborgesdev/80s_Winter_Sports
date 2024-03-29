using Godot;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Text;

public class SaveLoad
{
    #region const
    private static string path = ProjectSettings.GlobalizePath("user://");
    private static string configFileName = "config.json";
    private static string pathData = ProjectSettings.GlobalizePath("res://");
    private static string pathDataName = "Data\\country.json";
    #endregion

    public static bool SaveConfig<T>(T configObj)
    {        
        var json = JsonConvert.SerializeObject(configObj);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        var savePath = Path.Join(path, configFileName);        
        try
        {
            File.WriteAllText(savePath, json);                        
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }

    public static T LoadConfig<T>()
    {
        try
        {            
            var loadPath = Path.Join(path, configFileName);
            var file = File.ReadAllText(loadPath);
            return JsonConvert.DeserializeObject<T>(file);
        }
        catch (Exception ex)
        {
            return default(T);
        }
        
    }

    public static void DeleteConfigFile()
    {
        try
        {
            var loadPath = Path.Join(path, configFileName);
            File.Delete(loadPath);
        }
        catch (Exception ex)
        {         
        }
    }

    public static bool SaveData<T>(T dataObj)
    {
        var json = JsonConvert.SerializeObject(dataObj);
        var savePath = Path.Join(pathData, pathDataName);
        try
        {                    
            File.WriteAllText(savePath, json);
        }
        catch (Exception ex)
        {         
            return false;
        }
        return true;
    }
    public static bool SaveData<T>(T dataObj, string path)
    {
        var json = JsonConvert.SerializeObject(dataObj);
        var savePath = Path.Join(pathData, path);
        try
        {
            File.WriteAllText(savePath, json);
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }
    public static T LoadData<T>()
    {
        try
        {
            var loadPath = Path.Join(pathData, pathDataName);            
            var file = File.ReadAllText(loadPath);
            return JsonConvert.DeserializeObject<T>(file);
        }
        catch (Exception ex)
        {
            return default(T);
        }

    }
    public static T LoadData<T>(string path)
    {
        try
        {
            var loadPath = Path.Join(pathData, path);
            var file = File.ReadAllText(loadPath);
            return JsonConvert.DeserializeObject<T>(file);
        }
        catch (Exception ex)
        {
            return default(T);
        }

    }
}
