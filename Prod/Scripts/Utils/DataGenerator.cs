using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.DTO;

public static class DataGenerator
{
    #region Country
    private static string[] countryName = 
    {
        "Austria",
        "Canada",
        "Finland",
        "France",
        "West Germany",
        "Great Britain",
        "East Germany",
        "Italy",
        "Japan",
        "Netherlands",
        "Norway",
        "Switzerland",
        "Sweden",
        "Czechoslovakia",
        "Soviet Union",
        "United States"
    };
    private static string[] countryCode =
    {
        "AUT",
        "CAN",
        "FIN",
        "FRA",
        "FRG",
        "GBR",
        "GDR",
        "ITA",
        "JPN",
        "NED",
        "NOR",
        "SUI",
        "SWE",
        "TCH",
        "URS",
        "USA"
    };
    public static void CreateCountryData(Color[] hairColor, Color[] skinColor, Color bootsColor
                                        ,Color[] kit1BodyColor, Color[] kit1ArmsColor, Color[] kit1LegsColor, 
                                        Color[] kit2BodyColor, Color[] kit2LegsColor, Color[] kit3BodyColor, Color[] kit3LegsColor)
    {
        var countryObjDTO = new CountryObjDTO();
        countryObjDTO.countryList = new List<CountryDTO>();

        for (int i = 0; i < countryName.Length; i++)
        {
            var countryObj = new CountryDTO()
            {
                Id = i + 1,
                Name = countryName[i],
                Code = countryCode[i],
                SkinColor = skinColor[i],
                HairColor = hairColor[i],
                BootsColor = bootsColor,
                kit1BodyColor = kit1BodyColor[i],
                kit1ArmsColor = kit1ArmsColor[i],
                kit1LegsColor = kit1LegsColor[i],
                kit2BodyColor = kit2BodyColor[i],
                kit2ArmsColor = kit2BodyColor[i],
                kit2LegsColor = kit2LegsColor[i],
                kit3BodyColor = kit3BodyColor[i],
                kit3ArmsColor = kit3BodyColor[i],
                kit3LegsColor = kit3LegsColor[i]
            };
            countryObjDTO.countryList.Add(countryObj);
        }
        SaveLoad.SaveData<CountryObjDTO>(countryObjDTO);
    }
    #endregion
    #region Level
    private static string[] levelName =
    {
        "Downhill",
        "Slalom",
        "Giant slalom",
        "Super G",        
        "Speed skating 500 metres",
        "Speed skating 1500 metres",
        "Biathlon",
        "Luge",
        "Bobsleigh",
        "Ski jumping",
        "Ice Hockey",
        "Cross Country"
    };
    private static string[] prefabName =
    {
        "skiTrack",
        "skiTrack",
        "skiTrack",
        "skiTrack",
        "SpeedSkating",
        "SpeedSkating",
        "Biathlon",
        "LugeBobsleigh",
        "LugeBobsleigh",
        "Ski jumping",
        "Ice Hockey",
        "Cross Country"
    };

    private static int[] SubLevelId =
    {
        0, 
        1, 
        2, 
        3,
        0,
        1,
        0,
        0,
        1,
        0,
        0,
        0
    };
    public static void CreateLevelData()
    {
        var levelObjDTO = new LevelObjDTO();
        levelObjDTO.levelList = new List<LevelDTO>();        
        for (int i = 0; i < levelName.Length; i++)
        {            
            var levelObj = new LevelDTO()
            {
                Id = i + 1,
                Name = levelName[i],
                prefabName = prefabName[i],
                SubLevelId = SubLevelId[i]
            };
            levelObjDTO.levelList.Add(levelObj);
        }        
        SaveLoad.SaveData<LevelObjDTO>(levelObjDTO, "Data//level.json");
    }
    #endregion

}
