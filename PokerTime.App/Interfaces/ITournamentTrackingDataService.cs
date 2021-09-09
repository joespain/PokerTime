using PokerTime.Shared.Entities;
using System;
using System.Threading.Tasks;

namespace PokerTime.App.Interfaces
{
    public interface ITournamentTrackingDataService
    {
        Task<TournamentTracking> AddTournamentTracking(TournamentTracking tracking);
        Task<TournamentStructure> GetTournamentStructure(Guid trackingId, int structureId);
        Task<TournamentTracking> GetTournamentTracking(Guid trackingId);
        Task UpdateTournamentTracking(TournamentTracking tracking);
    }
}