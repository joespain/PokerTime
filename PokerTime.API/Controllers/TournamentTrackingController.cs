using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using PokerTime.API.Data;
using PokerTime.Shared.Entities;
using PokerTime.Shared.Models;
using System;
using System.Threading.Tasks;

namespace PokerTime.API.Controllers
{
    [Route("api/tournamenttracking")]
    [ApiController]
    //[Authorize("api-access")]
    public class TournamentTrackingController : PokerTimeControllerBase
    {
        public TournamentTrackingController(IPTRepository repository, IMapper mapper, LinkGenerator linkGenerator) : base(repository, mapper, linkGenerator)
        {

        }

        [HttpGet("{tournamentId}")]
        public async Task<ActionResult<TournamentTrackingModel>> GetTournamentTracker(Guid tournamentId)
        {
            try
            {
                var tracker = await _repository.GetTournamentTrackingById(tournamentId);

                return _mapper.Map<TournamentTrackingModel>(tracker);
            }
            catch(Exception e)
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TournamentTrackingModel>> AddTournamentTracker([FromBody] TournamentTrackingModel model)
        {
            try
            {
                var newTournamentTracker = _mapper.Map<TournamentTracking>(model);

                if(await _repository.GetTournamentTrackingById(newTournamentTracker.Id) == null)
                {
                    if (await _repository.AddTournamentTracking(newTournamentTracker))
                    {
                        return Created($"api/tournamenttracking/{newTournamentTracker.Id}",
                            _mapper.Map<TournamentTrackingModel>(newTournamentTracker));
                    }
                    else
                    {
                        return BadRequest("Failed to add new TournamentTracking.");
                    }
                }
                else
                {
                    if (await _repository.UpdateTournamentTracking(newTournamentTracker))
                    {
                        return _mapper.Map<TournamentTrackingModel>(newTournamentTracker);
                    }
                    else return BadRequest("Error updating the Tournament Tracking.");
                }

                
            }
            catch (Exception e)
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        } 


        [HttpPut("{tournamentId}")]
        public async Task<ActionResult<TournamentTrackingModel>> UpdateTournamentTracker(Guid tournamentId, [FromBody] TournamentTrackingModel model)
        {
            try
            {

                if (!await _repository.DoesTournamentTrackingExist(model.Id)) //If the tracking does not already exist in the database
                {
                    if (await _repository.AddTournamentTracking(_mapper.Map<TournamentTracking>(model)))
                    {
                        return Ok(model);
                    }
                    else return BadRequest("Error updating the Tournament Tracking.");
                }
                else
                {

                    if (await _repository.UpdateTournamentTracking(_mapper.Map<TournamentTracking>(model)))
                    {
                        return Ok(model);
                    }
                    else return BadRequest("Error updating the Tournament Tracking.");
                }


            }
            catch(Exception e)
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
    }
}
