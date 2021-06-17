using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Services;
using Server.Services.Identity;
using Server.ViewModels;

namespace Server.Pages.Videos
{
    [Authorize(AuthenticationSchemes=BasicAuthenticationDefaults.AuthenticationScheme)]
    public class AddCategory : PageModel
    {
        private readonly IVideoService _videoService;

        public AddCategory(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [BindProperty]
        public CreateCategoryModel Model { get; set; }
        
        public async Task<IActionResult>  OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostSaveChangesAsync()
        {
            await _videoService.AddNewCategory(Model.Name.ToUpper());
            
            return RedirectToPage("./Manage");
        }
    }
}