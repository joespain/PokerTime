using System.ComponentModel.DataAnnotations;

namespace PokerTime.API.Models
{
    public class BlindLevelModel
    {
        public int Id { get; set; }
        [Required]
        public int TournamentStructureId { get; set; }
        [Required]
        public int LevelNumber { get; set; }
        [Required]
        public int SmallBlind { get; set; }
        [Required]
        public int BigBlind { get; set; }
        public int Ante { get; set; }
        [Required]
        public int Minutes { get; set; }
    }
}
