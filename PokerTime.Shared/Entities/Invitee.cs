using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.Shared.Entities
{
    public class Invitee
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required][EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }

        public Guid HostId { get; set; }
        public List<Event> Events { get; set; }
    }
}
