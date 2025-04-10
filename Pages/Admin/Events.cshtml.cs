using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TicketWave.Data;
using TicketWave.Models;
using TicketWave.Services.Admin;


namespace TicketWave.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class EventsModel : PageModel
    {
        private readonly TicketWaveContext _context;

        private readonly DeleteEventService _deleteEventService;

        public EventsModel(TicketWaveContext context, DeleteEventService deleteEventService)
        {
            _context = context;
            _deleteEventService = deleteEventService!;
        }

        public List<EventTickets> AllEvents { get; set; } = new();

        public async Task OnGetAsync()
        {
            AllEvents = await _context.EventTickets
                .Include(e => e.ListingUser)
                .Include(e => e.EventOffers)
                .Where(e => !e.IsFlaggedByAdmin)
                .OrderByDescending(e => e.EventDateTime)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            bool deleted = await _deleteEventService.DeleteEventAsync(id);

            if (!deleted)
                return NotFound();

            return RedirectToPage();
        }

    }
}
