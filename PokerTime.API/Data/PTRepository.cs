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

        //TournamentStructures----------------------------------------------------

        public async Task<IEnumerable<TournamentStructure>> GetTournamentStructuresByUserIdAsync(int id)
        {
            _logger.LogInformation($"Getting all Tournaments");

            IQueryable<TournamentStructure> query = _context.TournamentStructures
                .Include(c => c.Host)
                .Include(c => c.BlindLevels)
                .Where(c => c.HostId == id);

            query = query.OrderBy(c => c.Name);

            return await query.ToListAsync();
        }

        public async Task<TournamentStructure> GetTournamentStructureAsync(int id)
        {
            _logger.LogInformation($"Getting Tournament");

            IQueryable<TournamentStructure> query = _context.TournamentStructures
                .Include(c => c.BlindLevels)
                .Include(c => c.Host)
                .Where(c => c.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task DeleteTournamentStructure(int structureId)
        {
            _logger.LogInformation($"Deleting TournamentStructure with Id {structureId}");


            var foundTournamentStructure = await _context.TournamentStructures.FirstOrDefaultAsync(s => s.Id == structureId);
            if (foundTournamentStructure == null) return;

            _context.TournamentStructures.Remove(foundTournamentStructure);
            await _context.SaveChangesAsync();

            return;
        }

        //Users-------------------------------------------------

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            _logger.LogInformation($"Getting all users");

            IQueryable<User> query = _context.Users
                            .OrderBy(u => u.Name);

            return await query.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            _logger.LogInformation($"Getting User with Id {userId}");

            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task DeleteUser(int userId)
        {
            _logger.LogInformation($"Deleting User with Id {userId}");


            var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (foundUser == null) return;

            _context.Users.Remove(foundUser);
            await _context.SaveChangesAsync();

            return;
        }


        //Invitees----------------------------------------

        public async Task<IEnumerable<Invitee>> GetAllInviteesByUserIdAsync(int id)
        {
            _logger.LogInformation($"Getting all Invitees for user with Id: {id}");

            IQueryable<Invitee> query = _context.Invitees
                .Where(u => u.UserId == id)
                .OrderBy(u => u.Name);

            return await query.ToListAsync();
        }



    }
}
