using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PokerTime.Shared.Entities;
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
            _logger.LogInformation($"Attempting to save the changes in the context.");

            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }

        //Hosts-------------------------------------------------

        public async Task<IEnumerable<Host>> GetAllHosts(bool includeTournamentStructures = false, bool includeInvitees = false, bool includeEvents= false)
        {
            _logger.LogInformation("Getting all hosts.");

            IQueryable<Host> query =
                _context.Hosts.OrderBy(u => u.Name);

            //Add additional connected entities to the query if requested
            if(includeTournamentStructures)
            {
                query = query.Include(u => u.TournamentStructures);
            }

            if (includeInvitees)
            {
                query = query.Include(u => u.Invitees);
            }

            if (includeEvents)
            {
                query = query.Include(u => u.Events);
            }

            return await query.ToListAsync();
        }

        public async Task<Host> GetHostByIdAsync(Guid id, bool includeTournamentStructures = false, bool includeInvitees = false, bool includeEvents = false)
        {
            //Is there a better way of doing this? I hate this huge if/else statement

            Host foundHost = null;

            foundHost = await _context.Hosts.FirstOrDefaultAsync(u => u.Id == id);

            if (foundHost == null)
                return null;

            _logger.LogInformation(GetLogString("Getting", "Host", $"{id}", $"{foundHost.Name}"));

            return foundHost;
        }

        public async Task<Host> GetHostByEmailAsync(string email, bool includeTournamentStructures = false, bool includeInvitees = false, bool includeEvents = false)
        {
            _logger.LogInformation("Getting host by email.");

            Host foundHost = null;

            if (includeTournamentStructures && includeInvitees && includeEvents)
            {
                foundHost = await _context.Hosts
                    .Include(u => u.Invitees)
                    .Include(u => u.Events)
                    .Include(u => u.TournamentStructures)
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
            else if (includeTournamentStructures && includeInvitees)
            {
                foundHost = await _context.Hosts
                    .Include(u => u.Invitees)
                    .Include(u => u.TournamentStructures)
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
            else if (includeTournamentStructures && includeEvents)
            {
                foundHost = await _context.Hosts
                    .Include(u => u.Events)
                    .Include(u => u.TournamentStructures)
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
            else if (includeEvents && includeInvitees)
            {
                foundHost = await _context.Hosts
                    .Include(u => u.Invitees)
                    .Include(u => u.Events)
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
            else if (includeTournamentStructures)
            {
                foundHost = await _context.Hosts
                    .Include(u => u.TournamentStructures)
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
            else if (includeEvents)
            {
                foundHost = await _context.Hosts
                    .Include(u => u.Events)
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
            else if (includeInvitees)
            {
                foundHost = await _context.Hosts
                    .Include(u => u.Invitees)
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
            else
            {
                foundHost = await _context.Hosts
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
            return foundHost;
        }


        public async Task<bool> DeleteHostByIdAsync(Guid id)
        {
            var foundHost = await _context.Hosts.FirstOrDefaultAsync(u => u.Id == id);

            _logger.LogInformation(GetLogString("Deleting", "Host", $"{id}", $"{foundHost.Name}"));

            Delete(foundHost);
            if (await _context.SaveChangesAsync() > 0) //Success
            {
                return true;
            }
            else return false;
        }

        //TournamentStructures----------------------------------------------------

        public async Task<IEnumerable<TournamentStructure>> GetTournamentStructuresByHostIdAsync(Guid id)
        {

            _logger.LogInformation(GetLogString("Getting", "TournamentStructures", $"{id}", "", $"Host"));

            IQueryable<TournamentStructure> query = _context.TournamentStructures
                .Include(s => s.BlindLevels)
                .Where(s => s.HostId == id)
                .OrderBy(s => s.DateCreated);

            return await query.ToListAsync();
        }

        public async Task<TournamentStructure> GetTournamentStructureByIdAsync(int id)
        {
            _logger.LogInformation(GetLogString("Getting", "TournamentStructure", $"{id}", "", $"Host"));

            IQueryable<TournamentStructure> query = _context.TournamentStructures
                .Include(s => s.BlindLevels)
                .Where(s => s.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteTournamentStructureByIdAsync(int id)
        {
            var foundTournamentStructure = await _context.TournamentStructures.FirstOrDefaultAsync(s => s.Id == id);
            if (foundTournamentStructure == null) return false;

            _logger.LogInformation(GetLogString("Deleting", "TournamentStructure", $"{id}", $"{foundTournamentStructure.Name}"));

            Delete(foundTournamentStructure);

            if (await _context.SaveChangesAsync() > 0) //Success
            {
                return true;
            }
            else return false;
        }

        

        //Invitees----------------------------------------

        public async Task<IEnumerable<Invitee>> GetAllInviteesByHostIdAsync(Guid id)
        {
            IQueryable<Invitee> query = _context.Invitees
                .Where(u => u.HostId == id)
                .OrderBy(u => u.Name);

            _logger.LogInformation(GetLogString("Getting","Invitees",$"{id}", "", "host"));

            return await query.ToListAsync();
        }

        public async Task<Invitee> GetInviteeByIdAsync(int id)
        {
            _logger.LogInformation($"Getting invitee with Id: {id}");

            return await _context.Invitees.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<bool> DeleteInviteeByIdAsync(int id)
        {
            var foundInvitee = await _context.Invitees.FirstOrDefaultAsync(u => u.Id == id);

            _logger.LogInformation(GetLogString("Deleting", "Invitee", $"{id}", $"{foundInvitee.Name}"));

            Delete(foundInvitee);
            if (await _context.SaveChangesAsync() > 0) //Success
            {
                return true;
            }
            else return false;
        }

        //Events---------------------------------------


        public async Task<IEnumerable<Event>> GetAllEventsByHostIdAsync(Guid id)
        {
            IQueryable<Event> query = _context.Events
                .Where(u => u.HostId == id)
                .OrderBy(u => u.Date);

            _logger.LogInformation(GetLogString("Getting", "Events", $"{id}", "", "host"));

            return await query.ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            var foundEvent = await _context.Events
                .Include(e => e.Invitees)
                .FirstOrDefaultAsync(i => i.Id == id);

            _logger.LogInformation(GetLogString("Getting", "Event", $"{id}", ""));

            return foundEvent;
        }

        public async Task<bool> DeleteEventByIdAsync(int id)
        {
            var foundEvent = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);

            _logger.LogInformation(GetLogString("Deleting", "Event", $"{id}", ""));

            Delete(foundEvent);
            if (await _context.SaveChangesAsync() > 0) //Success
            {
                return true;
            }
            else return false;
        }


        //BlindLevels

        public async Task<IEnumerable<BlindLevel>> GetBlindLevelsByStructureIdAsync(int id)
        {
            IQueryable<BlindLevel> query = _context.BlindLevels
                .Where(b => b.TournamentStructureId == id)
                .OrderBy(b => b.SequenceNum);

            _logger.LogInformation(GetLogString("Getting", "BlindLevels", $"{id}", "", "TournamentStructure"));

            return await query.ToListAsync();
        }

        public async Task<BlindLevel> GetBlindLevelByIdAsync(int id)
        {
            var foundBlindLevel = await _context.BlindLevels.FirstOrDefaultAsync(b => b.Id == id);

            _logger.LogInformation(GetLogString("Getting", "BlindLevel", $"{id}", ""));

            return foundBlindLevel;
        }

        public async Task<bool> DeleteBlindLevelByIdAsync(int id)
        {
            var foundBlindLevel = await _context.BlindLevels.FirstOrDefaultAsync(b => b.Id == id);

            _logger.LogInformation(GetLogString("Deleting", "BlindLevel", $"{id}", ""));

            Delete(foundBlindLevel);
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
