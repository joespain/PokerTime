using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokerTime.App.Client.Interfaces
{
    public interface IUserDataService
    {
        Task<User> AddUser(User user);
        Task DeleteUser(Guid Id);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUser(Guid Id);
        Task UpdateUser(User user);
    }
}