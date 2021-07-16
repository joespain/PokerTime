using PokerTime.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokerTime.API.Data
{
    public interface IPTRepository
    {
        //Adding, Removing & Editing
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;


        //Get Tournament Structures
        Task<IEnumerable<TournamentStructure>> GetTournamentStructuresByUserIdAsync(int id);
        Task<TournamentStructure> GetTournamentStructureByIdAsync(int id);
        Task<bool> DeleteTournamentStructure(int id);

        //Get Users
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int id);
        Task<bool> DeleteUser(int userId);

        //Get Invitees
        Task<IEnumerable<Invitee>> GetAllInviteesByUserIdAsync(int id);
        Task<Invitee> GetInviteeByIdAsync(int id);
        Task<bool> DeleteInvitee(int id);

        //Save
        Task<bool> SaveChangesAsync();
    }
}