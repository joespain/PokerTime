using Detached.Mappers.EntityFramework;
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

        public async Task<Host> GetHostByIdAsync(Guid id)
        {
            //Is there a better way of doing this? I hate this huge if/else statement

            Host foundHost = null;

            foundHost = await _context.Hosts.FirstOrDefaultAsync(u => u.Id == id);

            if (foundHost == null)
                return null;

            _logger.LogInformation($"Getting Host {id}");

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

            _logger.LogInformation($"Deleting Host {id}.");

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

            _logger.LogInformation($"Getting TournamentStructures for Host {id}.");

            IQueryable<TournamentStructure> query = _context.TournamentStructures
                .Include(s => s.BlindLevels)
                .Where(s => s.HostId == id)
                .OrderBy(s => s.DateCreated);

            return await query.ToListAsync();
        }

        public async Task<TournamentStructure> GetTournamentStructureByIdAsync(int id)
        {
            _logger.LogInformation($"Getting TournamentStructure {id}.");

            IQueryable<TournamentStructure> query = _context.TournamentStructures
                .Include(s => s.BlindLevels)
                .Where(s => s.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteTournamentStructureByIdAsync(int id)
        {
            var foundTournamentStructure = await _context.TournamentStructures.FirstOrDefaultAsync(s => s.Id == id);
            if (foundTournamentStructure == null) return false;

            _logger.LogInformation($"Deleting TournamentStructure {foundTournamentStructure.Name}, {id}");

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

            _logger.LogInformation($"Getting Invitees for Host {id}");

            return await query.ToListAsync();
        }

        public async Task<Invitee> GetInviteeByIdAsync(int id)
        {
            _logger.LogInformation($"Getting invitee with Id: {id}.");

            return await _context.Invitees.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<bool> DeleteInviteeByIdAsync(int id)
        {
            var foundInvitee = await _context.Invitees.FirstOrDefaultAsync(u => u.Id == id);

            _logger.LogInformation($"Deleting Invitee {id}.");

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

            _logger.LogInformation($"Getting Events for Host {id}");

            return await query.ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            var foundEvent = await _context.Events
                .Include(e => e.Invitees)
                .FirstOrDefaultAsync(e => e.Id == id);

            _logger.LogInformation($"Getting Event {id}.");

            return foundEvent;
        }

        public async Task<bool> DeleteEventByIdAsync(int id)
        {
            var foundEvent = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);

            _logger.LogInformation($"Deleting Event {id}.");

            Delete(foundEvent);
            if (await _context.SaveChangesAsync() > 0) //Success
            {
                return true;
            }
            else return false;
        }

        public async Task<bool> AddNewEvent(Event theEvent)
        {
            _context.Update(theEvent);

            _logger.LogInformation($"Adding new event.");

            if (await _context.SaveChangesAsync() > 0) //Success
            {
                return true;
            }
            else return false;
        }

        public async Task<bool> UpdateEvent(Event eventToUpdate)
        {
            
            var oldEvent = _context.Events
                .Include(e => e.Invitees)
                .Single(e => e.Id == eventToUpdate.Id);

            var oldInvitees = _context.Invitees
                .Include(i => i.Events)
                .Where(i => i.Events.Contains(oldEvent)).ToList();

            foreach(var invitee in eventToUpdate.Invitees)
            {
                if (invitee.Id == 0)
                {
                    invitee.Events.Add(oldEvent);
                    _context.Add(invitee);
                }
                else if (oldInvitees.Find(i => i.Id == invitee.Id) == null)
                {
                    invitee.Events.Add(oldEvent);
                    _context.Invitees.Update(invitee);
                }
                else
                {
                    var originalInvitee = oldInvitees.Find(i => i.Id == invitee.Id);
                    originalInvitee.Name = invitee.Name;
                    originalInvitee.Phone = invitee.Phone;
                    originalInvitee.Email = invitee.Email;
                    _context.Invitees.Update(originalInvitee);
                }
            }

            foreach(var invitee in oldInvitees)
            {
                if (eventToUpdate.Invitees.Find(i => i.Id == invitee.Id) == null)
                {
                    invitee.Events.Remove(oldEvent);
                    _context.Invitees.Update(invitee);
                }
            }

            await _context.SaveChangesAsync();

            oldEvent.Name = eventToUpdate.Name;
            oldEvent.Date = eventToUpdate.Date;
            oldEvent.Time = eventToUpdate.Time;
            oldEvent.TournamentStructureId = eventToUpdate.TournamentStructureId;
           // _context.Entry(oldEvent.Invitees).State = EntityState.Modified;

            _context.Events.Attach(oldEvent);

            _logger.LogInformation($"Updating Event {oldEvent.Id}");

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

            _logger.LogInformation($"Getting BlindLevels for TournamentStructure {id}");

            return await query.ToListAsync();
        }

        public async Task<BlindLevel> GetBlindLevelByIdAsync(int id)
        {
            var foundBlindLevel = await _context.BlindLevels.FirstOrDefaultAsync(b => b.Id == id);

            _logger.LogInformation($"Getting BlindLevel {id}");

            return foundBlindLevel;
        }

        public async Task<bool> DeleteBlindLevelByIdAsync(int id)
        {
            var foundBlindLevel = await _context.BlindLevels.FirstOrDefaultAsync(b => b.Id == id);

            _logger.LogInformation($"Deleting BlindLevel {id}.");

            Delete(foundBlindLevel);
            if (await _context.SaveChangesAsync() > 0) //Success
            {
                return true;
            }
            else return false;
        }

        //TournamentTracking

        public async Task<TournamentTracking> GetTournamentTrackingById(Guid id)
        {
            var TournamentTracker = await _context.TournamentTrackings.FirstOrDefaultAsync(t => t.Id == id);

            _logger.LogInformation($"Getting TournamentTracking for Tournament {id}.");

            return TournamentTracker;
        }


        public async Task<bool> UpdateTournamentTracking(TournamentTracking tracker)
        {
            _context.TournamentTrackings.Update(tracker);

            _logger.LogInformation($"Updating TournamentTracking for Tournament {tracker.Id}");

            if (await _context.SaveChangesAsync() > 0) //Success
            {
                return true;
            }
            else return false;
        }


    }
}
