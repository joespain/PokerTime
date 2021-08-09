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
                var events = await _repository.GetAllEventsByHostIdAsync(getHostId());

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

        [HttpGet("{eventId:int}")]
        public async Task<ActionResult<EventModel>> GetEvent(int eventId)
        {
            try
            {
                if (eventId == 0) return BadRequest("Event not found.");

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
                newEvent.Invitees = null;

                _repository.Add(newEvent);


                if (await _repository.SaveChangesAsync())
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

        [HttpPut("{eventId:int}")]
        public async Task<ActionResult<EventModel>> UpdateEvent(int eventId, [FromBody] EventModel newEvent)
        {
            try
            {
                //Make sure the Id URI is the same as the model.Id. If they're different, something's wrong.
                if (eventId != newEvent.Id)
                {
                    return BadRequest("Error updating Event.");
                }

                //If the old event doesn't exist, it's either deleted or got wrong id.
                var oldEvent = await _repository.GetEventByIdAsync(newEvent.Id);
                if (oldEvent == null)
                {
                    return BadRequest("Event to update was not found.");
                }

                //Mapper applies the changes of the model onto the entity, updating the entity in the process.
                _mapper.Map(newEvent, oldEvent);

                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<EventModel>(newEvent);
                }
                else
                {
                    return BadRequest("Error updating the Event.");
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpDelete("{eventId:int}")]
        public async Task<ActionResult> DeleteEvent(int eventId)
        {
            try
            {
                if (eventId == 0) return BadRequest("No event to delete.");
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
    }
}
