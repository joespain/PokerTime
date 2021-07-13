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

        public async Task<IEnumerable<TournamentStructure>> GetTournamentStructuresByUserIdAsync(Guid id)
        {
            _logger.LogInformation($"Getting all Tournaments");

            IQueryable<TournamentStructure> query = _context.TournamentStructures.Where(c => c.Host.Id == id);

            query = query.OrderBy(c => c.Name);

            return await query.ToListAsync();
        }

        public async Task<TournamentStructure> GetTournamentStructureAsync(int id)
        {
            _logger.LogInformation($"Getting Tournament");

            IQueryable<TournamentStructure> query = _context.TournamentStructures.Where(c => c.Id == id);

            return await query.FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<User>> GetAllUsers()
        {
            _logger.LogInformation($"Getting all users");

            IQueryable<User> query = _context.Users;

            query = query.OrderBy(u => u.Name);

            return await query.ToListAsync();
        }

        public async Task<bool> AddFriendByUserIdAsync(Guid UserId, Guid FriendId)
        {
            _logger.LogInformation($"Adding Friend");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            var friend = await _context.Users.FirstOrDefaultAsync(f => f.Id == FriendId);

            user.Friends.Add(friend);

            return await SaveChangesAsync();


        }

        public async Task<IEnumerable<User>> GetAllFriendsByIdAsync(Guid id)
        {
            _logger.LogInformation($"Getting all Friends");

            IQueryable<User> query = _context.Users.Where(u => u.Id == id);

            query = query.OrderBy(g => g.Name);

            return await query.ToArrayAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            _logger.LogInformation($"Getting user with email {email}");

            IQueryable<User> query = _context.Users.Where(u => u.Email == email);

            return await query.FirstOrDefaultAsync();

        }


    }
}
