using System.Collections.Generic;

namespace IntelliChallenge.RavenMatrix
{
    public class ChallengeElement
    {
        public List<ChallengeItem> itemsList { get; set; }

        public ChallengeElement()
        {
            itemsList = new List<ChallengeItem>();
        }
        
    }
}