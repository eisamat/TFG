using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    public class AdminDto
    {
        [Required]
        public string Fullname { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
    
    [AllowAnonymous]
    [ApiController]
    [Route("utils")]
    public class UtilsController: ControllerBase
    {
        private readonly ITherapistService _therapistService;

        public UtilsController(ITherapistService therapistService)
        {
            _therapistService = therapistService;
        }

        [HttpPost("admin")]
        public async Task<IActionResult> CreateAdmin([FromBody]AdminDto adminDto)
        {
            await _therapistService.AddTherapist(adminDto.Fullname, adminDto.Username, adminDto.Password);
            return Ok(adminDto);
        }
    }
}