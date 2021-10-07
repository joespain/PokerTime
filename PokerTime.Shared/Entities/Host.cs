using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokerTime.Shared.Entities
{
    public class Host
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsPaidUser { get; set; }

        public ICollection<TournamentStructure> TournamentStructures { get; set; }
        public ICollection<Invitee> Invitees { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
