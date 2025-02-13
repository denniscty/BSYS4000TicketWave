using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketWave.Data;
using TicketWave.Models;

namespace TicketWave.Pages_Events
{
    public class IndexModel : PageModel
    {
        private readonly TicketWave.Data.TicketWaveContext _context;

        public IndexModel(TicketWave.Data.TicketWaveContext context)
        {
            _context = context;
        }

        public IList<EventTickets> EventTickets { get;set; } = default!;

            [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public SelectList? EventName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchEvent { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            //EventTickets = await _context.EventTickets.ToListAsync();
            var eventSearch = from e in _context.EventTickets
                select e;
            if (!string.IsNullOrEmpty(SearchString))
            {
                eventSearch = eventSearch.Where(s => s.EventName.Contains(SearchString));
            }

            EventTickets = await eventSearch.ToListAsync();
        }
    }
}
