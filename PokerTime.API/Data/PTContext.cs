using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PokerTime.Shared.Entities;
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
        public DbSet<Host> Hosts { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Invitee> Invitees { get; set; }
        public DbSet<BlindLevel> BlindLevels { get; set; }
        public DbSet<TournamentTracking> TournamentTrackings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("PokerTime"));
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder bldr)
        {
            var Jim = new Host
            {
                Id = Guid.NewGuid(),
                Name = "Jim Spain",
                Email = "JimboSpain@gmail.com",
                Phone = "0987654321",
                IsPaidUser = true
            };

            //var Joe = new User
            //{
            //    Id = new System.Guid(),
            //    Name = "Joe Spain",
            //    Email = "Joe.Spain22@gmail.com",
            //    Phone = "7274094210",
            //    IsPaidUser = false,
            //};

            //bldr.Entity<User>(u =>
            //{
            //    u.HasData(new
            //    {
            //        Id = new System.Guid(),
            //        Name = "Mike Spain",
            //        Email = "MJSpain@gmail.com",
            //        Phone = "12354567890",
            //        IsPaidUser = true,
            //    });
            //});

            bldr.Entity<Host>()
                .HasData(Jim);
            //bldr.Entity<User>()
            //    .HasData(Joe);
        }

    }

}
