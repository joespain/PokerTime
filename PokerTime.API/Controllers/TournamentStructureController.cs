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
        public async Task<ActionResult<IEnumerable<TournamentStructureModel>>> Get(int UserId)
        {
            try
            {
                var results = await _repository.GetTournamentStructuresByUserIdAsync(UserId);

                return _mapper.Map<IEnumerable<TournamentStructureModel>>(results).ToList();
            }
            catch
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        //[HttpGet("{StructureId:int}")]
        //public async Task<ActionResult<TournamentStructureModel>> Get(int StructureId)
        //{
        //    try
        //    {
        //        var Results = await _repository.GetTournamentStructureAsync(StructureId);

        //        return _mapper.Map<TournamentStructureModel>(Results);
        //    }
        //    catch
        //    {
        //        //Update with real status code errors
        //        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
        //    }
        //}

        [HttpPost]
        public async Task<ActionResult<TournamentStructureModel>> Post(int Id, TournamentStructureModel Model)
        {
            try
            {   
                Model.HostId = Id;
                Model.Host = _mapper.Map<UserModel>(_repository.GetUserByIdAsync(Id));

                var location = _linkGenerator.GetPathByAction("Get",
                    "TournamentStructures", Model.Id);


                var newTournamentStructure = _mapper.Map<TournamentStructure>(Model);
                _repository.Add(newTournamentStructure);
                if (await _repository.SaveChangesAsync())
                {
                    return Created(location, _mapper.Map<TournamentStructureModel>(newTournamentStructure));
                }
            }
            catch
            {
                //Update with real status code errors
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }






    }
}
