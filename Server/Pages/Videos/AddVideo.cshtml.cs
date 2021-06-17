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
    public class AddVideo : PageModel
    {
        [BindProperty]
        public VideoDto Model { get; set; }
        
        [BindProperty]
        public string SelectedCategory { get; set; }
        
        public ICollection<SelectListItem> CategorySelect { get; set; }
        

        private readonly IVideoService _videoService;

        public AddVideo(IVideoService videoService)
        {
            _videoService = videoService;
            CategorySelect = new List<SelectListItem>();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var categories = (await _videoService.GetVideosToAssign()).Adapt<ICollection<CategoryDto>>();
            
            CategorySelect = categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id
            }).ToList();
            
            return Page();
        }
        
        public async Task<IActionResult> OnPostSaveChangesAsync()
        {
            await _videoService.AddNewVideo(new AddVideoViewModel
            {
                Name = Model.Name,
                YoutubeId = Model.YoutubeId,
                CategoryId = SelectedCategory
            });
            
            return RedirectToPage("./Manage");
        }
    }
}