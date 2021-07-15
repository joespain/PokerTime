using PokerTime.API.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.API.Models
{
    public class InviteeModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }


        public ICollection<Event> Events { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
