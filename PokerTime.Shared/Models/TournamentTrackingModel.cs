using PokerTime.Shared.Converters;
using PokerTime.Shared.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.Shared.Models
{
    public class TournamentTrackingModel
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
