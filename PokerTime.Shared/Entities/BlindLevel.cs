namespace PokerTime.Shared.Entities
{
    public class BlindLevel
    {
        public int Id { get; set; }
        public int TournamentStructureId { get; set; }
        public int SmallBlind { get; set; }
        public int BigBlind { get; set; }
        public int Ante { get; set; }
        public int Minutes { get; set; }
        public int SequenceNum { get; set; }
    }
}
