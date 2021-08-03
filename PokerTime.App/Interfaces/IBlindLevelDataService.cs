using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokerTime.App.Interfaces
{
    public interface IBlindLevelDataService
    {
        Task<BlindLevel> AddBlindLevel(BlindLevel blindLevel);
        Task DeleteStructure(BlindLevel blindLevel);
        Task<BlindLevel> GetBlindLevel(int structureId);
        Task<IEnumerable<BlindLevel>> GetBlindLevels(int structureId);
        Task UpdateBlindLevel(BlindLevel blindLevel);
    }
}