using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Server.Identity;
using Server.Models;
using Server.Services;

namespace Server.Pages.Patients
{
    [Authorize(AuthenticationSchemes=BasicAuthenticationDefaults.AuthenticationScheme)]
    public class PatientDetailsModel : PageModel
    {
        [BindProperty]
        public PatientDto Model { get; set; }
        
        private readonly ILogger<PatientDetailsModel> _logger;
        private readonly IPatientService _patientService;

        public PatientDetailsModel(ILogger<PatientDetailsModel> logger, IPatientService patientService)
        {
            _logger = logger;
            _patientService = patientService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            Model = await _patientService.GetPatient(id);

            if (User == null)
            {
                return NotFound();
            }
            
            return Page();
        }
        
        public async Task<IActionResult> OnPostSaveChangesAsync()
        {
            try
            {
                await _patientService.EditPatient(Model);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Error: {Error}", e.Message);
                throw;
            }
            
            return RedirectToPage("Details");
        }
        
        public async Task<IActionResult> OnPostRefreshTokenAsync()
        {
            await _patientService.RefreshPatientToken(Model.Id);
            
            return RedirectToPage("Details");
        }
    }
}