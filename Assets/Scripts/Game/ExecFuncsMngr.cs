using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;

using System;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Net.NetworkInformation;
using System.Xml;

public class ExecFuncsMngr : Singleton<ExecFuncsMngr>
{
    //Dictionary<string, int> MT_dict = new Dictionary<string, int>();
    //Dictionary<string, int> TotItems_dict = new Dictionary<string, int>();

    // gameState: State reached.
    private List<string> gameState = new List<string>();
    // gameStateElapsedTime: Necesary time to reach a State.
    private List<float> gameStateElapsedTime = new List<float>();
    // Sequence of states and the effort to reach each one.
    public List<ExecFuncsGamePlayStates> GamePlayStates = new List<ExecFuncsGamePlayStates>();

    private PlayerInfo Players = new PlayerInfo();

    
    protected ExecFuncsMngr() { }  // Allways will be singleton.
    //public static ExecFuncsMngr EFMngr = new ExecFuncsMngr();

    public ExecFuncsDB DB = new ExecFuncsDB();  // Global scope class
    public ExecFuncsDBSummary DBSummary = new ExecFuncsDBSummary();
    public ExecFuncsDBGamePlay DBGamePlay = new ExecFuncsDBGamePlay();

    public bool StatesSaved;

    public List<EFMngrStruct> EFMngrList = new List<EFMngrStruct>();
    public class EFMngrStruct {
        public float PLAN, RAZON, FLEX, REGU, MT, VPROC, DECI, TEMPO, DUAL, BRANCH;

        public EFMngrStruct(
                float arg_PLAN,
                float arg_RAZON,
                float arg_FLEX,
                float arg_REGU,
                float arg_MT,
                float arg_VPROC,
                float arg_DECI,
                float arg_TEMPO,
                float arg_DUAL,
                float arg_BRANCH
                )
        {
            PLAN = arg_PLAN;
            RAZON = arg_RAZON;
            FLEX = arg_FLEX;
            REGU = arg_REGU;
            MT = arg_MT;
            VPROC = arg_VPROC;
            DECI = arg_DECI;
            TEMPO = arg_TEMPO;
            DUAL = arg_DUAL;
            BRANCH = arg_BRANCH;

        }

    }

    [SerializeField]
    public bool isRestarted = false;

    public string PlayerName = "NONAME";  // Assigned in DialogNewPlayer.onClickOK() and recorded in UserPrefs to recovery last played level.
    public string LevelLaunchDateTime="";
    public string secretKey = "intelliseed"; // Edit this value and make sure it's the same as the one stored on the server

    private static string strArchetype;
    private static string homeURL;  
    public static string HomeURL
    {
        get {
            homeURL = "https://keepingames.000webhostapp.com/";  // TESTING ENVIRONMENT
            //homeURL = "http://localhost/intelinautas/";        // LOCAL DEVELOPMENT ENVIRONMENT
            return homeURL;
        }
    }


    // These are public just to debug
    public float PLAN;
    public float RAZON;
    public float FLEX;
    public float REGU;
    public float MT;
    public float VPROC;
    public float DECI;
    public float TEMPO;
    public float DUAL;
    public float BRANCH;
    public float TotalTime = 0.0f;

    private InventoryController InvCtrll;
    private int intTotalItems;
    public int TotalItems
    {
        get {
            InvCtrll = GameObject.Find("InventoryController").GetComponent<InventoryController>();
            if (InvCtrll == null)
                intTotalItems = 0;
            else
                intTotalItems = (PlayerPrefs.GetInt("qRestart") + 1) * InvCtrll.TotalItems();

            return intTotalItems;
        }
    }


    private string strGameplayName;
    public string GameplayName
    {
        get {
            // An assigned value for the private variable "ExecFuncsMngr.LevelLaunchDateTime" is required for this propperty 
            strGameplayName = SceneManager.GetActiveScene().name + "_" + LevelLaunchDateTime;
            return strGameplayName;
        }
    }

