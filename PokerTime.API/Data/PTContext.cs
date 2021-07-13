using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PokerTime.API.Data.Entities;
using System;

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("PokerTime"));
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder bldr)
        {
            //bldr.Entity<User>(entity =>
            //{
            //    entity.HasMany(e => e.Friends)
            //    .WithMany(e=> e.)
            //});

            var Mike = new User
            {
                Id = Guid.NewGuid(),
                Name = "Mike Spain",
                Email = "MJSpain@gmail.com",
                Phone = "12354567890",
                IsPaidUser = true
            };

            var Jim = new User
            {
                Id = Guid.NewGuid(),
                Name = "Jim Spain",
                Email = "JimboSpain@gmail.com",
                Phone = "0987654321",
                IsPaidUser = false
            };

            var Joe = new User
            {
                Id = Guid.NewGuid(),
                Name = "Joe Spain",
                Email = "Joe.Spain22@gmail.com",
                Phone = "7274094210",
                IsPaidUser = false
            };

            //Mike.Friends.Add(Joe);
            //Mike.Friends.Add(Jim);

            bldr.Entity<User>()
                .HasData(Mike);
            bldr.Entity<User>()
                .HasData(Jim);
            bldr.Entity<User>()
                .HasData(Joe);

            bldr.Entity<TournamentStructure>()
                .HasData(new
                {
                    Id = 1,
                    Name = "Joe's Structure of Champions!",
                    UserId = Mike.Id,
                    User = Mike,
                    DateCreated = DateTime.Today,
                    NumberOfEvents = 0
                });

            bldr.Entity<BlindLevel>()
                .HasData(new BlindLevel
                {
                    Id = 1,
                    TournamentStructureId = 1,
                    LevelNumber = 1,
                    Ante = 20,
                    BigBlind = 100,
                    SmallBlind = 200,
                    Minutes = 30
                });

            bldr.Entity<BlindLevel>()
                .HasData(new BlindLevel
                {
                    Id = 2,
                    TournamentStructureId = 1,
                    LevelNumber = 2,
                    Ante = 25,
                    BigBlind = 150,
                    SmallBlind = 250,
                    Minutes = 30
                });


        }

    }

}
