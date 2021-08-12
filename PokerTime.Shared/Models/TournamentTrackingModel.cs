using PokerTime.Shared.Entities;
using System;

namespace PokerTime.Shared.Models
{
    public class TournamentTrackingModel
    {
        public Guid Id { get; set; }
        public bool IsTournamentRunning { get; set; }
        public bool IsTimerRunning { get; set; }
        public DateTime TimeRemaining { get; set; }
        public BlindLevel CurrentBlindLevel { get; set; }
        public BlindLevel NextBlindLevel { get; set; }
    }
}
