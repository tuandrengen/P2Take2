namespace P2SeriousGame
{
    class ForeignKey
    {
        public int PersonId { get; set; }
        public int SessionId { get; set; }
        public int RoundsId { get; set; }

        public ForeignKey(int personId, int sessionId, int roundsId)
        {
            PersonId = personId;
            SessionId = sessionId;
            RoundsId = roundsId;
        }
    }
}
