using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Server.Models;
using Server.Services;
using Server.Services.Identity;

namespace Server.Pages.Patients 
{
    [Authorize(AuthenticationSchemes=BasicAuthenticationDefaults.AuthenticationScheme)]
    public class AddPatientModel : PageModel
    {
        private readonly ILogger<AddPatientModel> _logger;
        private readonly IPatientService _patientService;
        private readonly ITherapistService _therapistService;

        public AddPatientModel(ILogger<AddPatientModel> logger, IPatientService patientService, ITherapistService therapistService)
        {
            _logger = logger;
            _patientService = patientService;
            _therapistService = therapistService;
        }

        [BindProperty]
        public AddPatientViewModel Model { get; init; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            try
            {
                var identifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var therapist = await _therapistService.Get(identifier);
                await _patientService.Add(therapist, Model);
                return RedirectToPage("./List");
            }
            catch (Exception e)
            {
                _logger.LogCritical("Error: {Error}", e.Message);
                throw;
            }
        }
    }
}