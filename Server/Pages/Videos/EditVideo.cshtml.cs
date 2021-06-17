using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Server.Services;
using Server.Services.Identity;
using Server.ViewModels;

namespace Server.Pages.Videos
{
    [Authorize(AuthenticationSchemes=BasicAuthenticationDefaults.AuthenticationScheme)]
    public class EditVideo : PageModel
    {
        private readonly IVideoService _videoService;

        [BindProperty]
        public string SelectedCategory { get; set; }
        
        public ICollection<SelectListItem> CategorySelect { get; set; }
        
        [BindProperty]
        public VideoDto Model { get; set; }
        
        public EditVideo(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var categories = (await _videoService.GetVideosToAssign()).Adapt<ICollection<CategoryDto>>();
            
            CategorySelect = categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id
            }).ToList();

            Model = await _videoService.GetVideo(id);
            
            return Page();
        }
        
        public async Task<IActionResult> OnPostSaveChangesAsync()
        {
            Model.CategoryId = SelectedCategory;
            await _videoService.EditVideo(Model);
            return RedirectToPage("./Manage");
        }
        
        public async Task<IActionResult> OnPostDeleteVideoAsync()
        {
            await _videoService.DeleteVideo(Model.Id);
            return RedirectToPage("./Manage");
        }
    }
}