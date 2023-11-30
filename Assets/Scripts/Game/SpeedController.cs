using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

public class SpeedController : MonoBehaviour
{
    
    //public SpeedDB ScrObjSpeedDB;
    //// Start is called before the first frame update
    //void Start()
    //{


    //}

    //public float runningElapseTime = 0.0f;
    //public float walkingElapseTime = 0.0f;
    //public float idleElapseTime = 0.0f;
    ////Speed_TimeElapsed(ScrObjSpeedDB);

    //public IEnumerator SaveSpeedDB(int arg_GamePlayID)
    //{
    //    Speed_TimeElapsed();

    //    string addSpeedURL;

    //    if (PlayerPrefs.GetString(ExecFuncsMngr.Instance.PlayerName + "_ARCHETYPE") == "YES")
    //        addSpeedURL = ExecFuncsMngr.HomeURL + "intelinautas_archetype_addspeedmode.php?";
    //    else
    //        addSpeedURL = ExecFuncsMngr.HomeURL + "intelinautas_addspeedmode.php?"; //be sure to add a ? to your url

    //    // JUST FOR DEBUGGING
    //    addSpeedURL = ExecFuncsMngr.HomeURL + "intelinautas_archetype_addspeedmode.php?";

    //    ExecFuncsMngr.Instance.StatesSaved = false;

    //    string GP_ID = arg_GamePlayID.ToString();
    //    string strPlayerAge = ExecFuncsMngr.Instance.PlayerAge().ToString();

    //    string hash_idle = ExecFuncsMngr.Instance.Md5Sum(GP_ID + "IDLE" + idleElapseTime + strPlayerAge + ExecFuncsMngr.Instance.secretKey);
    //    string post_url_idle = addSpeedURL +
    //        "gameplayid=" + GP_ID +
    //        "&speedmode=IDLE" +
    //        "&elapsetime=" + idleElapseTime +
    //        "&playerage=" + strPlayerAge +
    //        "&hash=" + hash_idle;

    //    Debug.Log(post_url_idle);

    //    // Post the URL to the site and create a download object to get the result.
    //    //UnityWebRequest hs_post_idle = new UnityWebRequest(post_url_idle);
    //    WWW hs_post_idle = new WWW(post_url_idle);
    //    yield return hs_post_idle; // Wait until the download is done

    //    //if (hs_post.error != null)
    //    if (!string.IsNullOrEmpty(hs_post_idle.error))
    //    {
    //        Debug.Log("There was an error trying to save Speed [IDLE]: " + hs_post_idle.error);
    //    }


    //    string hash_walk = ExecFuncsMngr.Instance.Md5Sum(GP_ID + "WALK" + walkingElapseTime + strPlayerAge + ExecFuncsMngr.Instance.secretKey);
    //    string post_url_walk = addSpeedURL +
    //        "gameplayid=" + GP_ID +
    //        "&speedmode=WALK" +
    //        "&elapsetime=" + walkingElapseTime +
    //        "&playerage=" + strPlayerAge +
    //        "&hash=" + hash_walk;

    //    Debug.Log(post_url_walk);

    //    // Post the URL to the site and create a download object to get the result.
    //    //UnityWebRequest hs_post_walk = new UnityWebRequest(post_url_walk);
    //    WWW hs_post_walk = new WWW(post_url_walk);
    //    yield return hs_post_walk; // Wait until the download is done

    //    //if (hs_post.error != null)
    //    if (!string.IsNullOrEmpty(hs_post_walk.error))
    //    {
    //        Debug.Log("There was an error trying to save Speed [WALK]: " + hs_post_walk.error);
    //    }

    //    string hash_run = ExecFuncsMngr.Instance.Md5Sum(GP_ID + "RUN" + runningElapseTime + strPlayerAge + ExecFuncsMngr.Instance.secretKey);
    //    string post_url_run = addSpeedURL +
    //        "gameplayid=" + GP_ID +
    //        "&speedmode=RUN" +
    //        "&elapsetime=" + runningElapseTime +
    //        "&playerage=" + strPlayerAge +
    //        "&hash=" + hash_run;

