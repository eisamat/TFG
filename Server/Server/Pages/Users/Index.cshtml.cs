using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Server.Services;

namespace Server.Pages.Users
{
    public class UsersModel : PageModel
    {
        private readonly ILogger<UsersModel> _logger;
        private readonly IUserService _userService;
        public IList<UserDto> Users { get; set; }

        public UsersModel(IUserService userService, ILogger<UsersModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            Users = await _userService.GetAllUsers();
        }
    }
}