using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Server.Services.Identity;
using Server.ViewModels;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/patient/videos")]
    public class VideosController: ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IVideoService _videoService;

        public VideosController(IPatientService patientService, IVideoService videoService)
        {
            _patientService = patientService;
            _videoService = videoService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAssignedVideos()
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

            var videos = (await _videoService.GetAssignedVideos(patient)).Adapt<ICollection<VideoDto>>();
            return Ok(videos);
        }
        
        [HttpPost("Assign")]
        [Authorize(AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AssignVideos([FromBody] AssignVideoViewModel viewModel)
        {
            await _videoService.AssignVideo(viewModel);

            return Ok();
        }
    }
}