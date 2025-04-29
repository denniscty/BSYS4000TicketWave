using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TicketWave.Data;
using TicketWave.Models;
using TicketWave.Services.Admin;

namespace TicketWave.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class UsersModel : PageModel
    {
        private readonly TicketWaveContext _context;
        private readonly DeleteUserService _deleteUserService;

        public UsersModel(TicketWaveContext context, DeleteUserService deleteUserService)
        {
            _context = context;
            _deleteUserService = deleteUserService!;
        }

        public List<User> Users { get; set; } = new();

        public async Task OnGetAsync()
        {
            Users = await _context.Users
                .Where(u => u.Role != "Admin")
                .OrderBy(u => u.UserName)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string userId)
        {
            bool result = await _deleteUserService.DeleteUserAsync(userId);

            if (!result)
                return NotFound();

            return RedirectToPage(); // refresh current page
        }

    }
}
