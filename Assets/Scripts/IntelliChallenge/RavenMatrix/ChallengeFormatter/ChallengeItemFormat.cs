using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IntelliChallenge.RavenMatrix.ChallengeFormatter
{
    [Serializable]
    public class ChallengeItemFormat
    {
        public ItemType itemType;
        public ChallengeItemFormat()
        {
            Behaviors = new List<ItemBehavior>();
        }
  
      
        public List<ItemBehavior> Behaviors;

    }
}