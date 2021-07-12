using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PokerTime.API.Data
{
    public class PTContextFactory : IDesignTimeDbContextFactory<PTContext>, IPTContextFactory
    {
        public PTContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

            return new PTContext(new DbContextOptionsBuilder<PTContext>().Options, config);
        }
    }
}
