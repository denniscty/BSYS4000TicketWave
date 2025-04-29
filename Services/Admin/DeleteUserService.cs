using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketWave.Data;
using TicketWave.Models;

namespace TicketWave.Services.Admin
{
    public class DeleteUserService
    {
        private readonly TicketWaveContext _context;

        public DeleteUserService(TicketWaveContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _context.Users
                .Include(u => u.Events)
                .ThenInclude(e => e.EventOffers)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return false;

            // Step 1: Expire future events and mark them
            foreach (var evt in user.Events)
            {
                if (evt.EventDateTime > DateTime.UtcNow)
                {
                    evt.EventDateTime = DateTime.UtcNow.AddSeconds(-1); // force expiry
                    evt.EventDescription += "\n\n[System: Closed due to user deletion]";
                    evt.EventBuyOfferAccepted = false;
                }

                foreach (var offer in evt.EventOffers)
                {
                    if (offer.Status == OfferStatus.Accepted)
                        offer.Status = OfferStatus.Rejected;
                    else
                        _context.EventOffers.Remove(offer);
                }
            }

            // Step 2: Remove any offers they made
            var submittedOffers = await _context.EventOffers
                .Where(o => o.OfferedByUserId == userId)
                .ToListAsync();

            _context.EventOffers.RemoveRange(submittedOffers);

            // Step 3: Delete the user
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
