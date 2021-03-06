using System;
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
    public class PatientDetailsModel : PageModel
    {
        [BindProperty]
        public PatientViewModel Model { get; set; }
        
        private readonly ILogger<PatientDetailsModel> _logger;
        private readonly IPatientService _patientService;
        private readonly IVideoService _videoService;

        public PatientDetailsModel(ILogger<PatientDetailsModel> logger, IPatientService patientService, IVideoService videoService)
        {
            _logger = logger;
            _patientService = patientService;
            _videoService = videoService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            Model = (await _patientService.GetById(id)).Adapt<PatientViewModel>();
            Model.PreviouslyAssignedVideos = (await _videoService.GetPreviouslyAssignedVideos(id));
            
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
                await _patientService.Edit(Model);
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
            await _patientService.RefreshToken(Model.Id);
            
            return RedirectToPage("Details");
        }
        
        public async Task<IActionResult> OnPostUnassignVideoAsync()
        {
            try
            {
                string videoId = Request.Form["videoId"];
                await _videoService.UnassignVideo(Model.Id, videoId);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Error: {Error}", e.Message);
                throw;
            }
            
            return RedirectToPage("Details");
        }
    }
}