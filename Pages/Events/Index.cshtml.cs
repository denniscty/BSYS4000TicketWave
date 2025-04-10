using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TicketWave.Data;
using TicketWave.Models;
using System.Security.Claims;

namespace TicketWave.Pages.Events
{
    public class IndexModel : PageModel
    {
        private readonly TicketWaveContext _context;
        private const int PageSize = 9;

        public IndexModel(TicketWaveContext context)
        {
            _context = context;
        }

        public List<EventTickets> EventTickets { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SortOrder { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool ShowExpired { get; set; } = false;

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int TotalPages { get; set; }
        public int CurrentPage => PageNumber;

        public async Task OnGetAsync()
        {
            var query = _context.EventTickets
                .Include(e => e.ListingUser)
                .Include(e => e.EventOffers)
                .Where(e => !e.EventBuyOfferAccepted && !e.IsFlaggedByAdmin); // Hide accepted events or events banned by admin

            if (!ShowExpired)
            {
                var today = DateTime.UtcNow.Date;
                query = query.Where(e => e.EventDateTime.Date >= today);

            }

            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(e =>
                    e.EventName != null &&
                    e.EventName.ToLower().Contains(SearchString!));
            }

            query = SortOrder switch
            {
                "name_asc" => query.OrderBy(e => e.EventName != null ? e.EventName.ToLower() : string.Empty),
                "name_desc" => query.OrderByDescending(e => e.EventName != null ? e.EventName.ToLower() : string.Empty),
                "date_desc" => query.OrderByDescending(e => e.EventDateTime),
                _ => query.OrderBy(e => e.EventDateTime), // default date_asc
            };

            var totalCount = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            EventTickets = await query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
