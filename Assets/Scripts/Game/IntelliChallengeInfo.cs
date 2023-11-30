using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class IntelliChallengeInfo : MonoBehaviour {
    [Serializable]
    public class challengeInfo
    {
        public string ChllgTitle;
        public string SceneName;
        public string ChllgSceneName;
        public int QItems;
        public float DistItems;
        public float VarCoef;

        private float floatDifficulty;
        private int intCreatureID;

        public challengeInfo(
              string arg_ChallengeTitle
            , string arg_SceneName
            , string arg_ChallengeSceneName
            , int arg_ChllgCreatureID)
        {
            ChllgTitle = arg_ChallengeTitle;
            SceneName = arg_SceneName;
            ChllgSceneName = arg_ChallengeSceneName;
            intCreatureID = arg_ChllgCreatureID;
            QItems = 0;
            DistItems = 0f;
            VarCoef = 0f;
        }

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
                    floatDifficulty = -2.0f;
                }

                return floatDifficulty;
            }
        }

         public int ChllgCreatureID
        {
            get
            {
                // TODO: Evaluate that ChllgCreatureID should be unique throughout the game.
                return intCreatureID;
            }
            set
            { 
                intCreatureID = value; 
            }
        }
    }

    public List<challengeInfo> Challenges;
    public List<challengeInfo> CompletedChallenges;
    public List<SaveSceneState.SceneData> DestroyedSceneObjects = new List<SaveSceneState.SceneData>();

    public string[] ExecFuncsTags;

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
                        floatDifficulty = (QItems / 20) * (VarCoef / 5) / (DistItems / 100);
                }
                else
                {
                    floatDifficulty = -2.0f; ;
                }

                return floatDifficulty;
            }
        }

    }

    public int LastChallengeIdIn(string arg_SceneName)
    {
        return -1;
    }

    public int LastChallengeId()
    {
        int lastindex = CompletedChallenges.Count - 1;
        return CompletedChallenges[lastindex].ChllgCreatureID;
    }

    public int ChallengesCompletedIn(string arg_SceneName)
    {
        int counter=0;
        foreach (challengeInfo chllg_Info in CompletedChallenges)
        {
            if (chllg_Info.SceneName == arg_SceneName)
            {
                counter++;
            }
        }
        return counter;
    }



    public void CompleteChallenge(string arg_ChllgSceneName)
    {
        // arg_ChllgSceneName is unique within the 'Challenges' list
        List<challengeInfo> tmpChallenges = new List<challengeInfo>(Challenges);
        foreach (challengeInfo chllg_Info in tmpChallenges)
        {
            if (chllg_Info.ChllgSceneName == arg_ChllgSceneName)
            {
                CompletedChallenges.Add(chllg_Info);
                Challenges.Remove(chllg_Info);
            }
        }
    }


    public bool ChallengeIsCompleted(int arg_CreatureID)
    {
        // Indicates whether the challenge asociatedto the creature is complete in the current scene.
        // arg_ChllgSceneName is unique within the 'Challenges' list
        foreach (challengeInfo chllg_Info in CompletedChallenges)
        {
            if (chllg_Info.ChllgCreatureID == arg_CreatureID)
            {
                return true;
            }
        }
        return false;
    }

    public bool ChallengeIsCompleted(string arg_ChllgSceneName)
    {
        // arg_ChllgSceneName is unique within the 'Challenges' list
        foreach (challengeInfo chllg_Info in Challenges)
        {
            if (chllg_Info.ChllgSceneName == arg_ChllgSceneName)
            {
                return true;
            }
        }
        return false;
    }

    public challengeInfo ChallengeInfo(string arg_ChllgSceneName)
    {
        // arg_ChllgSceneName is unique within the 'Challenges' list
        foreach (challengeInfo chllg_Info in Challenges)
        {
            if (chllg_Info.ChllgSceneName == arg_ChllgSceneName)
            {
                return chllg_Info;
            }
        }

        challengeInfo tmpChllInfo = new challengeInfo("-", "NONE", "NO CHALLENGE", -1);
        return tmpChllInfo;
    }

    //public challengeInfo ChallengeInfo()
    //{
    //    throw new NotImplementedException("ChallengeInfo() No se ha implementado aún.");
    //}

    static IntelliChallengeInfo instanceChallengeInfo;
    public static IntelliChallengeInfo GetInstance()
    {
        return instanceChallengeInfo;
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

        if (instanceChallengeInfo != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instanceChallengeInfo = this;
        DontDestroyOnLoad(transform.gameObject);
        
    }

    public List<levelInfo> ExecFuncLevels;

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

    // Use this for initialization
    void Start()
    {

        Challenges = new List<challengeInfo>();
        // Order given by Difficulty Level Algorithm

        //// For Archetype gameplay testing

        Challenges.Add(new challengeInfo("DESAFÍO R006D01", "Room006", "Chllg_R006D01", 1));
        Challenges.Add(new challengeInfo("DESAFÍO R007D01", "Room007", "Chllg_R007D01", 1));
        Challenges.Add(new challengeInfo("DESAFÍO R007D02", "Room007", "Chllg_R007D02", 2));
        Challenges.Add(new challengeInfo("DESAFÍO R007D03", "Room007", "Chllg_R007D03", 3));


        //Challenges.Add(new challengeInfo("Créditos", "Credits", "", 0, 0.0f, 0.0f));

    }


    //public float LevelDifficulty()
    //{
    //    return ChallengeInfo().Difficulty;
    //}

    //public void SetFeatures(
    //                  int arg_QItems
    //                , float arg_DistItems
    //                , float arg_VarCoef)
    //{
    //    ChallengeInfo().QItems = arg_QItems;
    //    ChallengeInfo().DistItems = arg_DistItems;
    //    ChallengeInfo().VarCoef = arg_VarCoef;
    //}


    //public string GetFeatures()
    //{
    //    return ChallengeInfo().QItems.ToString() + "\t" + ChallengeInfo().DistItems.ToString() + "\t" + ChallengeInfo().VarCoef.ToString();
    //}


    //public string NextChallengeSceneName(int arg_CreatureID)
    //{
    //    throw new NotImplementedException("NextChallengeSceneName(int arg_CreatureID) no ha sido implementado aún.");
    //}

    //public string RunningLevelDescription()
    //{
    //    throw new NotImplementedException("RunningLevelDescription() no ha sido implementado aún.");
    //}

    //public string RunningSceneName()
    //{
    //    return  ChallengeInfo().SceneName;
    //}

    public string ChallengeSceneName(int arg_CreatureID)
    {
        string scnName = SceneManager.GetActiveScene().name;
        foreach (challengeInfo chllg_Info in Challenges)
        {
            if (chllg_Info.SceneName == scnName && chllg_Info.ChllgCreatureID == arg_CreatureID)
            {
                return chllg_Info.ChllgSceneName;
            }
        }

        return "NONE";
    }
    public string SceneName()
    {
        string chllgScnName = SceneManager.GetActiveScene().name;
        foreach (challengeInfo chllg_Info in Challenges)
        {
            if (chllg_Info.ChllgSceneName == chllgScnName)
            {
                return chllg_Info.SceneName;
            }
        }

        foreach (challengeInfo chllg_Info in CompletedChallenges)
        {
            if (chllg_Info.ChllgSceneName == chllgScnName)
            {
                return chllg_Info.SceneName;
            }
        }

        return "NONE";
    }


    //public int SaveChallenges(ChallengeInfo arg_info) {
    //    try
    //    {
    //        BinaryFormatter bf = new BinaryFormatter();
    //        FileStream fs = new FileStream(Application.persistentDataPath + "/Players.sav", FileMode.Create);

    //        bf.Serialize(fs, arg_info);
    //        fs.Close();
    //        //Debug.Log("SavePlayers: " + Application.persistentDataPath + "/Players.sav");

    //        return 1;
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log(e.Message);
    //        return 0;
    //        //throw;
    //    }
    //}

    //public ChallengeInfo LoadChallenges()
    //{
    //    ChallengeInfo info = null;

    //    if (File.Exists(Application.persistentDataPath + "/Players.sav"))
    //    {
    //        BinaryFormatter bf = new BinaryFormatter();
    //        FileStream fs = new FileStream(Application.persistentDataPath + "/Players.sav", FileMode.Open);
    //        info = bf.Deserialize(fs) as ChallengeInfo;
    //        fs.Close();
    //    }
    //    //Debug.Log("LoadChallenges: " + Application.persistentDataPath + "/Players.sav");
    //    return info;
    //}
}


//[Serializable]
//public class ChallengeInfo {
//    // info to be stored in a binary format file.
//    public List<string> Names = new List<string>();
//    public List<string> Challenge = new List<string>();
//    public List<string> Profile = new List<string>();
//}

