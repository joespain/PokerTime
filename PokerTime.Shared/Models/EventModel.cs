using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.Shared.Models
{
    public class EventModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(75)]
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public Guid EventLinkId { get; set; }

        [Required]
        public Guid HostId { get; set; }
        [Required]
        public int TournamentStructureId { get; set; }
        [ValidateComplexType]
        public ICollection<InviteeModel> Invitees { get; set; }
    }
}
