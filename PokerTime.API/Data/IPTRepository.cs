
using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokerTime.API.Data
{
    public interface IPTRepository
    {
        //Adding & Deleting
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;

        //Hosts
        public Task<Guid> GetHostIdByUserId(Guid userId, string name, string email);
        Task<IEnumerable<Host>> GetAllHosts(bool includeTournamentStructures = false, bool includeInvitees = false, bool includeEvents = false);
        Task<Host> GetHostByEmailAsync(string email, bool includeTournamentStructures = false, bool includeInvitees = false, bool includeEvents = false);
        Task<Host> GetHostByIdAsync(Guid id);
        Task<bool> DeleteHostByIdAsync(Guid id);

        //TournamentStructures 
        Task<IEnumerable<TournamentStructure>> GetTournamentStructuresByHostIdAsync(Guid id);
        Task<TournamentStructure> GetTournamentStructureByIdAsync(int id);
        Task<bool> DeleteTournamentStructureByIdAsync(int id);

        //Invitees
        Task<IEnumerable<Invitee>> GetAllInviteesByHostIdAsync(Guid id);
        Task<Invitee> GetInviteeByIdAsync(int id);
        Task<bool> DeleteInviteeByIdAsync(int id);

        //Events 
        Task<IEnumerable<Event>> GetAllEventsByHostIdAsync(Guid id);
        Task<Event> GetEventByIdAsync(Guid id);
        Task<bool> DeleteEventByIdAsync(Guid id);
        Task<bool> AddNewEvent(Event theEvent);
        Task<bool> UpdateEvent(Event theEvent);

        //BlindLevels

        Task<IEnumerable<BlindLevel>> GetBlindLevelsByStructureIdAsync(int id);
        Task<BlindLevel> GetBlindLevelByIdAsync(int id);
        Task<bool> DeleteBlindLevelByIdAsync(int id);

        //TournamentTracking
        Task<bool> AddTournamentTracking(TournamentTracking tracking);
        Task<TournamentTracking> GetTournamentTrackingById(Guid id);
        Task<bool> UpdateTournamentTracking(TournamentTracking tracker);
        Task<bool> DoesTournamentTrackingExist(Guid id);

        //Save
        Task<bool> SaveChangesAsync();
    }
}