using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokerTime.App.Interfaces
{
    public interface IInviteeDataService
    {
        Task<Invitee> AddInvitee(Invitee invitee, Guid hostId);
        Task DeleteInvitee(int inviteeId, Guid hostId);
        Task<Invitee> GetInvitee(int inviteeId, Guid hostId);
        Task<IEnumerable<Invitee>> GetInvitees(Guid hostId);
        Task UpdateInvitee(Invitee invitee);
    }
}