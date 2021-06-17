using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared.Api.Requests;
using Shared.Api.Responses;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/patient")]
    public class PatientController: ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody]PatientLoginRequest loginRequest)
        {
            var patient = await _patientService.GetByToken(loginRequest.Token);

            if (patient == null)
            {
                return BadRequest(new { message = "Invalid token" });
            }

            return Ok(patient.Adapt<PatientLoginResponse>());
        }
        
        [HttpPost("@me")]
        public async Task<IActionResult> Get()
        {
            var identifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (identifier == null)
            {
                return BadRequest(new { message = "Invalid token" });
            }
            
            var patient = await _patientService.GetById(identifier);

            if (patient == null)
            {
                return BadRequest(new { message = "Invalid token" });
            }

            return Ok(patient.Adapt<PatientDetailsResponse>());
        }
    }
}