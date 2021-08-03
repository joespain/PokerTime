using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.API.Controllers
{
    public class PokerTimeControllerBase : ControllerBase
    {
        public Guid getHostId()
        {
            //Re-do with code to obtain HostId from IDP
            return Guid.Parse("48b51074-220e-4275-b3f6-ed41b8319832");
        }

    }
}
