using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketWave.Models;

namespace TicketWave.Data
{
    public class TicketWaveContext : IdentityDbContext<User>
    {
        public TicketWaveContext(DbContextOptions<TicketWaveContext> options)
            : base(options) { }



            public DbSet<EventTickets> EventTickets { get; set; }
    }
}
