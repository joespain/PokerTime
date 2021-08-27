using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokerTime.Shared.Models
{
    public class InviteeModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }

        public Guid HostId { get; set; }
        public List<EventModel> Events {get;set;}
        //IsDisabled is used on the razor pages to enable/disable the form input.
        public bool IsDisabled { get; set; }  
    }
}
