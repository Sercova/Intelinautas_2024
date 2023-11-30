using System;
using IntelliChallenge.RavenMatrix.ChallengeFormatter;

namespace IntelliChallenge.RavenMatrix
{
    [Serializable]
    public class CloudResponseChallenge
    {
        public ChallengeFormat Challenge;
        public ChallengeFormat Alternatives;

    }
}