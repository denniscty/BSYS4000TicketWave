using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TicketWave.Data;
using TicketWave.Models;
using TicketWave.Services;
using System.Security.Claims;

namespace TicketWave.Pages.Events
{
    public class EditModel : PageModel
    {
        private readonly TicketWaveContext _context;
        private readonly ImageUploadService _imageUploader;

        private readonly IWebHostEnvironment _environment;

        public EditModel(TicketWaveContext context, IWebHostEnvironment environment, ImageUploadService imageUploader)
        {
            _context = context;
            _environment = environment;
            _imageUploader = imageUploader;
        }

        [BindProperty]
        public EventTickets EventTickets { get; set; } = default!;

        [BindProperty]
        public IFormFile? UploadImage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var eventTickets = await _context.EventTickets.FindAsync(id);
            if (eventTickets == null) return NotFound();

            // Prevent editing if offers exist
            bool hasOffers = await _context.EventOffers.AnyAsync(o => o.EventId == id);
            if (hasOffers)
            {
                TempData["EditError"] = "You cannot edit this listing because offers have been made.";
                return RedirectToPage("./Index");
            }

            // Prevent editing if event is in the past
            if (eventTickets.EventDateTime < DateTime.UtcNow)
            {
                TempData["EditError"] = "You cannot edit an event that has already passed.";
                return RedirectToPage("./Index");
            }

            EventTickets = eventTickets;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var original = await _context.EventTickets.FindAsync(EventTickets.EventId);
            if (original == null) return NotFound();

            // Only allow editing of selected fields
            original.EventName = EventTickets.EventName;
            original.EventDateTime = EventTickets.EventDateTime;
            original.EventLocation = EventTickets.EventLocation;
            original.EventTicketPrice = EventTickets.EventTicketPrice;
            original.EventNumOfTicket = EventTickets.EventNumOfTicket;
            original.EventUserContactEmail = EventTickets.EventUserContactEmail;
            original.EventDescription = EventTickets.EventDescription;
            original.ListingType = EventTickets.ListingType;

            // Handle image upload
            if (UploadImage != null)
            {
                var result = await _imageUploader.UploadAsync(UploadImage);

                if (!result.Success)
                {
                    ModelState.AddModelError("UploadImage", result.ErrorMessage ?? "Image upload failed.");
                    return Page();
                }

                // Delete old image if it exists
                if (!string.IsNullOrEmpty(original.ImageFileName))
                {
                    var oldPath = Path.Combine(_environment.WebRootPath, "images", "events", original.ImageFileName);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                // Save new image filename to DB
                original.ImageFileName = result.FileName;
            }


            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
