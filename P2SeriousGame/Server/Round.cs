namespace P2SeriousGame
{
    public class Round
    {
        public int RoundNumber { get; set; }
        public float NumberOfClicks { get; set; }
        public float ClicksPerMinute { get; set; }
        public int Win { get; set; }
        public int Loss { get; set; }
        public float TimeUsed { get; set; }
        public int RoundID { get; set; }

        public Round(int roundNumber, float clicks, float clicksAVG, int win, float timeUsed, int roundID)
        {
            this.RoundNumber = roundNumber;
            this.NumberOfClicks = clicks;
            this.ClicksPerMinute = clicksAVG;
            WinOrLoss(win);
            this.TimeUsed = timeUsed;
            this.RoundID = roundID;
        }

        public void WinOrLoss(int win)
        {
            if (win == 1)
            {
                Win = 1;
                Loss = 0;
            }
            else
            {
                Win = 0;
                Loss = 1;
            }
        }
    }
}
