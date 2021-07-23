using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.Shared.Entities
{
    public class Event
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public Guid HostId { get; set; }
        public int TournamentStructureId { get; set; }
        public ICollection<Invitee> Invitees { get; set; }
    }
}
