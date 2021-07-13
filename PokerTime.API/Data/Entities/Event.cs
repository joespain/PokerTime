using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.API.Data.Entities
{
    public class Event
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public User Host { get; set; }
        public DateTime Date { get; set; }
        public TournamentStructure Structure { get; set; }
        public ICollection<User> Guests { get; set; }
    }
}
