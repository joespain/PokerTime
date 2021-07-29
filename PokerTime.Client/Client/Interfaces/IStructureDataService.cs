using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokerTime.App.Client.Interfaces
{
    public interface IStructureDataService
    {
        Task<TournamentStructure> AddStructure(TournamentStructure structure, Guid hostId);
        Task DeleteStructure(int structureId, Guid hostId);
        Task<TournamentStructure> GetStructure(int structureId, Guid hostId);
        Task<IEnumerable<TournamentStructure>> GetStructures(Guid hostId);
        Task UpdateStructure(TournamentStructure structure);
    }
}