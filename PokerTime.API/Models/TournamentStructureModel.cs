using PokerTime.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.API.Models
{
    public class TournamentStructureModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public int NumberOfEvents { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<BlindLevelModel> BlindLevels { get; set; }


        public int HostId { get; set; }
        public UserModel Host { get; set; }
    }
}
