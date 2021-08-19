using PokerTime.Shared.Converters;
using System;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.Shared.Entities
{
    public class TournamentTracking
    {
        public Guid Id { get; set; }
        public bool IsTournamentRunning { get; set; }
        public bool IsTimerRunning { get; set; }
        [System.Text.Json.Serialization.JsonConverter(typeof(TimeSpanToStringConverter))]
        public TimeSpan TimeRemaining { get; set; }
        public DateTime Time { get; set; }
        public BlindLevel CurrentBlindLevel { get; set; }
        public BlindLevel NextBlindLevel { get; set; }

    }
}
