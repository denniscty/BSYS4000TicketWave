using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TicketWave.Data;
using TicketWave.Models;

namespace TicketWave.Pages.Offers
{
    public class OffersInboxModel : PageModel
    {
        private readonly TicketWaveContext _context;

        public OffersInboxModel(TicketWaveContext context)
        {
            _context = context;
        }

        public List<OfferViewModel> OfferDetails { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? FilterType { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
                return Unauthorized();
            
            var offersQuery = _context.EventOffers
                .Include(o => o.Event)
                .Include(o => o.OfferedByUser)
                .Where(o => o.Event != null && o.Event.EventListUserID == currentUserId);

            if (!string.IsNullOrEmpty(FilterType))
            {
                offersQuery = offersQuery.Where(o => o.Event!.ListingType == FilterType);
            }

            // ✅ Materialize query

            var rawOffers = await offersQuery
                .Include(o => o.Event)
                .Include(o => o.OfferedByUser)
                .OrderByDescending(o => o.OfferDate)
                .ToListAsync();

            OfferDetails = rawOffers.Select(o => new OfferViewModel
            {
                OfferId = o.Id,
                EventId = o.EventId,
                EventName = o.Event?.EventName ?? "[No Title]",
                OfferDate = o.OfferDate,
                OfferedByUserName = o.OfferedByUser?.UserName ?? o.OfferedByUserId,
                ListingType = o.Event?.ListingType ?? "Sell",
                Status = o.Status
            }).ToList();

            return Page();
        }

        public class OfferViewModel
        {
            public int OfferId { get; set; }
            public int EventId { get; set; }
            public string EventName { get; set; } = string.Empty;
            public string OfferedByUserName { get; set; } = string.Empty;
            public DateTime OfferDate { get; set; }
            public string ListingType { get; set; } = string.Empty;
            public OfferStatus Status { get; set; }
        }

        public async Task<IActionResult> OnPostAcceptAsync(int id)
        {
            var offer = await _context.EventOffers
                .Include(o => o.Event)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (offer == null || offer.Event == null)
                return NotFound();

            if (offer.Event.EventDateTime < DateTime.Now)
            {
                TempData["OfferError"] = "This event is in the past and cannot accept new offers.";
                return RedirectToPage();
            }

            // Reject all other offers for this event
            var allOffers = await _context.EventOffers
                .Where(o => o.EventId == offer.EventId && o.Id != id)
                .ToListAsync();

            foreach (var otherOffer in allOffers)
            {
                if (otherOffer.Status == OfferStatus.Pending)
                    otherOffer.Status = OfferStatus.Rejected;
            }

            offer.Status = OfferStatus.Accepted;
            await _context.SaveChangesAsync();

            TempData["OfferSuccess"] = "Offer accepted.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRejectAsync(int id)
        {
            var offer = await _context.EventOffers
                .Include(o => o.Event)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (offer == null || offer.Status != OfferStatus.Pending)
                return NotFound();

            offer.Status = OfferStatus.Rejected;
            await _context.SaveChangesAsync();

            TempData["OfferSuccess"] = "Offer rejected.";
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostReopenAsync(int eventId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
                return Unauthorized();

            var evt = await _context.EventTickets
                .Include(e => e.EventOffers)
                .FirstOrDefaultAsync(e =>
                    e.EventId == eventId &&
                    e.EventListUserID == currentUserId &&
                    e.EventDateTime > DateTime.Now);

            if (evt == null)
            {
                TempData["OfferError"] = "Event not found or already expired.";
                return RedirectToPage();
            }

            foreach (var offer in evt.EventOffers)
            {
                if (offer.Status == OfferStatus.Accepted)
                {
                    offer.Status = OfferStatus.Rejected;
                }
            }

            await _context.SaveChangesAsync();

            TempData["OfferSuccess"] = "Event reopened. Previous offer(s) marked as rejected.";
            return RedirectToPage();
        }



    }
}
