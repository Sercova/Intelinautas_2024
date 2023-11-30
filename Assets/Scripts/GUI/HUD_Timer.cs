using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HUD_Timer : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI txtName;

    [SerializeField]
    private TextMeshProUGUI txtTimer;

    [SerializeField]
    private TextMeshProUGUI txtLevelName;

    private float startTime;
    //private ExecFuncsPlayerInfo EFPlayerInfo;
    private IntelliChallengeInfo EFPlayerInfo;
    

    private void Start()
    {
        EFPlayerInfo = GameObject.Find("IntelliChallengeInfo").GetComponent<IntelliChallengeInfo>();

        startTime = Time.time;
        txtName.text = ExecFuncsMngr.Instance.PlayerName;
        txtName.text = txtName.text + " (" + PlayerPrefs.GetString(txtName.text + "_PROFILE") + ")";

        txtLevelName.text = EFPlayerInfo.RunningLevelDescription();
    }

    private void Update()
    {
        float t = Time.time - startTime;

        //txtTimer.text = string.Format("{0:00}", TimeSpan.FromSeconds(t));
        //txtTimer.text = string.Format("{0:0}", TimeSpan.FromSeconds(t));
        txtTimer.text = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss,fff");
    }
}
