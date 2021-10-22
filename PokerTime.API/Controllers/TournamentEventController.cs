using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PokerTime.API.Data;
using PokerTime.Shared.Entities;
using PokerTime.Shared.Models;
using System;
using System.Threading.Tasks;

namespace PokerTime.API.Controllers
{
    //This controller is for the host to update tournamenttrackers, including ending a tournament.

    [ApiController]
    [Route("api/tournamentevent")]
    [Authorize("api-access")]
    public class TournamentEventController : PokerTimeControllerBase
    {

        public TournamentEventController(IPTRepository repository, IMapper mapper, LinkGenerator linkGenerator) : base(repository, mapper, linkGenerator)
        {
        }

        [HttpPost]
        public async Task<ActionResult<TournamentTrackingModel>> AddTournamentTracker([FromBody] TournamentTrackingModel model)
        {
            try
            {
                TournamentTracking newTournamentTracker = _mapper.Map<TournamentTracking>(model);

                if (await _repository.GetTournamentTrackingById(newTournamentTracker.Id) is null)
                {
                    if (await _repository.AddTournamentTracking(newTournamentTracker))
                    {
                        return Created($"api/tournamentevent/{newTournamentTracker.Id}",
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
            catch (Exception e)
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpPost("{tournamentId}/end")]
        public async Task<ActionResult> EndTournament(Guid tournamentId)
        {
            //Changes the TournamentTracker property IsTournamentRunning to false
            try
            {
                TournamentTracking tracker = await _repository.GetTournamentTrackingById(tournamentId);
                if (tracker is null)
                {
                    return BadRequest("Error ending the tournament.");
                }
                else
                {
                    tracker.IsTournamentRunning = false;
                    if(await _repository.UpdateTournamentTracking(tracker))
                    {
                        return Ok("Tournament ended.");
                    }
                    else
                    {
                        return BadRequest("Error ending the tournament.");
                    }
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
