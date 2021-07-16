using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.API.Data.Entities
{
    public class Invitee
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required][EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }


        public ICollection<Invitee> Events { get;set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
