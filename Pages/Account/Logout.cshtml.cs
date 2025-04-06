using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TicketWave.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            await HttpContext.SignOutAsync(); // Log out the user
            return RedirectToPage("/Index"); // Redirect to home page after logout
        }

        public IActionResult OnGet()
        {
            return RedirectToPage("/Index");
        }
    }
}
