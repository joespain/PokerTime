using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokerTime.App.Interfaces
{
    public interface IEventDataService
    {
        Task<Event> AddEvent(Event newEvent);
        Task DeleteEvent(int eventId);
        Task<Event> GetEvent(int eventId);
        Task<IEnumerable<Event>> GetEvents();
        Task UpdateEvent(Event updateEvent);
    }
}