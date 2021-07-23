
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

        //Users 
        Task<IEnumerable<User>> GetAllUsers(bool includeTournamentStructures = false, bool includeInvitees = false, bool includeEvents = false);
        Task<User> GetUserByEmailAsync(string email, bool includeTournamentStructures = false, bool includeInvitees = false, bool includeEvents = false);
        Task<User> GetUserByIdAsync(Guid id, bool includeTournamentStructures = false, bool includeInvitees = false, bool includeEvents = false);
        Task<bool> DeleteUserByIdAsync(Guid id);

        //TournamentStructures 
        Task<IEnumerable<TournamentStructure>> GetTournamentStructuresByUserIdAsync(Guid id);
        Task<TournamentStructure> GetTournamentStructureByIdAsync(int id);
        Task<bool> DeleteTournamentStructureByIdAsync(int id);

        //Invitees
        Task<IEnumerable<Invitee>> GetAllInviteesByUserIdAsync(Guid id);
        Task<Invitee> GetInviteeByIdAsync(int id);
        Task<bool> DeleteInviteeByIdAsync(int id);

        //Events 
        Task<IEnumerable<Event>> GetAllEventsByUserIdAsync(Guid id);
        Task<Event> GetEventByIdAsync(int id);
        Task<bool> DeleteEventByIdAsync(int id);

        //BlindLevels

        Task<IEnumerable<BlindLevel>> GetBlindLevelsByStructureIdAsync(int id);
        Task<BlindLevel> GetBlindLevelByIdAsync(int id);
        Task<bool> DeleteBlindLevelByIdAsync(int id);

        //Save
        Task<bool> SaveChangesAsync();
    }
}