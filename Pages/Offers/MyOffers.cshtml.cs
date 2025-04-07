using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TicketWave.Data;
using TicketWave.Models;

namespace TicketWave.Pages.Offers
{
    public class MyOffersModel : PageModel
    {
        private readonly TicketWaveContext _context;

        public MyOffersModel(TicketWaveContext context)
        {
            _context = context;
        }

        public List<MyOfferViewModel> Offers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            Offers = await _context.EventOffers
                .Include(o => o.Event)
                .Where(o => o.OfferedByUserId == userId)
                .OrderByDescending(o => o.OfferDate)
                .Select(o => new MyOfferViewModel
                {
                    EventName = o.Event!.EventName!,
                    EventDateTime = o.Event.EventDateTime,
                    ListingType = o.Event.ListingType == "Sell" ? "Buy" : "Sell",
                    Location = o.Event.EventLocation ?? "",
                    ContactEmail = o.Status == OfferStatus.Accepted ? o.Event.EventUserContactEmail ?? "" : "",
                    Status = o.Status
                })
                .ToListAsync();

            return Page();
        }

        public class MyOfferViewModel
        {
            public string EventName { get; set; } = string.Empty;
            public DateTime EventDateTime { get; set; }
            public string ListingType { get; set; } = string.Empty;
            public string Location { get; set; } = string.Empty;
            public string ContactEmail { get; set; } = string.Empty;
            public OfferStatus Status { get; set; }
        }
    }
}
