using AutoMapper;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PokerTime.API.Data;
using PokerTime.Shared.Entities;
using System;
using System.Linq;
using System.Security.Claims;
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

        public async Task<Guid> GetHostId()
        {
            Guid userId = Guid.Parse(this.User.FindFirstValue("sub"));
            string name = this.User.FindFirstValue(ClaimTypes.Name);
            string email = this.User.FindFirstValue(ClaimTypes.Email);

            if(name is null)
            {
                name = "New User";
            }

            if(email is null)
            {
                email = "newemail@gmail.com";
            }

            Guid hostId = await _repository.GetHostIdByUserId(userId, name, email);

            return hostId;
        }

    }
}
