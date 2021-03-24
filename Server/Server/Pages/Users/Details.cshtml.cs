using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Server.Services;

namespace Server.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly ILogger<DetailsModel> _logger;
        private readonly IUserService _userService;

        public DetailsModel(ILogger<DetailsModel> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public UserDto UserDetails { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            UserDetails = await _userService.GetUser((int) id);

            if (User == null)
            {
                return NotFound();
            }
            
            return Page();
        }
    }
}