    SpeedController speedCtrll;
    private void Start()
    {
        // TODO: limpiar la lista de elementos en SpeedDB

        // Start event is triggered only this class is instantiated. 
        // e.i. "LevelSelector" scene

        //GameObject GObj = GameObject.Find("SpeedController");
        //if (GObj) {
        //    speedCtrll = GObj.GetComponent<SpeedController>();
        //    if (speedCtrll && !isRestarted)
        //        speedCtrll.ScrObjSpeedDB.ClearMeasurements();
        //}
    }

    public void SaveDBGamePlay(string arg_strFileName)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ExecFuncsDBGamePlay));
        FileStream stream = new FileStream(arg_strFileName, FileMode.Create);

        serializer.Serialize(stream, DBGamePlay);
        stream.Close();

    }

    public void SaveDBSummary(string arg_strFileName)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ExecFuncsDBSummary));
        FileStream stream = new FileStream(arg_strFileName, FileMode.Create);

        serializer.Serialize(stream, DBSummary);
        stream.Close();

    }

    public ExecFuncsDBSummary LoadDBSummary()
    {
        ExecFuncsDBSummary playerSummary = null;

        if (File.Exists(Application.persistentDataPath + "/" + ExecFuncsMngr.Instance.PlayerName + "_" + ExecFuncsMngr.Instance.MAC_Address() + ".xml"))
        {
            XmlSerializer xmlf = new XmlSerializer(typeof(ExecFuncsDBSummary));
            FileStream fs = new FileStream(Application.persistentDataPath + "/" + ExecFuncsMngr.Instance.PlayerName + "_" + ExecFuncsMngr.Instance.MAC_Address() + ".xml", FileMode.Open);
            playerSummary = xmlf.Deserialize(fs) as ExecFuncsDBSummary;
            fs.Close();
        }
        //Debug.Log("LoadPlayers: " + Application.persistentDataPath + "/Players.sav");
        return playerSummary;
    }

    GameObject GObj;
    GameController GameCtrller;

    public void SaveDB()
    {

        LevelLaunchDateTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        string strMAC = ExecFuncsMngr.Instance.MAC_Address();
        string strFileName = Application.persistentDataPath + "/" + ExecFuncsMngr.Instance.PlayerName + "_" + ExecFuncsMngr.Instance.MAC_Address() + "_" + ExecFuncsMngr.Instance.GameplayName + ".xml";
        FileStream stream = new FileStream(strFileName, FileMode.Create);

        XmlSerializer serializer = new XmlSerializer(typeof(ExecFuncsDB));
        serializer.Serialize(stream, DB);
        stream.Close();

        PlayerPrefs.SetString(ExecFuncsMngr.Instance.PlayerName, SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();

        GObj = GameObject.Find("GameController");
        GameCtrller = GObj.GetComponent<GameController>();

        //if (GObj)
        //{
        //    if (GameCtrller.ScrObjSpeedDB)
        //        GameCtrller.ScrObjSpeedDB.Speed_TimeElapsed();
        //}

        StateTimeElapsed(DB);

        // Time elapsed for every state
        MT = PhaseResult("WP");

        #region VPROC: Process Velocity
        if (GameObject.FindGameObjectsWithTag("Challenge_processing").Length != 0)
        {
            //  Fails over 3 attemps
            VPROC = 1 - (PlayerPrefs.GetInt("qChallengeTempFails") / (GameObject.FindGameObjectsWithTag("Challenge_processing").Length/2 * 3.0f));
            if (VPROC < 0.0f)
                VPROC = 0.0f;
            Debug.Log("VPROC: " + VPROC);
            Debug.Log("qChallengeTempFails: " + PlayerPrefs.GetInt("qChallengeTempFails"));
        }
        else
            VPROC = -1.0f;

        #endregion 

        #region MT: Work Memory
        // WP is acumulative for every retry or restart.
        if (MT > TotalItems)
            MT = TotalItems;

        MT = MT / TotalItems;
        #endregion

        #region PLAN: Planification
        PLAN = PhaseResult("WTCH") * 50.0f * MT / (TotalTime);
        if (PLAN > 1.0f)
            PLAN = 1.0f;
        #endregion

        #region REGU: Self Regulation
        // Three oportunities for every item in the inventory due to retries.
        REGU = (3 * InvCtrll.TotalItems() - PlayerPrefs.GetInt("qRestart")) * MT / (3 * InvCtrll.TotalItems());
        if (REGU < 0.0f)
            REGU = 0.0f;
        #endregion

        #region TEMPO: Temporal Challenge
        if (GameObject.FindGameObjectsWithTag("Challenge_temp").Length != 0)
        {
            //  Fails over 3 attemps, but there are 3 tag for every temp challenge
            TEMPO = 1 - (PlayerPrefs.GetInt("qChallengeTempFails") / ((GameObject.FindGameObjectsWithTag("Challenge_temp").Length / 3.0f) * 3.0f));
            if (TEMPO < 0.0f)
                TEMPO = 0.0f;
            Debug.Log("TEMPO: " + TEMPO);
            Debug.Log("qChallengeTempFails: " + PlayerPrefs.GetInt("qChallengeTempFails"));
        }
        else
            TEMPO = -1.0f;
        #endregion


        if (GameObject.FindGameObjectsWithTag("Challenge_reasoning").Length == 0)
        {
            RAZON = -1.0f;

        }


        //MT = MT_dict["MT"];

        // TODO: REFORMULAR EL CÁLCULO DE LOS PARÁMETROS QUE SE MANDAN A LA PANTALLA DE EVALUACIÓN.
        // UTILIZAR LA INFO QUE SE GUARDA EN EL RESUMEN.
        EFMngrList.Add(new EFMngrStruct(PLAN,RAZON,FLEX,REGU, MT, VPROC, DECI,TEMPO,DUAL,BRANCH));

        DBSummary = this.LoadDBSummary();

        if (DBSummary == null)
            DBSummary = new ExecFuncsDBSummary();

        DBSummary.AddSummaryLine(
                ExecFuncsMngr.Instance.GameplayName,
                PhaseResult("WTCH"),
                PhaseResult("PEH"),
                PhaseResult("PFH"),
                PhaseResult("PP"),
                PhaseResult("LS"),
                PhaseResult("LF"),
                PhaseResult("RESTR"),
                PhaseResult("CHKP"),
                PhaseResult("LM"),
                PhaseResult("FAIL"),
                Mathf.RoundToInt(PhaseResult("WP")),
                Mathf.RoundToInt(PhaseResult("retry")),
                Mathf.RoundToInt(PhaseResult("menupause")),
                TotalItems
                //GameCtrller.ScrObjSpeedDB.runningElapseTime,
                //GameCtrller.ScrObjSpeedDB.walkingElapseTime,
                //GameCtrller.ScrObjSpeedDB.idleElapseTime
            );

        string strFileNameStates = Application.persistentDataPath + "/" + ExecFuncsMngr.Instance.PlayerName + "_" + ExecFuncsMngr.Instance.MAC_Address() + "_" + ExecFuncsMngr.Instance.GameplayName + "_States.xml";
        SaveDBGamePlay(strFileNameStates);  // save the state's sequence to a xml file
        //StartCoroutine(SaveGamePlay(strFileNameStates));
        StartCoroutine(GetGamePlayID(strFileNameStates, ExecFuncsMngr.Instance.PlayerName, strMAC, SceneManager.GetActiveScene().name, LevelLaunchDateTime));

        //StartCoroutine(GameCtrller.ScrObjSpeedDB.SaveSpeedDB(intGamePlayID));

    
        // TODO: add something to distinguish the device.
        SaveDBSummary(Application.persistentDataPath + "/" + ExecFuncsMngr.Instance.PlayerName + "_" + ExecFuncsMngr.Instance.MAC_Address() + ".xml");

    }


    private IEnumerator SaveGamePlayStates(string arg_strFileNameStates)
    {
        string addgameplaystatesURL;
        // Comment and uncomment for Testing purposes
        //addgameplaystatesURL = HomeURL + "intelinautas_archetype_addgameplaystate.php?";

        if (PlayerPrefs.GetString(ExecFuncsMngr.Instance.PlayerName + "_ARCHETYPE") == "YES")
            addgameplaystatesURL = HomeURL + "intelinautas_archetype_addgameplaystate.php?";
        else
            addgameplaystatesURL = HomeURL + "intelinautas_addgameplaystate.php?"; //be sure to add a ? to your url

        XmlDocument xDoc = new XmlDocument();
        //xDoc = SerializeToXmlDocument(DBGamePlay);
        xDoc.Load(arg_strFileNameStates);

        Instance.StatesSaved = false;
        foreach (XmlNode item in xDoc.SelectNodes("IntelinautasGamePlay/GamePlayStates/State"))
        {
            string statesequence = item.SelectSingleNode("Seq").InnerText;
            string state = item.SelectSingleNode("PState").InnerText;
            string effort = item.SelectSingleNode("Effort").InnerText;
            string strPlayerAge = ExecFuncsMngr.Instance.PlayerAge().ToString();

            string hash = Md5Sum(intGamePlayID + statesequence + state + effort + strPlayerAge + secretKey);

            string post_url = addgameplaystatesURL + 
                "gameplayid=" + intGamePlayID +
                "&statesequence=" + statesequence  +
                "&state=" + state +
                "&effort=" + effort +
                "&playerage=" + strPlayerAge +
                "&hash=" + hash;

            Debug.Log("[States]post_url: " + post_url);

            // Post the URL to the site and create a download object to get the result.
            WWW hs_post = new WWW(post_url);
            yield return hs_post; // Wait until the download is done

            //if (hs_post.error != null)
            if (!string.IsNullOrEmpty(hs_post.error))
            {
                Debug.Log("There was an error trying to save state [" + statesequence + "]: " + hs_post.error);
            }

            //yield return null; // wait until foreach loop is finished

        }
        Instance.StatesSaved = true;

    }

    private int intGamePlayID;
    private IEnumerator GetGamePlayID(string arg_strFileNameStates, string arg_playername, string arg_macadd, string arg_gameplaylevel, string arg_gameplaydatetime)
    {
        string gameplayIDURL;
        string strPlayerAge = ExecFuncsMngr.Instance.PlayerAge().ToString();

        // Comment and uncomment for Testing purposes
        //gameplayIDURL = HomeURL + "intelinautas_archetype_addgameplayGetID.php?";

        if (PlayerPrefs.GetString(ExecFuncsMngr.Instance.PlayerName + "_ARCHETYPE") == "YES")
            gameplayIDURL = HomeURL + "intelinautas_archetype_addgameplayGetID.php?";
        else
            gameplayIDURL = HomeURL + "intelinautas_addgameplayGetID.php?";

        // JUST FOR DEBUGGING
        //gameplayIDURL = HomeURL + "intelinautas_archetype_addgameplayGetID.php?";

        intGamePlayID = -1;

        string hash = Md5Sum(arg_playername + arg_macadd + arg_gameplaylevel + arg_gameplaydatetime + strPlayerAge + secretKey);
        UnityWebRequest www = UnityWebRequest.Get(gameplayIDURL + "player_name=" + arg_playername + "&MAC=" + arg_macadd + "&gameplay_level=" + arg_gameplaylevel + "&gameplay_datetime=" + arg_gameplaydatetime + "&playerage=" + strPlayerAge + "&hash=" + hash);

        Debug.Log("[GamePlayID] post_url START: " + Time.time);
        yield return www.SendWebRequest();
        Debug.Log("[GamePlayID] post_url RETURN: " + Time.time);

        Debug.Log("[GamePlayID]post_url: " + www.url);
 
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (www.downloadHandler.text == "")
                intGamePlayID = -1;
            else
                intGamePlayID = int.Parse(www.downloadHandler.text);
            // Show results as text
            Debug.Log("intGamePlayID: " + intGamePlayID);
            
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }

        StartCoroutine(SaveGamePlayStates(arg_strFileNameStates));
        //StartCoroutine(GameCtrller.ScrObjSpeedDB.SaveSpeedDB(intGamePlayID));
        
    }

    public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

    float PhaseResult(string arg_State)
    {
        float result = 0f;

        for (int i = 0; i < gameState.Count; i++)
        {
            if (gameState[i] == arg_State)
                result += gameStateElapsedTime[i];
        }

        return result;
    }


    void StateTimeElapsed(ExecFuncsDB arg_DB) {

        DateTime CurrDateTime;
        DateTime LastDateTime = Convert.ToDateTime("1900-01-01");

        DateTime ChallengeDateTime_Start = Convert.ToDateTime("1900-01-01");
        DateTime ChallengeDateTime_End = Convert.ToDateTime("1900-01-01");

        gameStateElapsedTime.Clear();
        gameState.Clear();
        DBGamePlay.ResetStates();

        //gameState = new List<string>();
        //gameStateElapsedTime = new List<float>();
        int intStateSequence = 0;
        bool challengeStartFound = false;
        string lastMeasurement = "";
        foreach (ExecFuncsData item in arg_DB.Measurements)
        {
            //Debug.Log("item.value: " + item.value);
            if (item.scope == Scope.TIME && item.measurement.IndexOf("CHLL") == -1)
            {
                if (ConvertToDateTime(item.value, out CurrDateTime))
                {

                    if (lastMeasurement == "")
                        lastMeasurement = item.measurement;
                    else
                    {
                        // Initialize Diff in zero
                        TimeSpan Diff = CurrDateTime - CurrDateTime;

                        if (LastDateTime != Convert.ToDateTime("1900-01-01"))
                            Diff = CurrDateTime - LastDateTime;

                        TotalTime += (float)Diff.TotalSeconds;
                        gameStateElapsedTime.Add((float)Diff.TotalSeconds);
                        //gameState.Add(item.measurement);
                        gameState.Add(lastMeasurement);

                        intStateSequence++;
                        //DBGamePlay.AddState(intStateSequence, item.measurement, (float)Diff.TotalSeconds);
                        DBGamePlay.AddState(intStateSequence, lastMeasurement, (float)Diff.TotalSeconds);
                    }

                    if (item.measurement != lastMeasurement)
                        lastMeasurement = item.measurement;

                    LastDateTime = CurrDateTime;
                    //Debug.Log("CurrDT: " + CurrDateTime + "; LastDT: " + LastDateTime + "; State: " + item.measurement + "; Diff=" + (float)Diff.TotalSeconds);
                }
            }
            else if (item.scope == Scope.TIME && item.measurement.IndexOf("CHLL") >= 0)
            {
                // The calculation assumes nested challenges doesn't exists and 
                // "start" checkpoint is allways before the "end" checkpoint.
                // Calculate the time elapsed from the start to the end of the challenge
                //if (gameObject.name.Substring(gameObject.name.Length - 6, 6) == "_start")
                //{
                if (!challengeStartFound)
                {
                    ConvertToDateTime(item.value, out ChallengeDateTime_Start);
                    challengeStartFound = true;
                } else if (ConvertToDateTime(item.value, out ChallengeDateTime_End))
                {
                    if (lastMeasurement == "")
                        lastMeasurement = item.measurement;
                    else
                    {
                        // Initialize Diff in zero
                        TimeSpan Diff = ChallengeDateTime_End - ChallengeDateTime_End;

                        Diff = ChallengeDateTime_End - ChallengeDateTime_Start;

                        TotalTime += (float)Diff.TotalSeconds;
                        gameStateElapsedTime.Add((float)Diff.TotalSeconds);
                        //gameState.Add(item.measurement);
                        gameState.Add(lastMeasurement);
                        intStateSequence++;
                        //DBGamePlay.AddState(intStateSequence, item.measurement, (float)Diff.TotalSeconds);
                        DBGamePlay.AddState(intStateSequence, lastMeasurement, (float)Diff.TotalSeconds);
                    }

                    if (item.measurement != lastMeasurement)
                        lastMeasurement = item.measurement;

                    LastDateTime = ChallengeDateTime_End;
                    challengeStartFound = false;  // If any other challenge existed.
                }
            }

            if (item.scope == Scope.QUANTITY)
            {
                gameStateElapsedTime.Add(float.Parse(item.value));
                gameState.Add(item.measurement);
            }
        }

    }



    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        MAC_Address();
    }

    bool ConvertToDateTime(String arg_Str, out DateTime arg_DT)
    {
        try
        {
            arg_DT = DateTime.ParseExact(arg_Str, "yyyy-MM-dd HH:mm:ss,fff", CultureInfo.InvariantCulture);
            return true;
        }
        catch
        {
            arg_DT = Convert.ToDateTime("1900-01-01");
            return false;
        }
    }


    public double PlayerAge()
    {
        if (PlayerName == "NONAME")
            return 0;

        string strBirthDay = PlayerPrefs.GetString(ExecFuncsMngr.Instance.PlayerName + "_BIRTHDATE");
        DateTime BirthDate = new DateTime(int.Parse(strBirthDay.Substring(6, 4)), int.Parse(strBirthDay.Substring(3, 2)), int.Parse(strBirthDay.Substring(0, 2)), 0, 0, 0);

        DateTime zeroTime = new DateTime(1, 1, 1);

        TimeSpan ts_result;
        ts_result = (System.DateTime.Today - BirthDate);

        return (zeroTime + ts_result).Year - 1;
    }


    public string MAC_Address()
    {
        string result = "NO-WI-FI-AD-AP-TER";

        result = SystemInfo.deviceUniqueIdentifier;

        //Debug.Log("result: " + result);
        return result;
    }


    //public string MAC_Address()
    //{
    //    string result = "NO-WI-FI-AD-AP-TER";
    //    //IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
    //    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

    //    foreach (NetworkInterface adapter in nics)
    //    {
    //        Debug.Log("adapter.Name: " + adapter.Name);
    //        if (adapter.Name.ToUpper().Contains("WI-FI") || adapter.Name.ToUpper().Contains("WIFI"))
    //        {
    //            result = "";
    //            PhysicalAddress address = adapter.GetPhysicalAddress();
    //            byte[] bytes = address.GetAddressBytes();
    //            string mac = null;
    //            for (int i = 0; i < bytes.Length; i++)
    //            {
    //                mac = string.Concat(mac + (string.Format("{0}", bytes[i].ToString("X2"))));
    //                if (i != bytes.Length - 1)
    //                {
    //                    mac = string.Concat(mac + "-");
    //                }
    //            }
    //            //result += "   (" + adapter.Name + ":" + adapter.NetworkInterfaceType + ")\n";
    //            result += mac;
    //        }
    //    }

    //    return result;
    //}


    //public string SerializeToXml(object input)
    //{
    //    XmlSerializer ser = new XmlSerializer(input.GetType(), "http://schemas.yournamespace.com");
    //    string result = string.Empty;

    //    using (MemoryStream memStm = new MemoryStream())
    //    {
    //        ser.Serialize(memStm, input);

    //        memStm.Position = 0;
    //        result = new StreamReader(memStm).ReadToEnd();
    //    }

    //    return result;
    //}

    //public XmlDocument SerializeToXmlDocument(object input)
    //{
    //    XmlSerializer ser = new XmlSerializer(input.GetType(), "http://schemas.yournamespace.com");

    //    XmlDocument xd = null;

    //    using (MemoryStream memStm = new MemoryStream())
    //    {
    //        ser.Serialize(memStm, input);

    //        memStm.Position = 0;

    //        XmlReaderSettings settings = new XmlReaderSettings();
    //        settings.IgnoreWhitespace = true;

    //        using (var xtr = XmlReader.Create(memStm, settings))
    //        {
    //            xd = new XmlDocument();
    //            xd.Load(xtr);
    //        }
    //    }

    //    return xd;
    //}

}


