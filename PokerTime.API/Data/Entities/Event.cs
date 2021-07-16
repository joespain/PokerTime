using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.API.Data.Entities
{
    public class Event
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }

        public int TournamentStructureId { get; set; }
        public TournamentStructure Structure { get; set; }
        public ICollection<Invitee> Invitees { get; set; }
    }
}
