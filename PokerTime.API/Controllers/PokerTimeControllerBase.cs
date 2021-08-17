using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PokerTime.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.API.Controllers
{
    public abstract class PokerTimeControllerBase : ControllerBase
    {
        protected readonly IPTRepository _repository;
        protected readonly IMapper _mapper;
        protected readonly LinkGenerator _linkGenerator;

        public PokerTimeControllerBase(IPTRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        public Guid GetHostId()
        {
            //Re-do with code to obtain HostId from IDP
            return Guid.Parse("0e9771b9-2112-4256-8594-7cd3af8d34ee");
        }

    }
}
