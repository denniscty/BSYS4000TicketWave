using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TicketWave.Models;

namespace TicketWave.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public RegisterModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; } = new();

        public class RegisterInputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            //Console.WriteLine($"REGISTRATION ATTEMPT: {Input.UserName}");

            if (!ModelState.IsValid)
            {
                //Console.WriteLine("ModelState is invalid.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    //Console.WriteLine("Model validation error: " + error.ErrorMessage);
                }
                return Page();
            }

            // Check for duplicate username
            var existingUser = await _userManager.FindByNameAsync(Input.UserName);
            if (existingUser != null)
            {
                //Console.WriteLine("Duplicate registration attempt.");
                ModelState.AddModelError("Input.UserName", "Username is already taken.");
                return Page();
            }

            var user = new User
            {
                UserName = Input.UserName,
                Email = Input.Email,
                Role = "User"
            };

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                //Console.WriteLine("User registered successfully.");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("/Index");
            }

            //Console.WriteLine("User registration failed:");
            foreach (var error in result.Errors)
            {
                //Console.WriteLine(" - " + error.Description);
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

    }
}
