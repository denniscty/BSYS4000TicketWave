using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketWave.Data;
using TicketWave.Models;

namespace TicketWave.Pages.Events
{
    public class IndexModel : PageModel
    {
        private readonly TicketWaveContext _context;

        public IndexModel(TicketWaveContext context)
        {
            _context = context;
        }

        public IList<EventTickets> EventTickets { get; set; } = new List<EventTickets>();

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public SelectList? EventName { get; set; }

        public async Task OnGetAsync()
        {
            if (_context?.EventTickets != null)
            {
                var query = _context.EventTickets.AsQueryable();

                if (!string.IsNullOrEmpty(SearchString))
                {
                    query = query.Where(e => e.EventName!.Contains(SearchString));
                }

                EventTickets = await query.ToListAsync();
            }
        }
    }
}
