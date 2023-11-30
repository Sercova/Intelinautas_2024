using IntelliChallenge.RavenMatrix.ChallengeFormatter;

namespace IntelliChallenge.RavenMatrix
{
    public class ChallengeItem
    {
        public ItemType type;
        public  float LocalPosition { get; set; }
        public float spin { get; set; }
        
        // size
        public float radius { get; set; }
        public int sidesNumber { get; set; }

        // is On/Off
        public float isOn { get; set; }
        //public float isOn_var { get; set; }

    }
}