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
    [Authorize(AuthenticationSchemes=BasicAuthenticationDefaults.AuthenticationScheme)]
    public class RemovePatientModel : PageModel
    {
        public PatientViewModel Model { get; set; }
        
        private readonly IPatientService _patientService;

        public RemovePatientModel(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            Model = (await _patientService.GetById(id)).Adapt<PatientViewModel>();

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

            await _patientService.Remove(id);
            
            return RedirectToPage("./List");
        }
    }
}