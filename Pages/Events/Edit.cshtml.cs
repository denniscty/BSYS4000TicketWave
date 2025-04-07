using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TicketWave.Data;
using TicketWave.Models;

namespace TicketWave.Pages.Events
{
    public class EditModel : PageModel
    {
        private readonly TicketWaveContext _context;

        public EditModel(TicketWaveContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EventTickets EventTickets { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var eventTickets = await _context.EventTickets.FindAsync(id);
            if (eventTickets == null)
                return NotFound();

            EventTickets = eventTickets;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Fetch the original entity from DB
            var eventInDb = await _context.EventTickets.FindAsync(EventTickets.EventId);
            if (eventInDb == null)
                return NotFound();

            // Manually update editable fields only (prevent overposting)
            eventInDb.EventName = EventTickets.EventName;
            eventInDb.EventLocation = EventTickets.EventLocation;
            eventInDb.EventDateTime = EventTickets.EventDateTime;
            eventInDb.EventDescription = EventTickets.EventDescription;
            eventInDb.EventNumOfTicket = EventTickets.EventNumOfTicket;
            eventInDb.EventTicketPrice = EventTickets.EventTicketPrice;
            eventInDb.EventUserContactEmail = EventTickets.EventUserContactEmail;

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
