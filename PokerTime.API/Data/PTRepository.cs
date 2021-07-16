using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PokerTime.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokerTime.API.Data.Shared;

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
            _logger.LogInformation($"Attempting to save the changes in the context.");

            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }

        //TournamentStructures----------------------------------------------------

        public async Task<IEnumerable<TournamentStructure>> GetTournamentStructuresByUserIdAsync(int id)
        {

            _logger.LogInformation(GetLogString("Deleting", "TournamentStructure", $"{id}", "", $"User"));

            IQueryable<TournamentStructure> query = _context.TournamentStructures
                .Include(c => c.Host)
                .Include(c => c.BlindLevels)
                .Where(c => c.HostId == id);
            
            query = query.OrderBy(c => c.Name);

            return await query.ToListAsync();
        }

        public async Task<TournamentStructure> GetTournamentStructureByIdAsync(int id)
        {
            _logger.LogInformation(GetLogString("Getting", "TournamentStructures", $"{id}", "", $"User"));

            IQueryable<TournamentStructure> query = _context.TournamentStructures
                .Include(c => c.BlindLevels)
                .Include(c => c.Host)
                .Where(c => c.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteTournamentStructure(int id)
        {
            var foundTournamentStructure = await _context.TournamentStructures.FirstOrDefaultAsync(s => s.Id == id);
            if (foundTournamentStructure == null) return false;

            _logger.LogInformation(GetLogString("Deleting", "User", $"{id}", $"{foundTournamentStructure.Name}"));

            Delete(foundTournamentStructure);

            if (await _context.SaveChangesAsync() > 0) //Success
            {
                return true;
            }
            else return false;
        }

        //Users-------------------------------------------------

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            _logger.LogInformation($"Getting all users.");

            IQueryable<User> query = _context.Users.OrderBy(u => u.Name);

            return await query.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            _logger.LogInformation(GetLogString("Getting", "User", $"{id}", $"{foundUser.Name}"));

            return foundUser;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            _logger.LogInformation("Getting user by email.");

            return foundUser;
        }


        public async Task<bool> DeleteUser(int id)
        {
            var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            _logger.LogInformation(GetLogString("Deleting", "User", $"{id}", $"{foundUser.Name}"));

            Delete(foundUser);
            if (await _context.SaveChangesAsync() > 0) //Success
            {
                return true;
            }
            else return false;
        }



        //Invitees----------------------------------------

        public async Task<IEnumerable<Invitee>> GetAllInviteesByUserIdAsync(int id)
        {
            IQueryable<Invitee> query = _context.Invitees
                .Where(u => u.UserId == id)
                .OrderBy(u => u.Name);

            _logger.LogInformation(GetLogString("Getting","Invitees",$"{id}", "", "user"));

            return await query.ToListAsync();
        }

        public async Task<Invitee> GetInviteeByIdAsync(int id)
        {
            _logger.LogInformation($"Getting invitee with Id: {id}");

            return await _context.Invitees.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<bool> DeleteInvitee(int id)
        {
            var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            _logger.LogInformation(GetLogString("Deleting", "Invitee", $"{id}", $"{foundUser.Name}"));

            Delete(foundUser);
            if (await _context.SaveChangesAsync() > 0) //Success
            {
                return true;
            }
            else return false;
        }

        //Events---------------------------------------


        public async Task<IEnumerable<Event>> GetAllEventsByUserIdAsync(int id)
        {
            IQueryable<Event> query = _context.Events
                .Where(u => u.UserId == id)
                .OrderBy(u => u.Name);

            _logger.LogInformation(GetLogString("Getting", "Events", $"{id}", "", "user"));

            return await query.ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            _logger.LogInformation(GetLogString("Deleting", "Event", $"{id}", "", "User"));

            return await _context.Events.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<bool> DeleteEvent(int id)
        {
            var foundEvent = await _context.Events.FirstOrDefaultAsync(u => u.Id == id);

            _logger.LogInformation(GetLogString("Deleting", "Event", $"{id}", $"{foundEvent.Name}"));

            Delete(foundEvent);
            if (await _context.SaveChangesAsync() > 0) //Success
            {
                return true;
            }
            else return false;
        }





        //Log methods
        public string GetLogString(string action, string type, string id, string name)
        {
            return $"{action} {type}: {id}, {name} at {DateTime.Now}";
        }

        public string GetLogString(string action, string type, string id, string name, string forObject)
        {
            return $"{action} {type} for {forObject}: {id}, at {DateTime.Now}";
        }


    }
}
