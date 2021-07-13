using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
