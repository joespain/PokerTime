using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PokerTime.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.API.Data
{
    public class PTRepository : IPTRepository
    {
        private readonly PTContext _context;
        private readonly ILogger<PTRepository> _logger;

        public PTRepository(PTContext context, ILogger<PTRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Add<T>(T entity) where T : class
        {
            _logger.LogInformation($"Adding an object of type {entity.GetType()} to the context.");
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _logger.LogInformation($"Removing an object of type {entity.GetType()} to the context.");
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            _logger.LogInformation($"Attempting to save the changes in the context");

            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Tournament[]> GetAllTournamentsByIdAsync(Guid id)
        {
            _logger.LogInformation($"Getting all Tournaments");

            IQueryable<Tournament> query = _context.Tournaments.Where(c => c.UserId == id);


            // Order It
            query = query.OrderBy(c => c.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Guest[]> GetAllGuestsByIdAsync(Guid id)
        {
            _logger.LogInformation($"Getting all guests");

            IQueryable<Guest> query = _context.Guests.Where(g => g.UserId == id);

            query = query.OrderBy(g => g.Name);

            return await query.ToArrayAsync();
        }


    }
}
