using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokerTime.App.Interfaces
{
    public interface IHostDataService
    {
        Task<Host> AddHost(Host host);
        Task DeleteHost(Guid Id);
        Task<IEnumerable<Host>> GetAllHosts();
        Task<Host> GetHost(Guid Id);
        Task UpdateHost(Host host);
    }
}