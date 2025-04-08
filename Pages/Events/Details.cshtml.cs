using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TicketWave.Data;
using TicketWave.Models;
using TicketWave.Services;

namespace TicketWave.Pages.Events
{
    public class DetailsModel : PageModel
    {
        private readonly TicketWaveContext _context;
        private readonly OfferService _offerService;

        public DetailsModel(TicketWaveContext context, OfferService offerService)
        {
            _context = context;
            _offerService = offerService;
        }

        public EventTickets EventTickets { get; set; } = default!;
        public bool ShouldGreyscaleImage { get; set; }
        public bool IsOwner { get; set; }
        public string? AcceptedBuyerUsername { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var eventtickets = await _context.EventTickets
                .Include(e => e.ListingUser)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (eventtickets == null)
            {
                return NotFound();
            }

            EventTickets = eventtickets;

            if (EventTickets == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IsOwner = EventTickets.EventListUserID == userId;

            var acceptedOffer = await _context.EventOffers
                .Include(o => o.OfferedByUser)
                .FirstOrDefaultAsync(o => o.EventId == EventTickets.EventId && o.Status == OfferStatus.Accepted);

            AcceptedBuyerUsername = acceptedOffer?.OfferedByUser?.UserName;

            // Apply greyscale if event is past or offer is accepted
            ShouldGreyscaleImage = EventTickets.EventDateTime < DateTime.Now || acceptedOffer != null;

            return Page();
        }

        public async Task<IActionResult> OnPostOfferAsync(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                TempData["OfferError"] = "You must be logged in to make an offer.";
                return RedirectToPage("/Account/Login", new { area = "", returnUrl = Url.Page("/Events/Details", new { id }) });
            }

            var result = await _offerService.TrySubmitOfferAsync(id, userId);

            if (result != null)
                TempData["OfferError"] = result;
            else
                TempData["OfferSuccess"] = "Your offer has been submitted!";

            return RedirectToPage("Details", new { id });
        }
    }
}
