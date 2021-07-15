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
        Task<TournamentStructure> GetTournamentStructureAsync(int id);

        //Get Users
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserByIdAsync(int id);

        //Get Invitees
        Task<IEnumerable<Invitee>> GetAllInviteesByUserIdAsync(int id);
        

        //Save
        Task<bool> SaveChangesAsync();
    }
}