using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    [Route("api/hosts")]
    [Authorize("api-access")]
    public class HostsController : PokerTimeControllerBase
    {
        private readonly IPTRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        public HostsController(IPTRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<HostModel>>> GetHosts()
        //{
        //    try
        //    {
        //        var users = await _repository.GetAllHosts();

        //        return _mapper.Map<IEnumerable<HostModel>>(users).ToList();
        //    }
        //    catch(Exception e)
        //    {
        //        //replace with real error code
        //        return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
        //    }
        //}

        [HttpGet]
        public async Task<ActionResult<HostModel>> GetHost()
        {
            try
            {
                var result = await _repository.GetHostByIdAsync(getHostId());

                if(result == null)
                {
                    return BadRequest("Host does not exist.");
                }

                return _mapper.Map<HostModel>(result);
            }
            catch(Exception e)
            {
                //replace with real error code
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        //[HttpPost]
        //public async Task<ActionResult<HostModel>> AddHost(HostModel model)
        //{
        //    try
        //    {
        //        var newHost = _mapper.Map<Host>(model);

        //        _repository.Add(newHost);
                
        //        if (await _repository.SaveChangesAsync())
        //        {
        //            //Figure out why this linkgenerator isn't working.
        //            //var location = _linkGenerator.GetPathByAction(action: "Get", controller: "Users", values: new { userId = newUser.Id });

        //            //if (location == null) return BadRequest("Error saving user, location");

        //            //Returning an empty string for the host location.
        //            return Created("", _mapper.Map<HostModel>(newHost));
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        //replace with real error code
        //        return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
        //    }
        //    return BadRequest();
        //}

        [HttpPut]
        public async Task<ActionResult<HostModel>> UpdateHost([FromBody] HostModel newHost)
        {
            try
            {
                var oldHost = await _repository.GetHostByIdAsync(newHost.Id);
                if (oldHost == null)
                {
                    return BadRequest("Host to update was not found.");
                }

                //Mapper applies the changes of the model onto the user, updating the entity in the process.
                _mapper.Map(newHost, oldHost);

                if(await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<HostModel>(newHost);
                }
                else
                {
                    return BadRequest("Error updating the host.");
                }
            }
            catch(Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpDelete()]
        public async Task<ActionResult> DeleteHost()
        {
            try
            {

                if(await _repository.DeleteHostByIdAsync(getHostId()))
                {
                    return NoContent();  //Success
                }
                else
                {
                    return BadRequest("Error deleting host.");
                }
                
            }
            catch(Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

    }
}
