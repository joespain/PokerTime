using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.API.Data.Entities
{
    public class User
    {
        public User() 
        {
            Friends = new List<User>();
            TournamentStructures = new List<TournamentStructure>();
        }

        public Guid Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsPaidUser { get; set; }
        public virtual ICollection<User> Friends { get; set; }
        public ICollection<TournamentStructure> TournamentStructures { get; set; }
    }
}
