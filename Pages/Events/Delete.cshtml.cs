using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TicketWave.Data;
using TicketWave.Models;

namespace TicketWave.Pages_Events
{
    public class DeleteModel : PageModel
    {
        private readonly TicketWave.Data.TicketWaveContext _context;

        public DeleteModel(TicketWave.Data.TicketWaveContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EventTickets EventTickets { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventtickets = await _context.EventTickets.FirstOrDefaultAsync(m => m.EventId == id);

            if (eventtickets is not null)
            {
                EventTickets = eventtickets;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventtickets = await _context.EventTickets.FindAsync(id);
            if (eventtickets != null)
            {
                EventTickets = eventtickets;
                _context.EventTickets.Remove(EventTickets);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
