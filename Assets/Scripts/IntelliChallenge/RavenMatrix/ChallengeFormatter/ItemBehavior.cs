using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Serialization;

namespace IntelliChallenge.RavenMatrix.ChallengeFormatter
{
    [Serializable]
    public struct ItemBehavior
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public BehaviorType type { get; set; }  // Outline or Solid
        [FormerlySerializedAs("initialValue")] public float initialValue;
        [FormerlySerializedAs("increment")] public float increment;
        [FormerlySerializedAs("isOn")] public float isOn;   // [0.0f, 1.0f]  only 0.0f is OFF
        [FormerlySerializedAs("isOn_var")] public float isOn_var;
    }
}