
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokerTime.Shared.Email;
using System;
using System.Threading.Tasks;

namespace PokerTime.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("api-access")]
    public class EmailController : Controller
    {

        private readonly IMailService _mailService;

        public EmailController(IMailService mailService)
        {
            _mailService = mailService;

        }

        [HttpPost("Send")]
        public async Task<IActionResult> Send([FromBody] MailRequest request)
        {
            try
            {
                await _mailService.SendEmailAsync(request);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest($"Email not sent. Error: {e.Message}");
            }

        }


    }
}
