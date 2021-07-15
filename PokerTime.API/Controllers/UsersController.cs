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

        [HttpGet("{userId:int}")]
        public async Task<ActionResult<UserModel>> Get(int userId)
        {
            try
            {
                var result = await _repository.GetUserByIdAsync(userId);

                return _mapper.Map<UserModel>(result);
            }
            catch
            {
                //replace with real error code
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserModel>> Post(UserModel model)
        {
            try
            {
                //What if user with same name/email/phone exists?
                //var user = _repository.GetUserByEmailAsync(model.Email);
                //if (user != null)
                //{
                //    return BadRequest("User already exists");
                //}

                var location = _linkGenerator.GetPathByAction(HttpContext, "Get",
                    "Users", model.Id);

                var newUser = _mapper.Map<User>(model);
                _repository.Add(newUser);
                if (await _repository.SaveChangesAsync())
                {
                    return Created(location, _mapper.Map<UserModel>(newUser));
                }
            }
            catch (Exception e)
            {
                //replace with real error code
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
            return BadRequest();
        }

        [HttpPut("{userId:int}")]
        public async Task<ActionResult<UserModel>> UpdateUser([FromBody] UserModel user)
        {
            try
            {
                var existingUser = await _repository.GetUserByIdAsync(user.Id);
                if (existingUser == null)
                {
                    return BadRequest("User to update was not found.");
                }

                //var location = _linkGenerator.GetPathByAction("Get",
                //    "Users",
                //    new { Id = user.Id });

                //if(string.IsNullOrEmpty(location))
                //{
                //    return BadRequest("Error updating user.");
                //}

                _mapper.Map(user, existingUser);

                if(await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<UserModel>(user);
                }
                return BadRequest("There was an error updating the user.");
            }
            catch(Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            try
            {
                if (userId == 0) return BadRequest("No user to delete.");

                await _repository.DeleteUser(userId);
                return NoContent();  //Success
            }
            catch(Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

    }
}
