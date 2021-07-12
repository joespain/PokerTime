using Microsoft.EntityFrameworkCore;
using PokerTime.API.Data.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.API.Data
{
    public class PTContext : DbContext
    {
        private readonly IConfiguration _config;
        public PTContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Guest> Guests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("PokerTime"));
        }

        protected override void OnModelCreating(ModelBuilder bldr)
        {
            bldr.Entity<Tournament>()
                .HasData(new 
                {
                    Id = 1,
                    Name = "Joe's Tournament of Champions",
                    HostId = new Guid { }
                });

            bldr.Entity<Round>()
                .HasData(new 
                {
                    Id = 1,
                    TournamentId = 1,
                    RoundNumber = 1,
                    Ante = 20,
                    BigBlind = 100,
                    SmallBlind = 200,
                    Minutes = 30
                });

            bldr.Entity<Round>()
                .HasData(new
                {
                    Id = 2,
                    TournamentId = 1,
                    RoundNumber = 2,
                    Ante = 25,
                    BigBlind = 150,
                    SmallBlind = 250,
                    Minutes = 30
                });

            //bldr.Entity<Guest>()
            //    .HasData(new
            //    {
            //        Id = 1,
            //        Name = "Billy Bob",
            //        Email = "Jimbo@gmail.com"
            //    });
        }

    }

}
