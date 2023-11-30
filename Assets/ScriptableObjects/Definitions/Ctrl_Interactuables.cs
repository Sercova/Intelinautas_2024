using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Ctrl_Interactuables : MonoBehaviour
{
    public InteractuableReference CurrentInteractuable;
    public FloatReference InteractuableTime;
    public Slider timerBar;
    public float changeTolerance;

    //void LateUpdate()
    //{
    //    float mod = Time.time % InteractuableTime.Variable.Value;
    //    timerBar.value = mod;
    //    if (mod < changeTolerance)
    //        CurrentInteractuable.Variable.Value = "-";
    //}
}
