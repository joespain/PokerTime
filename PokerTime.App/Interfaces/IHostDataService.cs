using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokerTime.App.Interfaces
{
    public interface IHostDataService
    {
        Task<Host> AddHost(Host host);
        Task DeleteHost();
        //Task<IEnumerable<Host>> GetAllHosts();
        Task<Host> GetHost();
        Task UpdateHost(Host host);
    }
}