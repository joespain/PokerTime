using System;

namespace PokerTime.API.Data.Entities
{
    public class Guest
    {
        public int Id { get; set; }
        public Guid HostId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
