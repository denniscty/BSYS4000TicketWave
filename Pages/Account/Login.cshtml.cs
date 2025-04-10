using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TicketWave.Data;
using TicketWave.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace TicketWave.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly TicketWaveContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginModel(TicketWaveContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; } = new ();

        public class LoginInputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //Console.WriteLine($"LOGIN ATTEMPT: " + Input.UserName);

            if (!ModelState.IsValid)
            {
                //return Page();

                //Console.WriteLine("ModelState is invalid");
                return Page();


            }

            var user = await _userManager.FindByNameAsync(Input.UserName);


            if (user == null)
            {
                // If the user doesn't exist, show an error.
                //Console.WriteLine("User not found.");
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            if (await _userManager.CheckPasswordAsync(user, Input.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Role, user.Role ?? "User")
                };

                var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);

                return RedirectToPage("/Events/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

        }
    }
}
