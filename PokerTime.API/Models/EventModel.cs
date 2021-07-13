using PokerTime.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.API.Models
{
    public class EventModel
    {
        public int Id { get; set; }
        public ICollection<UserModel> Guests { get; set; }
        public UserModel Host { get; set; }
        public DateTime Date { get; set; }
        public TournamentStructureModel Structure { get; set; }
    }
}
