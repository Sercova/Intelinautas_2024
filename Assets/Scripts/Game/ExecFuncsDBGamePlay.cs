using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Globalization;

[System.Serializable]
[XmlRoot("IntelinautasGamePlay")]
public class ExecFuncsDBGamePlay
{
    [XmlArray("GamePlayStates")]
    [XmlArrayItem("State")]
    public List<ExecFuncsGamePlayStates> States = new List<ExecFuncsGamePlayStates>();

    internal void ResetStates()
    {
        States.Clear();
    }


    internal void AddState(int arg_state_sequence, string arg_state, float arg_effort)
    {
        ExecFuncsGamePlayStates element = new ExecFuncsGamePlayStates();
        element.StateSequence = arg_state_sequence;
        element.State = arg_state;
        element.Effort = arg_effort;

        States.Add(element);
    }
}