using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace IntelliChallenge.RavenMatrix.ChallengeFormatter
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BehaviorType
    {
        Spin,
        Sides,
        Position,
        Radius,
        isOn
    }
}