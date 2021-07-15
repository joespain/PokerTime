using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.API.Data.Entities
{
    public class Event
    {
        //public Event()
        //{
        //    Invitees = new List<Invitee>();
        //}
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public int HostId { get; set; }
        public User Host { get; set; }
        public DateTime Date { get; set; }

        public int TournamentStructureId { get; set; }
        public TournamentStructure Structure { get; set; }
        public ICollection<Invitee> Invitees { get; set; }
    }
}
