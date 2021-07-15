using PokerTime.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.API.Models
{
    public class EventModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public int HostId { get; set; }
        public UserModel Host { get; set; }
        public DateTime Date { get; set; }

        public int TournamentStructureId { get; set; }
        public TournamentStructureModel Structure { get; set; }
        public ICollection<InviteeModel> Invitees { get; set; }
    }
}
