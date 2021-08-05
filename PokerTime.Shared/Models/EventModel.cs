using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.Shared.Models
{
    public class EventModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Guid EventLinkId { get; set; }

        public Guid HostId { get; set; }
        public int TournamentStructureId { get; set; }
    }
}
