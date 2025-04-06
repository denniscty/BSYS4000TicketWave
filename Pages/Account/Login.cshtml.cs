using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TicketWave.Data;
using TicketWave.Models;
using System.ComponentModel.DataAnnotations;

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

            var result = await _signInManager.PasswordSignInAsync(user, Input.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // If login succeeds, redirect to the home page or another page
                //Console.WriteLine("Login success!");
                return RedirectToPage("/Events/Index");
            }
            else
            {
                // If password is wrong, show an error
                //Console.WriteLine("Login failed.");
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }
    }
}
