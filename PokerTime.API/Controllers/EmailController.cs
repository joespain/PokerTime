
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
        private readonly ILogger<EmailController> _logger;

        public EmailController(IMailService mailService, ILogger<EmailController> logger)
        {
            _mailService = mailService;
            _logger = logger;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> Send([FromBody] MailRequest request)
        {
            try
            {
                var userId = User.FindFirst(a => a.Type == "sub")?.Value;
                _logger.LogInformation(message: "{UserName} - {UserId} is sending email to {EmailAddress}.",
                    User.Identity.Name, userId, request.ToEmail);

                await _mailService.SendEmailAsync(request);
                return Ok();
            }
            catch(Exception e)
            {
                _logger.LogError($"Error {e.Message}");
                return BadRequest("Email not sent.");
            }

        }


    }
}
