using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PokerTime.API.Data;
using PokerTime.API.Data.Entities;
using PokerTime.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IPTRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        public UsersController(IPTRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> Get()
        {
            try
            {
                var users = await _repository.GetAllUsers();

                return _mapper.Map<IEnumerable<UserModel>>(users).ToList();
            }
            catch
            {
                //replace with real error code
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            
        }

        [HttpGet("{UserId:int}")]
        public async Task<ActionResult<UserModel>> Get(int UserId)
        {
            try
            {
                var result = await _repository.GetUserByIdAsync(UserId);

                return _mapper.Map<UserModel>(result);
            }
            catch
            {
                //replace with real error code
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }


        //[HttpGet("{UserId:int}")]
        //public async Task<ActionResult<IEnumerable<TournamentStructureModel>>> Get(int id)
        //{
        //    try
        //    {
        //        var result = await _repository.GetTournamentStructuresByUserIdAsync(id);

        //        return _mapper.Map<IEnumerable<TournamentStructureModel>>(result).ToList();
        //    }
        //    catch
        //    {
        //        //replace with real error code
        //        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
        //    }
        //}

        [HttpPost]
        public async Task<ActionResult<UserModel>> Post(UserModel model)
        {
            try
            {
                var location = _linkGenerator.GetPathByAction("Get",
                    "Users", model.Id);

                //What if user with same name/email/phone exists?
                //var user = _repository.GetUserByEmailAsync(model.Email);
                //if (user != null)
                //{
                //    return BadRequest("User already exists");
                //}

                var newUser = _mapper.Map<User>(model);
                _repository.Add(newUser);
                if(await _repository.SaveChangesAsync())
                {
                    return Created(location, _mapper.Map<UserModel>(newUser));
                }
            }
            catch
            {
                //replace with real error code
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }

    }
}
