using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.Shared.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        [StringLength(75)]
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }

        [Required]
        public Guid HostId { get; set; }
        [Required]
        public int TournamentStructureId { get; set; }
        public List<Invitee> Invitees { get; set; }
    }
}
