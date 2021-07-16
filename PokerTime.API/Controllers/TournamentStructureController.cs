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
    [Route("api/Users/{UserId}/TournamentStructures")]
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
        public async Task<ActionResult<IEnumerable<TournamentStructureModel>>> GetTournamentStructures(int userId)
        {
            try
            {
                var results = await _repository.GetTournamentStructuresByUserIdAsync(userId);

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
        public async Task<ActionResult<TournamentStructureModel>> AddTournamentStructure(int userId, TournamentStructureModel model)
        {
            try
            {   
                //Get the user to attach to the Tournament Structure
                var user = await _repository.GetUserByIdAsync(userId);

                if(user == null) return BadRequest("User does not exist.");

                var newTournamentStructure = _mapper.Map<TournamentStructure>(model);

                newTournamentStructure.DateCreated = DateTime.Today;
                newTournamentStructure.Host = user;
                newTournamentStructure.HostId = userId;
                
                _repository.Add(newTournamentStructure);
                if (await _repository.SaveChangesAsync())
                {
                    var location = _linkGenerator.GetPathByAction(HttpContext, 
                        "Get", "Users",
                    values: new { userId, newTournamentStructure.Id });
                    return Created(location, _mapper.Map<TournamentStructureModel>(newTournamentStructure));
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
        public async Task<ActionResult<TournamentStructureModel>> UpdateTournamentStructure(TournamentStructureModel structure)
        {
            try
            {
                var existingStructure = await _repository.GetTournamentStructureByIdAsync(structure.Id);
                if (existingStructure == null) return BadRequest("Tournament Structure does not exist. Create a new structure.");

                _mapper.Map(structure, existingStructure);
                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<TournamentStructureModel>(structure);
                }
                else return BadRequest("There was an error updating the Tournament Structure.");
            }
            catch(Exception e)
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }


        [HttpDelete("{structureId:int}")]
        public async Task<ActionResult> DeleteTournamentStructure(int id)
        {
            try
            {
                if (id == 0) return BadRequest("No tournament structure to delete.");
                if(await _repository.DeleteTournamentStructure(id))
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
