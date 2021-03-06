using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
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
    [Route("api/TournamentStructures/{structureId:int}/BlindLevels")]
    [Authorize("api-access")]
    public class BlindLevelController : PokerTimeControllerBase
    {
        public BlindLevelController(IPTRepository repository, IMapper mapper, LinkGenerator linkGenerator) : base(repository, mapper, linkGenerator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlindLevelModel>>> GetBlindLevels(int structureId)
        {
            try
            {
                var results = await _repository.GetBlindLevelsByStructureIdAsync(structureId);

                return _mapper.Map<IEnumerable<BlindLevelModel>>(results).ToList();
            }
            catch
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{blindLevelId:int}")]
        public async Task<ActionResult<BlindLevelModel>> GetBlindLevel(int blindLevelId)
        {
            try
            {
                var results = await _repository.GetBlindLevelByIdAsync(blindLevelId);

                return _mapper.Map<BlindLevelModel>(results);
            }
            catch
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<BlindLevelModel>> AddBlindLevel(int structureId, [FromBody] BlindLevelModel model)
        {
            try
            {
                //Get the structure to attach to the Blind Level
                var structure = await _repository.GetTournamentStructureByIdAsync(structureId);

                //If the structure doesn't exist, something is wrong.
                if (structure == null) return BadRequest("Structure does not exist.");

                //If the structureId doesn't match the model's structureId, something's wrong.
                if (model.TournamentStructureId != structureId) return BadRequest("Error adding BlindLevel.");

                var newBlindLevel = _mapper.Map<BlindLevel>(model);

                //Add the TournamentStructureID to the new blind level.
                newBlindLevel.TournamentStructureId = structureId;
                _repository.Add(newBlindLevel);

                //Add the blind level to the tournamentstructure's list of blind levels.
                structure.BlindLevels.Add(newBlindLevel);

                if (await _repository.SaveChangesAsync())
                {
                    //var location = _linkGenerator.GetPathByAction(HttpContext,
                    //    "Get", "BlindLevels",
                    //values: new { structureId, newBlindLevel.Id, userId });
                    return Created($"api/tournamentstructures/{structureId}/blindlevels/{newBlindLevel.Id}",
                        _mapper.Map<BlindLevelModel>(newBlindLevel));
                }
                else
                {
                    return BadRequest("Failed to add new Blind Level.");
                }
            }
            catch (Exception e)
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<IEnumerable<BlindLevelModel>>> UpdateBlindLevel([FromBody] IEnumerable<BlindLevelModel> BlindLevels)
        {
            try
            {
                foreach(var blindLevel in BlindLevels)
                {
                    if(blindLevel.Id == 0)
                    {
                        _repository.Add(blindLevel);
                    }
                    else
                    {
                        //If the old structure doesn't exist, it's either deleted or got wrong id.
                        var oldBlindLevel = await _repository.GetBlindLevelByIdAsync(blindLevel.Id);
                        //if (oldBlindLevel == null)
                        //{
                        //    return BadRequest("BlindLevel does not exist.");
                        //}
                        //Mapper applies the changes of the model onto the entity, updating the entity in the process.
                        _mapper.Map(blindLevel, oldBlindLevel);
                        await _repository.SaveChangesAsync();
                    }
                }

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(BlindLevels);
                }
                else return BadRequest("Error updating the Blind Levels.");
            }
            catch (Exception e)
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpDelete("{blindLevelId:int}")]
        public async Task<ActionResult> DeleteBlindLevel(int blindLevelId)
        {
            try
            {
                if (blindLevelId == 0) return BadRequest("No Blind Level to delete.");
                if (await _repository.DeleteBlindLevelByIdAsync(blindLevelId))
                {
                    return NoContent();  //Success
                }
                else
                {
                    return BadRequest("Error deleting TournamentStructure.");
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
