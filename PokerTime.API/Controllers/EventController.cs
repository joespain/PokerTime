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
    [Route("api/events")]
    [Authorize("api-access")]
    public class EventController : PokerTimeControllerBase
    {
        public EventController(IPTRepository repository, IMapper mapper, LinkGenerator linkGenerator) : base(repository, mapper, linkGenerator)
        {

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventModel>>> GetEvents()
        {
            try
            {
                var events = await _repository.GetAllEventsByHostIdAsync(GetHostId());

                if (events == null)
                {
                    events = new List<Event>();
                }

                return _mapper.Map<IEnumerable<EventModel>>(events).ToList();
            }
            catch (Exception e)
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpGet("{eventId:guid}")]
        public async Task<ActionResult<EventModel>> GetEvent(Guid eventId)
        {
            try
            {
                if (eventId == Guid.NewGuid()) return BadRequest("Event not found.");

                return _mapper.Map<EventModel>(await _repository.GetEventByIdAsync(eventId));
            }
            catch (Exception e)
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<EventModel>> AddEvent([FromBody] EventModel model)
        {
            try
            {
                var newEvent = _mapper.Map<Event>(model);
                bool allInviteesAreNew = true;

                foreach(var invitee in newEvent.Invitees)
                {
                    if(invitee.Id != 0)
                    {
                        allInviteesAreNew = false;
                    }
                }

                if (allInviteesAreNew)
                {
                    _repository.Add(newEvent);
                }
                else
                {
                    if(await _repository.AddNewEvent(newEvent))
                    {
                        return Created($"api/events/{newEvent.Id}", _mapper.Map<EventModel>(newEvent));
                    }
                    else
                    {
                        return BadRequest("Error adding event.");
                    }
                }

                if(await _repository.SaveChangesAsync())
                {
                    return Created($"api/events/{newEvent.Id}", _mapper.Map<EventModel>(newEvent));
                }
                else
                {
                    return BadRequest("Error adding event.");
                }
            }
            catch (Exception e)
            {
                //replace with real error code
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpPut("{eventId:guid}")]
        public async Task<ActionResult<EventModel>> UpdateEvent(Guid eventId, [FromBody] EventModel eventToUpdateModel)
        {
            try
            {
                var eventToUpdate = _mapper.Map<Event>(eventToUpdateModel);
                ////Make sure the Id URI is the same as the model.Id. If they're different, something's wrong.
                if (eventId != eventToUpdate.Id)
                {
                    return BadRequest("Error updating event.");
                }

                if (await _repository.UpdateEvent(eventToUpdate)) //Returns true if update is successful
                {
                    return _mapper.Map<EventModel>(eventToUpdate);
                }
                else
                {
                    return BadRequest("Error updating the event.");
                }
                
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpDelete("{eventId:guid}")]
        public async Task<ActionResult> DeleteEvent(Guid eventId)
        {
            try
            {
                if (eventId == Guid.NewGuid()) return BadRequest("No event to delete.");
                if (await _repository.DeleteEventByIdAsync(eventId))
                {
                    return NoContent();  //Success
                }
                else
                {
                    return BadRequest("Error deleting event.");
                }
            }
            catch (Exception e)
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        private async Task UpdateInvitees(List<Invitee> invitees)
        {
            List<Invitee> oldInvitees = (await _repository.GetAllInviteesByHostIdAsync(GetHostId())).ToList();
            _mapper.Map(invitees, oldInvitees);
            await _repository.SaveChangesAsync();
        }
    }
}
