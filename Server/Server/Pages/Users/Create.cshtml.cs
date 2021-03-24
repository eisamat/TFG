using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Server.Services;

namespace Server.Pages.Users
{
    public class CreateUserModel : PageModel
    {
        private readonly ILogger<CreateUserModel> _logger;
        private readonly IUserService _userService;

        public CreateUserModel(ILogger<CreateUserModel> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [BindProperty]
        public CreateUserDto CreateUserDto { get; init; }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _userService.CreateNewUser(CreateUserDto);

            return RedirectToPage("./Index");
        }
    }
}