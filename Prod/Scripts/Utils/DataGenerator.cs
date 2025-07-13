using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterSports.Scripts.DTO;
using static System.Formats.Asn1.AsnWriter;

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

    private static int[] sportSkillAUT =
    {
        5,//"Downhill"
        3,//"Slalom"
        4,//"Giant slalom"
        5,//"Super G"
        3,//"Speed skating 500 metres"
        4,//"Speed skating 1500 metres"
        3,//"Biathlon"
        4,//"Luge"
        3,//"Bobsleigh"
        5,//"Skijumping"
        2,//"Ice Hockey"
        3//"Cross Country"
    };
    private static int[] sportSkillCAN =
    {
        4,//"Downhill"
        2,//"Slalom"
        4,//"Giant slalom"
        4,//"Super G"
        5,//"Speed skating 500 metres"
        5,//"Speed skating 1500 metres"
        2,//"Biathlon"
        3,//"Luge"
        3,//"Bobsleigh"
        3,//"Skijumping"
        3,//"Ice Hockey"
        3//"Cross Country"
    };
    private static int[] sportSkillFIN =
    {
        1,//"Downhill"
        1,//"Slalom"
        1,//"Giant slalom"
        1,//"Super G"
        2,//"Speed skating 500 metres"
        3,//"Speed skating 1500 metres"
        3,//"Biathlon"
        1,//"Luge"
        1,//"Bobsleigh"
        5,//"Skijumping"
        4,//"Ice Hockey"
        5//"Cross Country"
    };
    private static int[] sportSkillFRA =
    {
        4,//"Downhill"
        4,//"Slalom"
        4,//"Giant slalom"
        5,//"Super G"
        2,//"Speed skating 500 metres"
        3,//"Speed skating 1500 metres"
        3,//"Biathlon"
        2,//"Luge"
        2,//"Bobsleigh"
        2,//"Skijumping"
        2,//"Ice Hockey"
        3//"Cross Country"
    };
    private static int[] sportSkillFRG =
    {
        5,//"Downhill"
        4,//"Slalom"
        4,//"Giant slalom"
        3,//"Super G"
        3,//"Speed skating 500 metres"
        3,//"Speed skating 1500 metres"
        5,//"Biathlon"
        5,//"Luge"
        3,//"Bobsleigh"
        3,//"Skijumping"
        3,//"Ice Hockey"
        3//"Cross Country"
    };
    private static int[] sportSkillGBR =
    {
        3,//"Downhill"
        2,//"Slalom"
        2,//"Giant slalom"
        2,//"Super G"
        2,//"Speed skating 500 metres"
        1,//"Speed skating 1500 metres"
        2,//"Biathlon"
        2,//"Luge"
        3,//"Bobsleigh"
        2,//"Skijumping"
        1,//"Ice Hockey"
        2//"Cross Country"
    };
    private static int[] sportSkillGDR =
    {
        1,//"Downhill"
        1,//"Slalom"
        1,//"Giant slalom"
        1,//"Super G"
        5,//"Speed skating 500 metres"
        5,//"Speed skating 1500 metres"
        5,//"Biathlon"
        5,//"Luge"
        5,//"Bobsleigh"
        5,//"Skijumping"
        1,//"Ice Hockey"
        5//"Cross Country"
    };
    private static int[] sportSkillITA =
    {
        3,//"Downhill"
        5,//"Slalom"
        5,//"Giant slalom"
        3,//"Super G"
        2,//"Speed skating 500 metres"
        3,//"Speed skating 1500 metres"
        4,//"Biathlon"
        5,//"Luge"
        3,//"Bobsleigh"
        2,//"Skijumping"
        2,//"Ice Hockey"
        4//"Cross Country"
    };
    private static int[] sportSkillJPN =
    {
        3,//"Downhill"
        2,//"Slalom"
        2,//"Giant slalom"
        2,//"Super G"
        4,//"Speed skating 500 metres"
        3,//"Speed skating 1500 metres"
        2,//"Biathlon"
        2,//"Luge"
        2,//"Bobsleigh"
        4,//"Skijumping"
        2,//"Ice Hockey"
        2//"Cross Country"
    };
    private static int[] sportSkillNED =
    {
        1,//"Downhill"
        1,//"Slalom"
        1,//"Giant slalom"
        1,//"Super G"
        4,//"Speed skating 500 metres"
        5,//"Speed skating 1500 metres"
        1,//"Biathlon"
        1,//"Luge"
        2,//"Bobsleigh"
        1,//"Skijumping"
        2,//"Ice Hockey"
        1//"Cross Country"
    };
    private static int[] sportSkillNOR =
    {
        3,//"Downhill"
        2,//"Slalom"
        2,//"Giant slalom"
        2,//"Super G"
        3,//"Speed skating 500 metres"
        5,//"Speed skating 1500 metres"
        5,//"Biathlon"
        2,//"Luge"
        1,//"Bobsleigh"
        4,//"Skijumping"
        2,//"Ice Hockey"
        5//"Cross Country"
    };
    private static int[] sportSkillSUI =
    {
        5,//"Downhill"
        4,//"Slalom"
        5,//"Giant slalom"
        4,//"Super G"
        2,//"Speed skating 500 metres"
        2,//"Speed skating 1500 metres"
        2,//"Biathlon"
        1,//"Luge"
        5,//"Bobsleigh"
        3,//"Skijumping"
        2,//"Ice Hockey"
        4//"Cross Country"
    };
    private static int[] sportSkillSWE =
    {
        2,//"Downhill"
        5,//"Slalom"
        5,//"Giant slalom"
        4,//"Super G"
        3,//"Speed skating 500 metres"
        5,//"Speed skating 1500 metres"
        3,//"Biathlon"
        1,//"Luge"
        2,//"Bobsleigh"
        3,//"Skijumping"
        4,//"Ice Hockey"
        5//"Cross Country"
    };
    private static int[] sportSkillTCH =
    {
        4,//"Downhill"
        3,//"Slalom"
        3,//"Giant slalom"
        2,//"Super G"
        1,//"Speed skating 500 metres"
        2,//"Speed skating 1500 metres"
        3,//"Biathlon"
        3,//"Luge"
        1,//"Bobsleigh"
        4,//"Skijumping"
        4,//"Ice Hockey"
        4//"Cross Country"
    };
    private static int[] sportSkillURS =
    {
        3,//"Downhill"
        3,//"Slalom"
        3,//"Giant slalom"
        2,//"Super G"
        5,//"Speed skating 500 metres"
        5,//"Speed skating 1500 metres"
        5,//"Biathlon"
        5,//"Luge"
        5,//"Bobsleigh"
        2,//"Skijumping"
        5,//"Ice Hockey"
        5//"Cross Country"
    };
    private static int[] sportSkillUSA =
    {
        5,//"Downhill"
        5,//"Slalom"
        5,//"Giant slalom"
        3,//"Super G"
        5,//"Speed skating 500 metres"
        5,//"Speed skating 1500 metres"
        3,//"Biathlon"
        3,//"Luge"
        3,//"Bobsleigh"
        3,//"Skijumping"
        5,//"Ice Hockey"
        3//"Cross Country"
    };

    public static void CreateCountryData(Color[] hairColor, Color[] skinColor, Color bootsColor
                                        ,Color[] kit1BodyColor, Color[] kit1ArmsColor, Color[] kit1LegsColor, 
                                        Color[] kit2BodyColor, Color[] kit2LegsColor, Color[] kit3BodyColor, Color[] kit3LegsColor)
    {

        List<List<int>> listSkill = new List<List<int>>();
        listSkill.Add(sportSkillAUT.ToList());
        listSkill.Add(sportSkillCAN.ToList());
        listSkill.Add(sportSkillFIN.ToList());
        listSkill.Add(sportSkillFRA.ToList());
        listSkill.Add(sportSkillFRG.ToList());
        listSkill.Add(sportSkillGBR.ToList());
        listSkill.Add(sportSkillGDR.ToList());
        listSkill.Add(sportSkillITA.ToList());
        listSkill.Add(sportSkillJPN.ToList());
        listSkill.Add(sportSkillNED.ToList());
        listSkill.Add(sportSkillNOR.ToList());
        listSkill.Add(sportSkillSUI.ToList());
        listSkill.Add(sportSkillSWE.ToList());
        listSkill.Add(sportSkillTCH.ToList());
        listSkill.Add(sportSkillURS.ToList());
        listSkill.Add(sportSkillUSA.ToList());

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
                kit3LegsColor = kit3LegsColor[i],
                sportSkill = listSkill[i]
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
    #region AI
    public static void CreateAIData() 
    {
        var aiDTO = new AIDTO();
        aiDTO.aiObjDTOList = new List<AIObjDTO>();
        for (int i = 0; i < levelName.Length; i++)
        {
            var obj = new AIObjDTO()
            {
                id = i + 1,
                score = 6000
            };
            aiDTO.aiObjDTOList.Add(obj);            
        }        
        SaveLoad.SaveData<AIDTO>(aiDTO, "Data//AIInit.json");
    }
    #endregion
}
