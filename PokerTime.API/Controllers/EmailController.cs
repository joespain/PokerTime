
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokerTime.Shared.Email;
using System;
using System.Threading.Tasks;

namespace PokerTime.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : Controller
    {

        private readonly IMailService MailService;
        private ILogger Logger;
        public EmailController(IMailService mailService, ILogger logger)
        {
            this.MailService = mailService;
            this.Logger = logger;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> Send([FromBody] MailRequest request)
        {
            try
            {
                await MailService.SendEmailAsync(request);
                return Ok();
            }
            catch(Exception e)
            {
                Logger.LogDebug(e.Message);
                return BadRequest("Email not sent.");
            }

        }


    }
}
