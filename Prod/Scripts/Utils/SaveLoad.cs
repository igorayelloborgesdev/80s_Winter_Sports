using Godot;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;

public class SaveLoad
{
    #region const
    private static string path = ProjectSettings.GlobalizePath("user://");
    private static string configFileName = "config.json";
    private static string pathData = ProjectSettings.GlobalizePath("res://");
    private static string pathDataName = "Data\\country.json";
    private static string saveFileName = "saveGame";
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

    public static T LoadData<T>(string path, string path1)
    {
        try
        {
            var loadPath = Path.Join(pathData, path);

            if (!File.Exists(loadPath))
            {
                var loadPath1 = Path.Join(pathData, path1);
                var file = File.ReadAllText(loadPath1);
                return JsonConvert.DeserializeObject<T>(file);
            }

            var file1 = File.ReadAllText(loadPath);
            return JsonConvert.DeserializeObject<T>(file1);
        }
        catch (Exception ex)
        {
            return default(T);
        }

    }

    public static List<T> CheckSaveGameFile<T>()
    {
        try
        {      
            List<T> list = new List<T>();
            for (int i = 0; i < 5; i++)
            {
                var loadPath = Path.Join(path, (saveFileName + i.ToString() + ".json"));
                if (File.Exists(loadPath))
                {
                    var file = File.ReadAllText(loadPath);
                    var fileLoaded = JsonConvert.DeserializeObject<T>(file);
                    list.Add(fileLoaded);
                }
            }            
            return list;
        }
        catch (Exception ex)
        {
            return default(List<T>);
        }
    }

    public static bool SaveGameData<T>(T dataObj, string saveFileId)
    {
        var json = JsonConvert.SerializeObject(dataObj);
        var savePath = Path.Join(path, (saveFileName + saveFileId + ".json"));
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

}
