using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace IntelliChallenge.RavenMatrix.ChallengeFormatter
{
    [Serializable]
    public class ChallengeFormat
    {
        [FormerlySerializedAs("elementsQty")] public int elementsQty;
        [FormerlySerializedAs("questionType")] public string questionType;

        public List<ChallengeElementFormat> ElementFormatsList;
        public ChallengeFormat()
        {
            ElementFormatsList = new List<ChallengeElementFormat>();
        }
    }
}