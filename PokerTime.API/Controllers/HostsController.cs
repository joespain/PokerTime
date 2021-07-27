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
    [Route("api/users")]
    public class HostsController : ControllerBase
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HostModel>>> GetHosts()
        {
            try
            {
                var users = await _repository.GetAllHosts();

                return _mapper.Map<IEnumerable<HostModel>>(users).ToList();
            }
            catch(Exception e)
            {
                //replace with real error code
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpGet("{hostId:Guid}")]
        public async Task<ActionResult<HostModel>> GetHost(Guid hostId)
        {
            try
            {
                var result = await _repository.GetHostByIdAsync(hostId,true,true,true);

                if(result == null)
                {
                    return BadRequest("User does not exist.");
                }

                return _mapper.Map<HostModel>(result);
            }
            catch(Exception e)
            {
                //replace with real error code
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<HostModel>> AddHost(HostModel model)
        {
            try
            {
                var newHost = _mapper.Map<Host>(model);

                _repository.Add(newHost);
                
                if (await _repository.SaveChangesAsync())
                {
                    //Figure out why this linkgenerator isn't working.
                    //var location = _linkGenerator.GetPathByAction(action: "Get", controller: "Users", values: new { userId = newUser.Id });

                    //if (location == null) return BadRequest("Error saving user, location");

                    return Created($"/users/{newHost.Id}", _mapper.Map<HostModel>(newHost));
                }
            }
            catch (Exception e)
            {
                //replace with real error code
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
            return BadRequest();
        }

        [HttpPut("{hostId:Guid}")]
        public async Task<ActionResult<HostModel>> UpdateHost(Guid hostId, [FromBody] HostModel newHost)
        {
            try
            {
                var oldHost = await _repository.GetHostByIdAsync(hostId);
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

        [HttpDelete("{hostId:Guid}")]
        public async Task<ActionResult> DeleteHost(Guid hostId)
        {
            try
            {

                if(await _repository.DeleteHostByIdAsync(hostId))
                {
                    return NoContent();  //Success
                }
                else
                {
                    return BadRequest("Error deleting User.");
                }
                
            }
            catch(Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

    }
}
