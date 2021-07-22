using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.Shared.Models
{
    public class EventModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public int UserId { get; set; }
        public int TournamentStructureId { get; set; }
        public ICollection<InviteeModel> Invitees { get; set; }
    }
}
