using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace IntelliChallenge.RavenMatrix.ChallengeFormatter
{
    [Serializable]
    public class ChallengeElementFormat
    {
        [FormerlySerializedAs("itemsQty")] public int itemsQty;
        public List<ChallengeItemFormat> ItemFormatsList;

        public ChallengeElementFormat()
        {
            ItemFormatsList = new List<ChallengeItemFormat>();
        }
    }
}