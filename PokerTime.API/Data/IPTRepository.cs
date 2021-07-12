using PokerTime.API.Data.Entities;
using System;
using System.Threading.Tasks;

namespace PokerTime.API.Data
{
    public interface IPTRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        //Get Tournaments
        Task<Tournament[]> GetAllTournamentsByIdAsync(Guid id);
        //Get Guests
        Task<Guest[]> GetAllGuestsByIdAsync(Guid id);
        Task<bool> SaveChangesAsync();
    }
}