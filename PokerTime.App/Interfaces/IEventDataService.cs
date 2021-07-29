using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokerTime.App.Interfaces
{
    public interface IEventDataService
    {
        Task<Event> AddEvent(Event newEvent, Guid hostId);
        Task DeleteEvent(int eventId, Guid hostId);
        Task<Event> GetEvent(int eventId, Guid hostId);
        Task<IEnumerable<Event>> GetEvents(Guid hostId);
        Task UpdateEvent(Event updateEvent);
    }
}