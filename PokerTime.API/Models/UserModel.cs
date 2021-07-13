using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.API.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required][EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsPaidUser { get; set; }
        public ICollection<UserModel> Friends { get; set; }
        public ICollection<TournamentStructureModel> Structures { get; set; }
    }
}
