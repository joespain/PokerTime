using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PokerTime.API.Data.Entities;
using System;
using System.Collections.Generic;

namespace PokerTime.API.Data
{
    public class PTContext : DbContext
    {
        private readonly IConfiguration _config;
        public PTContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        public DbSet<TournamentStructure> TournamentStructures { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Invitee> Invitees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("PokerTime"));
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder bldr)
        {
            var Jim = new User
            {
                Id = 1,
                Name = "Jim Spain",
                Email = "JimboSpain@gmail.com",
                Phone = "0987654321",
                IsPaidUser = false
            };

            var Joe = new User
            {
                Id = 2,
                Name = "Joe Spain",
                Email = "Joe.Spain22@gmail.com",
                Phone = "7274094210",
                IsPaidUser = false,
            };

            bldr.Entity<User>(u =>
            {
                u.HasData(new
                {
                    Id = 3,
                    Name = "Mike Spain",
                    Email = "MJSpain@gmail.com",
                    Phone = "12354567890",
                    IsPaidUser = true,
                });
            });

            bldr.Entity<User>()
                .HasData(Jim);
            bldr.Entity<User>()
                .HasData(Joe);

            //var bill = new Invitee
            //{
            //    Id = 1,
            //    Name = "Billy Bob",
            //    Email = "BillyB@gmail.com",
            //    Phone = "7274094211",
            //    UserId = 3
            //};
            //var troy = new Invitee
            //{
            //    Id = 2,
            //    Name = "Troy Trofelkers",
            //    Email = "Troy@gmail.com",
            //    Phone = "8184094211",
            //    UserId = 3
            //};

            //var ts1 = new TournamentStructure
            //{
            //    Id = 1,
            //    Name = "Joe's Structure of Champions!",
            //    HostId = 3,
            //    DateCreated = DateTime.Today,
            //    NumberOfEvents = 0
            //};



            //bldr.Entity<Invitee>()
            //    .HasData(bill);
            //bldr.Entity<Invitee>()
            //    .HasData(troy);

            //bldr.Entity<TournamentStructure>()
            //    .HasData(ts1);

            //bldr.Entity<BlindLevel>()
            //    .HasData(new BlindLevel
            //    {
            //        Id = 1,
            //        TournamentStructureId = 1,
            //        LevelNumber = 1,
            //        Ante = 20,
            //        BigBlind = 100,
            //        SmallBlind = 200,
            //        Minutes = 30
            //    });

            //bldr.Entity<BlindLevel>()
            //    .HasData(new BlindLevel
            //    {
            //        Id = 2,
            //        TournamentStructureId = 1,
            //        LevelNumber = 2,
            //        Ante = 25,
            //        BigBlind = 150,
            //        SmallBlind = 250,
            //        Minutes = 30
            //    });


        }

    }

}
