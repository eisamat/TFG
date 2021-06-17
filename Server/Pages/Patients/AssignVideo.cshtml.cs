using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Server.Services;
using Server.Services.Identity;
using Server.ViewModels;

namespace Server.Pages.Patients
{
    [Authorize(AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme)]
    public class AssignVideo : PageModel
    {
        private readonly IPatientService _patientService;
        private readonly IVideoService _videoService;

        private readonly ILogger<AssignVideo> _logger;

        [BindProperty] public PatientViewModel Model { get; set; }
        public ICollection<CategoryDto> Categories { get; set; }

        public AssignVideo(IPatientService patientService, IVideoService videoService,
            ILogger<AssignVideo> logger)
        {
            _patientService = patientService;
            _videoService = videoService;
            _logger = logger;

            Categories = new List<CategoryDto>();
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Model = (await _patientService.GetById(id)).Adapt<PatientViewModel>();

            if (Model == null)
            {
                return NotFound();
            }

            Categories = (await _videoService.GetVideosToAssign()).Adapt<ICollection<CategoryDto>>();
            
            return Page();
        }
    }
}