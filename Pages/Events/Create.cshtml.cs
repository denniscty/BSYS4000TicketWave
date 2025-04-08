using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketWave.Data;
using TicketWave.Models;
using TicketWave.Services;

namespace TicketWave.Pages.Events
{
    public class CreateModel : PageModel
    {
        private readonly TicketWaveContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ImageUploadService _imageUploader;

        public CreateModel(TicketWaveContext context, IWebHostEnvironment environment, ImageUploadService imageUploader)
        {
            _context = context;
            _environment = environment;
            _imageUploader = imageUploader;
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
            //Console.WriteLine("👉 OnPostAsync triggered");
            if (!ModelState.IsValid)
            {
                //Console.WriteLine("❌ ModelState is invalid");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    //Console.WriteLine("   - " + error.ErrorMessage);
                }
                return Page();
            }

            // Set seller ID from logged-in user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            EventTickets.EventListUserID = userId ?? throw new InvalidOperationException("User ID is null");

            if (userId == null)
            {
                //Console.WriteLine("❌ No user ID found");
                return Unauthorized();
            }

            //Console.WriteLine($"✅ User ID found: {userId}");
            EventTickets.EventListUserID = userId;
            EventTickets.EventBuyOfferAccepted = false;

            // Optional: autofill contact email from user claims
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (!string.IsNullOrEmpty(email))
            {
                EventTickets.EventUserContactEmail = email;
                //Console.WriteLine($"📧 Using user email: {email}");
            }

            // Handle image upload
            if (UploadImage != null)
            {
                var result = await _imageUploader.UploadAsync(UploadImage);

                if (!result.Success)
                {
                    ModelState.AddModelError("UploadImage", result.ErrorMessage ?? "Image upload failed.");
                    return Page();
                }

                EventTickets.ImageFileName = result.FileName;
            }            

            // blocks the creation of past event
            if (EventTickets.EventDateTime.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("EventTickets.EventDateTime", "You cannot list an event in the past.");
                return Page();
            }

            //try
            //{
            //    _context.EventTickets.Add(EventTickets);
            //    await _context.SaveChangesAsync();
            //    //Console.WriteLine("✅ Event successfully saved to database.");
            //}
            //catch (Exception ex)
            //{
                //Console.WriteLine("❌ DB save failed: " + ex.Message);
            //}
            Console.WriteLine($"📝 Creating event: {EventTickets.EventName}");
            Console.WriteLine($"📅 Date: {EventTickets.EventDateTime}");
            Console.WriteLine($"👤 UserId: {EventTickets.EventListUserID}");
            Console.WriteLine($"✔ OfferAccepted: {EventTickets.EventBuyOfferAccepted}");
            Console.WriteLine($"🎯 ListingType: {EventTickets.ListingType}");

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    Console.WriteLine($"❌ Validation Error: {error.ErrorMessage}");

                return Page();
            }

            _context.EventTickets.Add(EventTickets);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index", new { PageNumber = 1 });
        }
    }
}
