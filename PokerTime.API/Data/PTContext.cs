using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PokerTime.API.Data.Entities;

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
        }

    }

}
