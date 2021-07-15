using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.API.Data.Entities
{
    public class User
    {

        //Eventually I want to be able to connect users together so each user has a list of Friends(people they've invited before)
        //but I can't figure out how to do a self-referencing relationship with EF Core.
        //public User()
        //{
        //    this.Invitees = new List<Invitee>();
        //    this.TournamentStructures = new List<TournamentStructure>();
        //    this.Events = new List<Event>();
        //}

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsPaidUser { get; set; }

        public ICollection<TournamentStructure> TournamentStructures { get; set; }
        public ICollection<Invitee> Invitees { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
