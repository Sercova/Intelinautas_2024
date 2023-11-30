using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class ExecFuncsPlayerInfo : MonoBehaviour {

    [Serializable]
    public class levelInfo
    {
        public string levelName;
        public string sceneName;
        public string nextlevelSceneName;
        public int QItems;
        public float DistItems;
        public float VarCoef;

        public levelInfo(string arg_levelName
            , string arg_sceneName
            , string arg_nextlevelSceneName
            , int arg_QItems
            , float arg_DistItems
            , float arg_VarCoef)
        {
            levelName = arg_levelName;
            sceneName = arg_sceneName;
            nextlevelSceneName = arg_nextlevelSceneName;
            QItems = arg_QItems;
            DistItems = arg_DistItems;
            VarCoef = arg_VarCoef;
        }

        private float floatDifficulty;

        public float Difficulty
        {
            get
            {
                string levelName;
                if (PlayerPrefs.HasKey(ExecFuncsMngr.Instance.PlayerName))
                {
                    levelName = PlayerPrefs.GetString(ExecFuncsMngr.Instance.PlayerName);
                    if (SceneManager.GetSceneByName(levelName) == null)
                        floatDifficulty = -1.0f;
                    else
                        //floatDifficulty = (QItems / 20) * (VarCoef / 5) / (DistItems / 100);
                        floatDifficulty = 0.0f;
                }
                else
                {
                    floatDifficulty = -2.0f; ;
                }

                return floatDifficulty;
            }
        }

    }

    public List<levelInfo> ExecFuncLevels;

    public string[] ExecFuncsTags;

    public levelInfo LevelInfo(string arg_LevelName)
    {
        foreach (levelInfo lvl_Info in ExecFuncLevels)
        {
            if (lvl_Info.sceneName == arg_LevelName)
            {
                return lvl_Info;
            }
        }

        levelInfo tmpLvlInfo = new levelInfo("-", "NONE", "Trainning_Controls", 0, 0.0f, 0.0f);
        return tmpLvlInfo;
    }

    public levelInfo LevelInfo()
    {
        string levelName;
        if (PlayerPrefs.HasKey(ExecFuncsMngr.Instance.PlayerName))
        {
            // Level reached.
            levelName = PlayerPrefs.GetString(ExecFuncsMngr.Instance.PlayerName);
            //if (SceneManager.GetSceneByName("Levels/" + levelName).buildIndex != -1)
            //{
                foreach (levelInfo lvl_Info in ExecFuncLevels)
                {
                    if (lvl_Info.sceneName == levelName)
                    {
                        return lvl_Info;
                    }
                }
            //}
        }

        levelInfo tmpLvlInfo = new levelInfo("-", "NONE", "Trainning_Controls", 0, 0.0f, 0.0f);
        return tmpLvlInfo;

    }

    void Awake()
    {
        // Extract  tag's names from the file ExecFuncs\ProjectSettings\TagManager.asset'
        ExecFuncsTags = new string[4]
            { "Challenge_temp",
            "Challenge_processing",
            "Challenge_reasoning"
            ,"Challenge_intro"  // It should not be counted for the calculation of the complexity of the scene.
            };

        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    void Start()
    {

        ExecFuncLevels = new List<levelInfo>();
        // Order given by Difficulty Level Algorithm

        //// For Archetype gameplay testing
        
        ExecFuncLevels.Add(new levelInfo("PROTOTIPO 1 de 2", "Room006", "Room007", 0, 0.0f, 0.0f));
        ExecFuncLevels.Add(new levelInfo("PROTOTIPO 2 de 2", "Room007", "Room008", 0, 0.0f, 0.0f));


        //ExecFuncLevels.Add(new levelInfo("Créditos", "Credits", "", 0, 0.0f, 0.0f));

    }


    public float LevelDifficulty()
    {
        return LevelInfo().Difficulty;
    }

    public void SetFeatures(
                      int arg_QItems
                    , float arg_DistItems
                    , float arg_VarCoef)
    {
        LevelInfo().QItems = arg_QItems;
        LevelInfo().DistItems = arg_DistItems;
        LevelInfo().VarCoef = arg_VarCoef;
    }


    public string GetFeatures()
    {
        return LevelInfo().QItems.ToString() + "\t" + LevelInfo().DistItems.ToString() + "\t" + LevelInfo().VarCoef.ToString();
    }


    public string NextLevelTo()
    {
        //string result = LevelInfo().nextlevelSceneName;
        //if (result == "-")
        //    return "ChileSurTorresPaine08";
        //else
        return LevelInfo().nextlevelSceneName;
    }

    public string RunningLevelDescription()
    {
        string nextSceneName = LevelInfo().nextlevelSceneName;

        foreach (levelInfo lvl_Info in ExecFuncLevels)
        {
            if (lvl_Info.sceneName == nextSceneName)
            {
                return lvl_Info.levelName;
            }
        }

        return "-";
    }

    public string RunningSceneName()
    {
        return  LevelInfo().sceneName;
    }

    public string NextLevelTo(string arg_reachedSceneName)
    {
        foreach (levelInfo lvl_Info in ExecFuncLevels)
        {
            if (lvl_Info.sceneName == arg_reachedSceneName)
            {
                return lvl_Info.nextlevelSceneName;
            }
        }
        return "Trainning_Controls";
    }

    public int SavePlayers(PlayerInfo arg_info) {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(Application.persistentDataPath + "/Players.sav", FileMode.Create);

            bf.Serialize(fs, arg_info);
            fs.Close();
            //Debug.Log("SavePlayers: " + Application.persistentDataPath + "/Players.sav");

            return 1;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return 0;
            //throw;
        }
    }

    public PlayerInfo LoadPlayers()
    {
        PlayerInfo info = null;

        if (File.Exists(Application.persistentDataPath + "/Players.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(Application.persistentDataPath + "/Players.sav", FileMode.Open);
            info = bf.Deserialize(fs) as PlayerInfo;
            fs.Close();
        }
        //Debug.Log("LoadPlayers: " + Application.persistentDataPath + "/Players.sav");
        return info;
    }
}


[Serializable]
public class PlayerInfo {
    // info to be stored in a binary format file.
    public List<string> Names = new List<string>();
    public List<string> Level = new List<string>();
    public List<string> Profile = new List<string>();
}