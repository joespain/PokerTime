using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokerTime.Shared.Entities
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
        public Guid HostId { get; set; }
    }
}
