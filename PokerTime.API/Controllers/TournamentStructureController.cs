using AutoMapper;
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
    [Route("api/Users/{hostId:Guid}/TournamentStructures")]
    public class TournamentStructureController : ControllerBase
    {
        private readonly IPTRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        public TournamentStructureController(IPTRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentStructureModel>>> GetTournamentStructures(Guid hostId)
        {
            try
            {
                var results = await _repository.GetTournamentStructuresByUserIdAsync(hostId);

                return _mapper.Map<IEnumerable<TournamentStructureModel>>(results).ToList();
            }
            catch
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{StructureId:int}")]
        public async Task<ActionResult<TournamentStructureModel>> GetTournamentStructure(int structureId)
        {
            try
            {
                var results = await _repository.GetTournamentStructureByIdAsync(structureId);

                return _mapper.Map<TournamentStructureModel>(results);
            }
            catch
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TournamentStructureModel>> AddTournamentStructure(Guid hostId, [FromBody] TournamentStructureModel model)
        {
            try
            {   
                //Get the user to attach to the Tournament Structure
                var user = await _repository.GetUserByIdAsync(hostId);

                if(user == null) return BadRequest("User does not exist.");

                var newTournamentStructure = _mapper.Map<TournamentStructure>(model);

                newTournamentStructure.DateCreated = DateTime.Today;
                newTournamentStructure.HostId = hostId;
                
                _repository.Add(newTournamentStructure);
                if (await _repository.SaveChangesAsync())
                {
                    //find out why the link generator isn't working.
                    //var location = _linkGenerator.GetPathByAction(HttpContext, 
                    //    "Get", "Users",
                    //values: new { userId, newTournamentStructure.Id });
                    return Created($"users/{hostId}/tournamentstructures/{newTournamentStructure.Id}",
                        _mapper.Map<TournamentStructureModel>(newTournamentStructure));
                }
                else
                {
                    return BadRequest("Failed to add new Tournament Structure.");
                }
            }
            catch(Exception e)
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpPut("{structureId:int}")]
        public async Task<ActionResult<TournamentStructureModel>> UpdateTournamentStructure(int structureId, TournamentStructureModel newStructure)
        {
            try
            {
                //Make sure the Id URI is the same as the model.Id. If they're different, something's fucked.
                if (structureId != newStructure.Id)
                {
                    return BadRequest("Error updating TournamentStructure.");
                }

                //If the old structure doesn't exist, it's either deleted or got wrong id.
                var oldStructure = await _repository.GetTournamentStructureByIdAsync(newStructure.Id);
                if (oldStructure == null)
                {
                    return BadRequest("Tournament Structure does not exist.");
                }

                //Mapper applies the changes of the model onto the entity, updating the entity in the process.
                _mapper.Map(newStructure, oldStructure);

                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<TournamentStructureModel>(newStructure);
                }
                else return BadRequest("Error updating the Tournament Structure.");
            }
            catch(Exception e)
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpDelete("{structureId:int}")]
        public async Task<ActionResult> DeleteTournamentStructure(int structureId)
        {
            try
            {
                if (structureId == 0) return BadRequest("No TournamentStructure to delete.");
                if(await _repository.DeleteTournamentStructureByIdAsync(structureId))
                {
                    return NoContent();  //Success
                }
                else
                {
                    return BadRequest("Error deleting TournamentStructure.");
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
