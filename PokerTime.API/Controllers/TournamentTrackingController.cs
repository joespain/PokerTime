using AutoMapper;
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
    [Route("api/tournamenttracking")]
    [ApiController]
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

                _repository.Add(newTournamentTracker);

                if (await _repository.SaveChangesAsync())
                {
                    return Created($"api/tournamenttracking/{newTournamentTracker.Id}",
                        _mapper.Map<TournamentStructureModel>(newTournamentTracker));
                }
                else
                {
                    return BadRequest("Failed to add new TournamentTracking.");
                }
            }
            catch (Exception e)
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        } 


        [HttpPut("{tournamentId}")]
        public async Task<ActionResult<TournamentTrackingModel>> UpdateTournamentTracker([FromBody] TournamentTrackingModel model)
        {
            try
            {
                var trackerToUpdate = await _repository.GetTournamentTrackingById(model.Id);
                _mapper.Map(model, trackerToUpdate);

                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<TournamentTrackingModel>(trackerToUpdate);
                }
                else return BadRequest("Error updating the Tournament Tracking.");

            }
            catch(Exception e)
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
    }
}
