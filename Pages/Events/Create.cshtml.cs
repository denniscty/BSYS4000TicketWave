using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketWave.Data;
using TicketWave.Models;

namespace TicketWave.Pages_Events
{
    public class CreateModel : PageModel
    {
        private readonly TicketWave.Data.TicketWaveContext _context;

        public CreateModel(TicketWave.Data.TicketWaveContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public EventTickets EventTickets { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    // Log or display the error message
                    Console.WriteLine(error.ErrorMessage);
                }
                return Page();
            }

            _context.EventTickets.Add(EventTickets);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
