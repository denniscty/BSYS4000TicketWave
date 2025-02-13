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
    public class EditModel : PageModel
    {
        private readonly TicketWave.Data.TicketWaveContext _context;

        public EditModel(TicketWave.Data.TicketWaveContext context)
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

            var eventtickets =  await _context.EventTickets.FirstOrDefaultAsync(m => m.EventId == id);
            if (eventtickets == null)
            {
                return NotFound();
            }
            EventTickets = eventtickets;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(EventTickets).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventTicketsExists(EventTickets.EventId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool EventTicketsExists(int id)
        {
            return _context.EventTickets.Any(e => e.EventId == id);
        }
    }
}
