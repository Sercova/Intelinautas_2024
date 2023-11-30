using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;


[System.Serializable]
public enum Scope
{
    TIME,
    QUANTITY
};


[System.Serializable]
public class ExecFuncsData
{

    [XmlElement("scope")]
    public Scope scope;
    [XmlElement("measurement")]
    public string measurement;
    [XmlElement("value")]
    public string value;

}

[System.Serializable]
public class ExecFuncsSpeedData
{
    [XmlElement("speedmode")]
    public string speedmode;
    [XmlElement("timestamp")]
    public string timestamp;

    public ExecFuncsSpeedData(string arg_speedmode)
    {
        speedmode = arg_speedmode;
        timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff");
    }

}


[CreateAssetMenu]
public class ExecFuncsDataVariable : ScriptableObject
{

    [XmlElement("scope")]
    public Scope scope;
    [XmlElement("measurement")]
    public string measurement;
    [XmlElement("value")]
    public string value;

    private void OnEnable()
    {
        scope = Scope.TIME;
        measurement = "";
        value = "";
    }
}

[System.Serializable]
public class ExecFuncsDataReference
{
    public bool UseConstant = true;
    public Scope ConstantScope;
    public string ConstantMeasurement;
    public string ConstantValue;
    public ExecFuncsDataVariable Variable;

    public Scope scope
    {
        get { return UseConstant ? ConstantScope : Variable.scope; }
    }

    public string measurement
    {
        get { return UseConstant ? ConstantMeasurement : Variable.measurement; }
    }

    public string value
    {
        get { return UseConstant ? ConstantValue : Variable.value; }
    }
}


[System.Serializable]
public class ExecFuncsDataSummary
{
    [XmlAttribute]
    public string PlayerName;

    [XmlElement("gameplay")]
    public string gameplay;
    [XmlElement("watching")]
    public float watching;
    [XmlElement("playingemptyhands")]
    public float playingemptyhands;
    [XmlElement("playingfullhands")]
    public float playingfullhands;
    [XmlElement("paused")]
    public float paused;
    [XmlElement("levelstarted")]
    public float levelstarted;
    [XmlElement("levelfinished")]
    public float levelfinished;
    [XmlElement("restarted")]
    public float restarted;
    [XmlElement("checkpoint")]
    public float checkpoint;
    [XmlElement("levelmenu")]
    public float levelmenu;
    [XmlElement("failed")]
    public float failed;
    [XmlElement("totalWP")]
    public int totalWP;
    [XmlElement("retry")]
    public int retry;
    [XmlElement("menupause")]
    public int menupause;
    [XmlElement("items")]
    public int items;
    [XmlElement("speedRun")]
    public float speedRun;
    [XmlElement("speedWalk")]
    public float speedWalk;
    [XmlElement("speedIdle")]
    public float speedIdle;
}
