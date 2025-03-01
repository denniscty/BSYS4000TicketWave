using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketWave.Models; // Ensure this namespace is correct

namespace TicketWave.Data
{
    public class TicketWaveContext : DbContext
    {
        public TicketWaveContext(DbContextOptions<TicketWaveContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        
        public DbSet<EventTickets> EventTickets { get; set; }  // Ensure your model is added here


    }
}
