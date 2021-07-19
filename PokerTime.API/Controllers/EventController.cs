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
    [Route("api/users/{userId}/events")]
    public class EventController : ControllerBase
    {
        private readonly IPTRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        public EventController(IPTRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventModel>>> GetEvents(int userId)
        {
            try
            {
                if (userId == 0) return BadRequest("User does not exist.");

                var events = await _repository.GetAllEventsByUserIdAsync(userId);

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
        public async Task<ActionResult<EventModel>> AddEvent(int userId, [FromBody] EventModel model)
        {
            try
            {
                //var location = _linkGenerator.GetPathByAction(HttpContext, "Get",
                //    "Events",
                //    values: new { userId, model.Id });

                var newEvent = _mapper.Map<Event>(model);
                _repository.Add(newEvent);
                if (await _repository.SaveChangesAsync())
                {
                    return Created($"api/users/{userId}/events/{newEvent.Id}", 
                        _mapper.Map<EventModel>(newEvent));
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

        [HttpDelete]
        public async Task<ActionResult> DeleteEvent(int eventId)
        {
            try
            {
                if (eventId == 0) return BadRequest("No invitee to delete.");
                if (await _repository.DeleteEventByIdAsync(eventId))
                {
                    return NoContent();  //Success
                }
                else
                {
                    return BadRequest("Error deleting invitee.");
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
