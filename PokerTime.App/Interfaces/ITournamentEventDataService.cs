using PokerTime.Shared.Entities;
using System;
using System.Threading.Tasks;

namespace PokerTime.App.Interfaces
{
    public interface ITournamentEventDataService
    {
        Task<TournamentTracking> AddTournamentTracking(TournamentTracking tracking);
        Task EndTournamentTracking(TournamentTracking tracking);
        Task UpdateTournamentTracking(TournamentTracking tracking);
        Task<TournamentTracking> GetTournamentTracking(Guid trackingId);
    }
}