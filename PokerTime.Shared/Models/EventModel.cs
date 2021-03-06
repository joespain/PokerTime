using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.Shared.Models
{
    public class EventModel
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(75)]
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }

        [Required]
        public Guid HostId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Structure.")]
        public int TournamentStructureId { get; set; }
        [ValidateComplexType]
        public List<InviteeModel> Invitees { get; set; }
    }
}
