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
        Task<IEnumerable<TournamentStructure>> GetTournamentStructuresByUserIdAsync(Guid id);
        Task<TournamentStructure> GetTournamentStructureAsync(int id);

        //Get Users
        Task<IEnumerable<User>> GetAllFriendsByIdAsync(Guid id);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsers();

        //Save
        Task<bool> SaveChangesAsync();
    }
}