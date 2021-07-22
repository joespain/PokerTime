using PokerTime.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokerTime.App.Client.Interfaces
{
    public interface IUserDataService
    {
        Task<User> AddUser(User user);
        Task DeleteUser(int userId);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUser(int userId);
        Task UpdateUser(User user);
    }
}