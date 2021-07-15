using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.API.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsPaidUser { get; set; }

        public ICollection<TournamentStructureModel> TournamentStructures { get; set; }
        public ICollection<InviteeModel> Invitees { get; set; }
        public ICollection<EventModel> Events { get; set; }
    }
}
