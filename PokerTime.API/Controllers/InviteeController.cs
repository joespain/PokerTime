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
    [Route("api/users/{userId}/invitees")]
    public class InviteeController : ControllerBase
    {
        private readonly IPTRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        public InviteeController(IPTRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InviteeModel>>> GetInvitees(int userId)
        {
            try
            {
                if (userId == 0) return BadRequest("User does not exist.");

                var invitees = await _repository.GetAllInviteesByUserIdAsync(userId);

                return _mapper.Map<IEnumerable<InviteeModel>>(invitees).ToList();
            }
            catch(Exception e)
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpGet("{inviteeId:int}")]
        public async Task<ActionResult<InviteeModel>> GetInvitee(int inviteeId)
        {
            try
            {
                if (inviteeId == 0) return BadRequest("Invitee not found.");

                return  _mapper.Map<InviteeModel>(await _repository.GetInviteeByIdAsync(inviteeId));
            }
            catch(Exception e)
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<InviteeModel>> AddInvitee(int userId, InviteeModel model)
        {
            try
            {
                var location = _linkGenerator.GetPathByAction(HttpContext, "Get",
                    "Invitees", 
                    values: new { userId, model.Id });

                var newInvitee = _mapper.Map<Invitee>(model);
                _repository.Add(newInvitee);
                if (await _repository.SaveChangesAsync())
                {
                    return Created(location, _mapper.Map<InviteeModel>(newInvitee));
                }
                else
                {
                    return BadRequest("Error adding Invitee.");
                }
            }
            catch (Exception e)
            {
                //replace with real error code
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteInvitee(int id)
        {
            try
            {
                if (id == 0) return BadRequest("No invitee to delete.");
                if(await _repository.DeleteInvitee(id))
                {
                    return NoContent();  //Success
                }
                else
                {
                    return BadRequest("Error deleting Invitee.");
                }
                
            }
            catch (Exception e)
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
    }
}
