using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Server.Identity;
using Server.Models;
using Server.Services;

namespace Server.Pages.Patients
{
    [Authorize(AuthenticationSchemes=BasicAuthenticationDefaults.AuthenticationScheme)]
    public class PatientListModel : PageModel
    {
        private readonly ILogger<PatientListModel> _logger;
        private readonly IPatientService _patientService;
        private readonly ITherapistService _therapistService;
        public IList<PatientDto> Patients { get; set; }

        public PatientListModel(IPatientService patientService, ILogger<PatientListModel> logger, ITherapistService therapistService)
        {
            _patientService = patientService;
            _logger = logger;
            _therapistService = therapistService;
        }

        public async Task OnGetAsync()
        {
            var identifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var therapist = await _therapistService.GetTherapist(identifier);
            Patients = await _patientService.GetAllPatients(therapist);
        }
    }
}