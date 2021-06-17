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
    public class EditCategory : PageModel
    {
        private readonly IVideoService _videoService;

        [BindProperty]
        public EditCategoryModel Model { get; set; }
        
        public EditCategory(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public async Task<IActionResult>  OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _videoService.GetCategory(id);
           
            Model = new EditCategoryModel
            {
                Id = category.Id,
                Name = category.Name
            };
            
            return Page();
        }
        
        public async Task<IActionResult> OnPostSaveChangesAsync()
        {
            await _videoService.EditCategory(Model);
            return RedirectToPage("./Manage");
        }
        
        public async Task<IActionResult> OnPostDeleteCategoryAsync()
        {
            await _videoService.DeleteCategory(Model.Id);
            return RedirectToPage("./Manage");
        }
    }
}