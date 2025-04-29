using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketWave.Data;
using TicketWave.Models;

namespace TicketWave.Services.Admin
{
    public class DeleteEventService
    {
        private readonly TicketWaveContext _context;

        public DeleteEventService(TicketWaveContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            Console.WriteLine($"🧼 Admin attempting to delete event {id}");

            var evt = await _context.EventTickets
                .Include(e => e.EventOffers)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (evt == null)
            {
                Console.WriteLine("❌ Event not found");
                return false;
            }

            // Option 1: Soft-delete (recommended)
            evt.EventDescription += "\n\n[System: This event was removed by an admin for violation.]";
            evt.EventBuyOfferAccepted = false;
            evt.EventDateTime = DateTime.UtcNow.AddSeconds(-1); // consider it expired
            evt.IsFlaggedByAdmin = true;

            foreach (var offer in evt.EventOffers)
            {
                if (offer.Status == OfferStatus.Accepted)
                {
                    offer.Status = OfferStatus.Rejected;
                }
                else
                {
                    _context.EventOffers.Remove(offer);
                }
            }

            // Optional: hard-delete instead
            // _context.EventTickets.Remove(evt);

            await _context.SaveChangesAsync();
            Console.WriteLine("✅ Event deletion logic complete.");
            return true;
        }
    }
}