    //    Debug.Log(post_url_run);

    //    // Post the URL to the site and create a download object to get the result.
    //    //UnityWebRequest hs_post_run = new UnityWebRequest(post_url_run);
    //    WWW hs_post_run = new WWW(post_url_run);
    //    yield return hs_post_run; // Wait until the download is done

    //    //if (hs_post.error != null)
    //    if (!string.IsNullOrEmpty(hs_post_run.error))
    //    {
    //        Debug.Log("There was an error trying to save Speed [RUN]: " + hs_post_run.error);
    //    }

    //    //yield return null; // wait until foreach loop is finished

    //    ExecFuncsMngr.Instance.StatesSaved = true;
    //}

    //public void Speed_TimeElapsed()
    //{

    //    DateTime CurrDateTime;
    //    DateTime LastDateTime = Convert.ToDateTime("1900-01-01");

    //    DateTime ChallengeDateTime_Start = Convert.ToDateTime("1900-01-01");
    //    DateTime ChallengeDateTime_End = Convert.ToDateTime("1900-01-01");

    //    string lastSpeedMode = "";
    //    foreach (ExecFuncsSpeedData item in ScrObjSpeedDB.SpeedMeasurements)
    //    {
    //        //Debug.Log("item.value: " + item.value);
    //        if (item.speedmode.IndexOf("HiS") > -1
    //            || item.speedmode.IndexOf("LoS") > -1
    //            || item.speedmode.IndexOf("IDLE") > -1)
    //        {
    //            if (ConvertToDateTime(item.timestamp, out CurrDateTime))
    //            {
    //                // Consider the time that has elapsed with lastSpeedMeasurement
    //                if (lastSpeedMode == "")
    //                    lastSpeedMode = item.speedmode;
    //                else
    //                {
    //                    // Initialize Diff in zero
    //                    TimeSpan Diff = CurrDateTime - CurrDateTime;

    //                    if (LastDateTime != Convert.ToDateTime("1900-01-01"))
    //                        Diff = CurrDateTime - LastDateTime;

    //                    switch (lastSpeedMode)
    //                    {
    //                        case "IDLE":
    //                            idleElapseTime += (float)Diff.TotalSeconds;
    //                            break;
    //                        case "IDLE_CHKP":
    //                            idleElapseTime += (float)Diff.TotalSeconds;
    //                            break;
    //                        case "IDLE_MSG":
    //                            idleElapseTime += (float)Diff.TotalSeconds;
    //                            break;
    //                        case "HiSR":
    //                            runningElapseTime += (float)Diff.TotalSeconds;
    //                            break;
    //                        case "LoSR":
    //                            walkingElapseTime += (float)Diff.TotalSeconds;
    //                            break;
    //                        case "LoSL":
    //                            walkingElapseTime += (float)Diff.TotalSeconds;
    //                            break;
    //                        case "HiSL":
    //                            runningElapseTime += (float)Diff.TotalSeconds;
    //                            break;
    //                        default:
    //                            break;
    //                    }
    //                }

    //                if (item.timestamp != lastSpeedMode)
    //                    lastSpeedMode = item.speedmode;

    //                LastDateTime = CurrDateTime;
    //            }
    //        }
    //    }
    //    // Round results to 3 digits
    //    idleElapseTime = Mathf.Round(idleElapseTime * 1000.0f) / 1000.0f;
    //    walkingElapseTime = Mathf.Round(walkingElapseTime * 1000.0f) / 1000.0f;
    //    runningElapseTime = Mathf.Round(runningElapseTime * 1000.0f) / 1000.0f;
    //}

    //bool ConvertToDateTime(String arg_Str, out DateTime arg_DT)
    //{
    //    try
    //    {
    //        arg_DT = DateTime.ParseExact(arg_Str, "yyyy-MM-dd HH:mm:ss,fff", CultureInfo.InvariantCulture);
    //        return true;
    //    }
    //    catch
    //    {
    //        arg_DT = Convert.ToDateTime("1900-01-01");
    //        return false;
    //    }
    //}

}
