using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Services;
using Server.Services.Identity;
using Server.ViewModels;

namespace Server.Pages.Videos
{
    [Authorize(AuthenticationSchemes=BasicAuthenticationDefaults.AuthenticationScheme)]
    public class VideoManageModel : PageModel
    {
        private readonly IVideoService _videoService;

        public ICollection<CategoryDto> Categories { get; set; }
        
        public VideoManageModel(IVideoService videoService)
        {
            _videoService = videoService;

            Categories = new List<CategoryDto>();
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Categories = (await _videoService.GetVideosToAssign()).Adapt<ICollection<CategoryDto>>();

            return Page();
        }
    }
}