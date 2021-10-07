using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.Shared.Models
{
    public class HostModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        [Required][StringLength(100)]
        public string Name { get; set; }
        [Required][EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsPaidUser { get; set; }

        public ICollection<TournamentStructureModel> TournamentStructures { get; set; }
        public ICollection<InviteeModel> Invitees { get; set; }
        public ICollection<EventModel> Events { get; set; }
    }
}
