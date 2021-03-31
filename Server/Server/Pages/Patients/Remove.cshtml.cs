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
    public class RemovePatientModel : PageModel
    {
        public PatientDto Model { get; set; }
        
        private readonly ILogger<RemovePatientModel> _logger;
        private readonly IPatientService _patientService;

        public RemovePatientModel(ILogger<RemovePatientModel> logger, IPatientService patientService)
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _patientService.RemovePatient(id);
            
            return RedirectToPage("./List");
        }
    }
}