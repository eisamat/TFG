using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Services;

namespace Server.Controllers
{
    public class LoginDto
    {
        public string Token { get; set; }
    }
    
    [Authorize]
    [ApiController]
    [Route("auth")]
    public class AuthenticationController: ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IPatientService _patientService;

        public AuthenticationController(IPatientService patientService, ILogger<AuthenticationController> logger)
        {
            _patientService = patientService;
            _logger = logger;
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
        {
            var user = await _patientService.GetPatientByToken(loginDto.Token);

            if (user == null)
            {
                return BadRequest(new { message = "Invalid token" });
            }

            return Ok(user);
        }
        
        [HttpPost("@me")]
        public async Task<IActionResult> GetUserInformation()
        {
            var identifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (identifier == null)
            {
                return BadRequest(new { message = "Invalid token" });
            }
            
            var user = await _patientService.GetPatient(identifier);

            if (user == null)
            {
                return BadRequest(new { message = "Invalid token" });
            }

            return Ok(user);
        }
    }
}