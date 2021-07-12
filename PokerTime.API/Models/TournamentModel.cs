using PokerTime.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.API.Models
{
    public class TournamentModel
    {
        [Required][StringLength(100)]
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public ICollection<Round> Rounds { get; set; }
    }
}
