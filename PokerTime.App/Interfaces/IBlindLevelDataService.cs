using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokerTime.App.Interfaces
{
    public interface IBlindLevelDataService
    {
        Task<BlindLevel> AddBlindLevel(BlindLevel blindLevel, Guid hostId);
        Task DeleteStructure(BlindLevel blindLevel, Guid hostId);
        Task<BlindLevel> GetBlindLevel(int structureId, Guid hostId);
        Task<IEnumerable<BlindLevel>> GetBlindLevels(int structureId, Guid hostId);
        Task UpdateBlindLevel(BlindLevel blindLevel, Guid hostId);
    }
}