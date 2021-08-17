using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokerTime.App.Interfaces
{
    public interface IEventDataService
    {
        Task<Event> AddEvent(Event newEvent);
        Task DeleteEvent(Guid eventId);
        Task<Event> GetEvent(Guid eventId);
        Task<IEnumerable<Event>> GetEvents();
        Task UpdateEvent(Event updateEvent);
    }
}