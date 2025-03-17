using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TicketWave.Data;
using TicketWave.Models;

namespace TicketWave.Pages_Events
{
    public class IndexModel : PageModel
    {
        private readonly TicketWaveContext _context;
        private readonly IConfiguration _configuration;

        public IndexModel(TicketWaveContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public PaginatedList<EventTickets> EventTickets { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public SelectList? EventName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchEvent { get; set; } = string.Empty;

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<EventTickets> Events { get; set; } = default!;
            

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        
        {
            int currentPageIndex = pageIndex ?? 1;
            var pageSize = _configuration.GetValue("PageSize", 3);


            //IQueryable<EventTickets> eventSearch = _context.EventTickets.AsQueryable();
            IQueryable<EventTickets> eventTicketsIQ = from e in _context.EventTickets
                                          select e;


            if (!string.IsNullOrEmpty(SearchString))
            {
                eventTicketsIQ = eventTicketsIQ.Where(e => e.EventName.Contains(SearchString));
            }

            EventTickets = await PaginatedList<EventTickets>.CreateAsync(
                eventTicketsIQ.AsNoTracking(), pageIndex ?? 1, pageSize
            );
            //EventTickets = await eventSearch.ToListAsync();
        }

        public async Task OnGetPaginatedAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<EventTickets> eventTicketsIQ = _context.EventTickets.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                eventTicketsIQ = eventTicketsIQ.Where(e => e.EventName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    eventTicketsIQ = eventTicketsIQ.OrderByDescending(e => e.EventName);
                    break;
                case "Date":
                    eventTicketsIQ = eventTicketsIQ.OrderBy(e => e.EventDateTime);
                    break;
                case "date_desc":
                    eventTicketsIQ = eventTicketsIQ.OrderByDescending(e => e.EventDateTime);
                    break;
                default:
                    eventTicketsIQ = eventTicketsIQ.OrderBy(e => e.EventName);
                    break;
            }

            var pageSize = _configuration.GetValue<int>("PageSize", 4);
            Events = await PaginatedList<EventTickets>.CreateAsync(eventTicketsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
