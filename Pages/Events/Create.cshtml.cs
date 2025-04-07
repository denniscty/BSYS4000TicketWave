using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketWave.Data;
using TicketWave.Models;

namespace TicketWave.Pages.Events
{
    public class CreateModel : PageModel
    {
        private readonly TicketWaveContext _context;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(TicketWaveContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public EventTickets EventTickets { get; set; } = default!;

        [BindProperty]
        public IFormFile? UploadImage { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("👉 OnPostAsync triggered");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("❌ ModelState is invalid");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("   - " + error.ErrorMessage);
                }
                return Page();
            }

            // Set seller ID from logged-in user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                Console.WriteLine("❌ No user ID found");
                return Unauthorized();
            }

            Console.WriteLine($"✅ User ID found: {userId}");
            EventTickets.EventListUserID = userId;
            EventTickets.EventBuyOfferAccepted = false;

            // Optional: autofill contact email from user claims
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (!string.IsNullOrEmpty(email))
            {
                EventTickets.EventUserContactEmail = email;
                Console.WriteLine($"📧 Using user email: {email}");
            }

            // Handle image upload
            if (UploadImage != null)
            {
                try
                {
                    Console.WriteLine($"📁 Uploading image: {UploadImage.FileName}");

                    var fileName = Path.GetFileName(UploadImage.FileName);
                    var imagePath = Path.Combine(_environment.WebRootPath, "images", "events");

                    if (!Directory.Exists(imagePath))
                    {
                        Directory.CreateDirectory(imagePath);
                        Console.WriteLine($"📂 Created image directory: {imagePath}");
                    }

                    var fullPath = Path.Combine(imagePath, fileName);
                    using var stream = new FileStream(fullPath, FileMode.Create);
                    await UploadImage.CopyToAsync(stream);

                    EventTickets.ImageFileName = fileName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Image upload error: " + ex.Message);
                }
            }

            try
            {
                _context.EventTickets.Add(EventTickets);
                await _context.SaveChangesAsync();
                Console.WriteLine("✅ Event successfully saved to database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ DB save failed: " + ex.Message);
            }

            return RedirectToPage("Index");
        }
    }
}
