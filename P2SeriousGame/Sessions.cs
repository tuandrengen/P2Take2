using System.Collections.Generic;

namespace P2SeriousGame
{
    public class Sessions
    {
        public int SessionID { get; set; }
        public List<Round> Rounds = new List<Round>();
    }
}