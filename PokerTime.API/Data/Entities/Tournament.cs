using System;
using System.Collections.Generic;

namespace PokerTime.API.Data.Entities
{
    public class Tournament
    {
        public int Id { get; set; }
        public Guid HostId { get; set; }
        public string Name { get; set; }
        public ICollection<Round> Rounds { get; set; }

    }
}
