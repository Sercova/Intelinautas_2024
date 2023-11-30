using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Globalization;

[System.Serializable]
[XmlRoot("Intelinautas")]
public class ExecFuncsDB
{
    [XmlArray("Measurements")]
    [XmlArrayItem("Measure")]
    public List<ExecFuncsData> Measurements = new List<ExecFuncsData>();

    internal void AddMeasurement(string arg_measurement)
    {
        ExecFuncsData element = new ExecFuncsData();
        //element.interaction_name = arg_IntName;
        //element.interaction_datetime = arg_IntDateTime;
        element.scope = Scope.TIME;
        element.measurement = arg_measurement;
        //element.value = System.DateTime.Now.ToString("o");
        element.value = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff");
        //element.value = System.DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat);

        Measurements.Add(element);
    }

    internal void AddMeasurement(string arg_measurement, int arg_value)
    {
        ExecFuncsData element = new ExecFuncsData();
        //element.interaction_name = arg_IntName;
        //element.interaction_datetime = arg_IntDateTime;
        element.scope = Scope.QUANTITY;
        element.measurement = arg_measurement;
        element.value = arg_value.ToString();

        Measurements.Add(element);
    }
}