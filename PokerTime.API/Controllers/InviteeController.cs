using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PokerTime.API.Data;
using PokerTime.Shared.Entities;
using PokerTime.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.API.Controllers
{
    [ApiController]
    [Route("api/invitees")]
    [Authorize("api-access")]
    public class InviteeController : PokerTimeControllerBase
    {
        public InviteeController(IPTRepository repository, IMapper mapper, LinkGenerator linkGenerator) : base(repository, mapper, linkGenerator)
        {

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InviteeModel>>> GetInvitees()
        {
            try
            {
                var invitees = await _repository.GetAllInviteesByHostIdAsync(await GetHostId());

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
        public async Task<ActionResult<InviteeModel>> AddInvitee([FromBody] InviteeModel model)
        {
            try
            {
                //Make sure the host exists
                var host = await _repository.GetHostByIdAsync(await GetHostId());

                if (host == null) return BadRequest("Host not found.");

                model.HostId = host.Id;

                var newInvitee = _mapper.Map<Invitee>(model);

                _repository.Add(newInvitee);

                if (await _repository.SaveChangesAsync())
                {
                    //var location = _linkGenerator.GetPathByAction(HttpContext, "Get",
                    //"Invitees",
                    //values: new { userId, newInvitee.Id});
                    return Created($"api/invitees/{newInvitee.Id}", _mapper.Map<InviteeModel>(newInvitee));
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
        }

        [HttpPut("{inviteeId:int}")]
        public async Task<ActionResult<InviteeModel>> UpdateInvitee(int inviteeId, [FromBody] InviteeModel newInvitee)
        {
            try
            {
                //Make sure the Id URI is the same as the model.Id. If they're different, something's fucked.
                if (inviteeId != newInvitee.Id)
                {
                    return BadRequest("Error updating Invitee.");
                }

                //If the old invitee doesn't exist, it's either deleted or got wrong id.
                var oldInvitee = await _repository.GetInviteeByIdAsync(newInvitee.Id);
                if (oldInvitee == null)
                {
                    return BadRequest("Invitee to update was not found.");
                }

                //Mapper applies the changes of the model onto the entity, updating the entity in the process.
                _mapper.Map(newInvitee, oldInvitee);

                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<InviteeModel>(newInvitee);
                }
                else
                {
                    return BadRequest("Error updating the Invitee.");
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpDelete("{inviteeId:int}")]
        public async Task<ActionResult> DeleteInvitee(int inviteeId)
        {
            try
            {
                if (inviteeId == 0) return BadRequest("No invitee to delete.");
                if(await _repository.DeleteInviteeByIdAsync(inviteeId))
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
