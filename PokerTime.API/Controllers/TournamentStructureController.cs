using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
    [Route("api/tournamentstructures")]
    [ApiController]
    [Authorize("api-access")]
    public class TournamentStructureController : PokerTimeControllerBase
    {
        public TournamentStructureController(IPTRepository repository, IMapper mapper, LinkGenerator linkGenerator) : base(repository, mapper, linkGenerator)
        {

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentStructureModel>>> GetTournamentStructures()
        {
            try
            {
                var results = await _repository.GetTournamentStructuresByHostIdAsync(GetHostId());

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
        public async Task<ActionResult<TournamentStructureModel>> AddTournamentStructure([FromBody] TournamentStructureModel model)
        {
            try
            {
                //Get the host to attach to the Tournament Structure
                var host = await _repository.GetHostByIdAsync(GetHostId());

                if (host == null) return BadRequest("Host does not exist.");

                var newTournamentStructure = _mapper.Map<TournamentStructure>(model);

                newTournamentStructure.DateCreated = DateTime.Today;
                newTournamentStructure.HostId = host.Id;
                
                _repository.Add(newTournamentStructure);
                if (await _repository.SaveChangesAsync())
                {
                    //find out why the link generator isn't working.
                    //var location = _linkGenerator.GetPathByAction(HttpContext, 
                    //    "Get", "Users",
                    //values: new { userId, newTournamentStructure.Id });
                    return Created($"api/tournamentstructures/{newTournamentStructure.Id}",
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
                    return NotFound();
                }

                //Update or add new blind levels
                if (newStructure.BlindLevels != null)
                {
                    foreach(var blindLevel in newStructure.BlindLevels)
                    {
                        if(blindLevel.Id == 0)
                        {
                            var newBlindLevel = _mapper.Map<BlindLevel>(blindLevel);
                            _repository.Add(newBlindLevel);
                        }
                        else
                        {
                            var oldBlindLevel = await _repository.GetBlindLevelByIdAsync(blindLevel.Id);
                            _mapper.Map(blindLevel, oldBlindLevel);
                        }
                    }
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
