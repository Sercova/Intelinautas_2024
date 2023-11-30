using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;


//[System.Serializable]
//public enum Scope
//{
//    TIME,
//    QUANTITY
//};


[System.Serializable]
public class ExecFuncsGamePlayData
{
    [XmlElement("player_name")]
    public string player_name;
    [XmlElement("MAC")]
    public string MAC;
    [XmlElement("gameplay_name")]  // scene_name +  launch_datetime
    public string gameplay_name;

}


[System.Serializable]
public class ExecFuncsGamePlayStates
{
    [XmlElement("Seq")]
    public int StateSequence;
    [XmlElement("PState")]
    public string State;
    [XmlElement("Effort")]
    public float Effort;
}