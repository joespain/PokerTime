﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.API.Models
{
    public class GuestModel
    {
        public Guid UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required][EmailAddress]
        public string Email { get; set; }
    }
}
