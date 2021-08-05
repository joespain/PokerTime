using System.ComponentModel.DataAnnotations;

namespace PokerTime.Shared.Models
{
    public class BlindLevelModel
    {
        public int Id { get; set; }
        [Required]
        public int TournamentStructureId { get; set; }
        [Required]
        public int SmallBlind { get; set; }
        [Required]
        public int BigBlind { get; set; }
        public int Ante { get; set; }
        [Required]
        [Range(1,int.MaxValue,ErrorMessage ="Please enter a value greater than 0")]
        public int Minutes { get; set; }
        [Required]
        public int SequenceNum { get; set; }
    }
}
