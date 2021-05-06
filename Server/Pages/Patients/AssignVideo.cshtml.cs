using System;
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

            if (User == null)
            {
                return NotFound();
            }

            var videos = (await _videoService.GetVideos()).Adapt<ICollection<VideoDto>>();

            var categories = videos.GroupBy(v => v.CategoryId);

            foreach (var categoryGroup in categories)
            {
                var vidCollection = categoryGroup.ToList();

                if (vidCollection.Count == 0)
                {
                    break;
                }

                var cat = new CategoryDto
                {
                    Id = categoryGroup.Key,
                    Name = vidCollection.First().Name,
                    Videos = vidCollection
                };

                Categories.Add(cat);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAssignVideoAsync()
        {
            try
            {
                string videoId = Request.Form["videoId"];
                await _videoService.AssignVideo(Model.Id, videoId);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Error: {Error}", e.Message);
                throw;
            }

            return RedirectToPage("Details", new {id = Model.Id});
        }
    }
}