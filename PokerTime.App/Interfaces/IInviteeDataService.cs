using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokerTime.App.Interfaces
{
    public interface IInviteeDataService
    {
        Task<Invitee> AddInvitee(Invitee invitee);
        Task DeleteInvitee(int inviteeId);
        Task<Invitee> GetInvitee(int inviteeId);
        Task<IEnumerable<Invitee>> GetInvitees();
        Task UpdateInvitee(Invitee invitee);
    }
}