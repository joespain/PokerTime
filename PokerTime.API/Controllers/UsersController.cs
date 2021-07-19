﻿using AutoMapper;
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
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            try
            {
                var users = await _repository.GetAllUsers();

                return _mapper.Map<IEnumerable<UserModel>>(users).ToList();
            }
            catch(Exception e)
            {
                //replace with real error code
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpGet("{userId:int}")]
        public async Task<ActionResult<UserModel>> GetUser(int userId)
        {
            try
            {
                var result = await _repository.GetUserByIdAsync(userId);

                if(result == null)
                {
                    return BadRequest("User does not exist.");
                }

                return _mapper.Map<UserModel>(result);
            }
            catch(Exception e)
            {
                //replace with real error code
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserModel>> AddUser(UserModel model)
        {
            try
            {
                //What if user with same name/email/phone exists?
                var user = _repository.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    return BadRequest("User already exists");
                }

                var newUser = _mapper.Map<User>(model);

                _repository.Add(newUser);
                
                if (await _repository.SaveChangesAsync())
                {

                    //Figure out why this linkgenerator isn't working.
                    //var location = _linkGenerator.GetPathByAction(action: "Get", controller: "Users", values: new { userId = newUser.Id });

                    //if (location == null) return BadRequest("Error saving user, location");

                    return Created($"/Users/{newUser.Id}", _mapper.Map<UserModel>(newUser));
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
        public async Task<ActionResult<UserModel>> UpdateUser(int userId, [FromBody] UserModel newUser)
        {
            try
            {
                var oldUser = await _repository.GetUserByIdAsync(userId);
                if (oldUser == null)
                {
                    return BadRequest("User to update was not found.");
                }

                //Mapper applies the changes of the model onto the user, updating the entity in the process.
                _mapper.Map(newUser, oldUser);

                if(await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<UserModel>(newUser);
                }
                else
                {
                    return BadRequest("Error updating the user.");
                }
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

                if(await _repository.DeleteUserByIdAsync(userId))
                {
                    return NoContent();  //Success
                }
                else
                {
                    return BadRequest("Error deleting User.");
                }
                
            }
            catch(Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

    }
}
