using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TicketWave.Data;
using TicketWave.Models;

namespace TicketWave.Pages.Events
{
    public class MyEventsModel : PageModel
    {
        private readonly TicketWaveContext _context;

        public MyEventsModel(TicketWaveContext context)
        {
            _context = context;
        }

        public List<EventTickets> EventTickets { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public string? Filter { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToPage("/Account/Login");

            var query = _context.EventTickets
                .Include(e => e.EventOffers)
                .Where(e => e.EventListUserID == userId);

            if (!string.IsNullOrEmpty(Filter))
                query = query.Where(e => e.ListingType == Filter);

            EventTickets = await query
                .OrderByDescending(e => e.EventDateTime)
                .ToListAsync();

            return Page();
        }
    }
}
