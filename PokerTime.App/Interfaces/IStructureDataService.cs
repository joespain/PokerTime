using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokerTime.App.Interfaces
{
    public interface IStructureDataService
    {
        Task<TournamentStructure> AddStructure(TournamentStructure structure);
        Task DeleteStructure(int structureId);
        Task<TournamentStructure> GetStructure(int structureId);
        Task<IEnumerable<TournamentStructure>> GetStructures();
        Task UpdateStructure(TournamentStructure structure);
        Task IncrementStructurePlayCount(TournamentStructure structure);
    }
}