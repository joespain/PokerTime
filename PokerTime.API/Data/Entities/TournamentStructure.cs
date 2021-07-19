using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.API.Data.Entities
{
    public class TournamentStructure
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public int NumberOfEvents { get; set; }
        public DateTime DateCreated { get; set; }

        public ICollection<BlindLevel> BlindLevels { get; set; }
        public int UserId { get; set; }
    }
}
