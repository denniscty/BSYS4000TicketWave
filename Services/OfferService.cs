using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketWave.Data;
using TicketWave.Models;

namespace TicketWave.Services
{
    public class OfferService
    {
        private readonly TicketWaveContext _context;

        public OfferService(TicketWaveContext context)
        {
            _context = context;
        }

        public async Task<string?> TrySubmitOfferAsync(int eventId, string userId)
        {
            var eventItem = await _context.EventTickets.FindAsync(eventId);
            if (eventItem == null)
                return "Event not found.";

            if (eventItem.EventListUserID == userId)
                return "You cannot make an offer on your own listing.";

            if (eventItem.EventDateTime < DateTime.Now)
                return "You cannot make an offer on a past event.";

            bool alreadyOffered = await _context.EventOffers.AnyAsync(o =>
                o.EventId == eventId && o.OfferedByUserId == userId);

            if (alreadyOffered)
                return "You have already made an offer for this event.";

            _context.EventOffers.Add(new EventOffer
            {
                EventId = eventId,
                OfferedByUserId = userId,
                OfferDate = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return null;
        }
    }
}
