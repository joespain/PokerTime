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
    [AllowAnonymous]
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
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error getting the tracker");
            }
        }

        [HttpGet("{tournamentId}/{structureId}")]
        public async Task<ActionResult<TournamentStructureModel>> GetTournamentStructure(int structureId)
        {
            try
            {
                var structure = await _repository.GetTournamentStructureByIdAsync(structureId);

                return _mapper.Map<TournamentStructureModel>(structure);
            }
            catch
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error getting the structure");
            }
        }
    }
}
