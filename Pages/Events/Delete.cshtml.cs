using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TicketWave.Data;
using TicketWave.Models;

namespace TicketWave.Pages.Events
{
    public class DeleteModel : PageModel
    {
        private readonly TicketWaveContext _context;

        public DeleteModel(TicketWaveContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EventTickets EventTickets { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var eventTickets = await _context.EventTickets.FirstOrDefaultAsync(m => m.EventId == id);
            if (eventTickets == null)
                return NotFound();

            EventTickets = eventTickets;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var eventTickets = await _context.EventTickets.FindAsync(id);
            if (eventTickets != null)
            {
                _context.EventTickets.Remove(eventTickets);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
