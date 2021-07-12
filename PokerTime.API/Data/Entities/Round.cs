namespace PokerTime.API.Data.Entities
{
    public class Round
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int RoundNumber { get; set; }
        public int SmallBlind { get; set; }
        public int BigBlind { get; set; }
        public int Ante { get; set; }
        public int Minutes { get; set; }

    }
}
