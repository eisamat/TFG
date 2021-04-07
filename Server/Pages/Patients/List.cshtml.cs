using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Server.Models;
using Server.Services;
using Server.Services.Identity;
using Server.ViewModels;

namespace Server.Pages.Patients
{
    [Authorize(AuthenticationSchemes=BasicAuthenticationDefaults.AuthenticationScheme)]
    public class PatientListModel : PageModel
    {
        private readonly ILogger<PatientListModel> _logger;
        private readonly IPatientService _patientService;
        private readonly ITherapistService _therapistService;
        public IList<PatientViewModel> Patients { get; set; }

        public PatientListModel(IPatientService patientService, ILogger<PatientListModel> logger, ITherapistService therapistService)
        {
            _patientService = patientService;
            _logger = logger;
            _therapistService = therapistService;
        }

        public async Task OnGetAsync()
        {
            var identifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var therapist = await _therapistService.Get(identifier);
            Patients = (await _patientService.List(therapist)).Adapt<List<PatientViewModel>>();
        }
    }
}