using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using TicketWave.Models;  // Ensure the correct namespace

namespace TicketWave.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TicketWaveContext(
                serviceProvider.GetRequiredService<DbContextOptions<TicketWaveContext>>()))
            {
                // ✅ Ensure the database exists
                context.Database.EnsureCreated();

                // ✅ Check if Users exist, if not, add test users
                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User { UserId = 1, UserName = "seller1", Email = "seller1@email.com", PasswordHash = "hashedpassword1" },
                        new User { UserId = 2, UserName = "buyer1", Email = "buyer1@email.com", PasswordHash = "hashedpassword2" },
                        new User { UserId = 3, UserName = "seller2", Email = "seller2@email.com", PasswordHash = "hashedpassword3" },
                        new User { UserId = 4, UserName = "buyer2", Email = "buyer2@email.com", PasswordHash = "hashedpassword4" }
                    );
                    context.SaveChanges();
                }

                // ✅ Check if Events exist, if not, add test events
                if (!context.EventTickets.Any())
                {
                    context.EventTickets.AddRange(
                        new EventTickets
                        {
                            EventName = "Rock Concert",
                            EventListUserID = 1,  // ✅ Assign existing UserId (Seller)
                            EventDateTime = DateTime.Now.AddDays(30),
                            EventLocation = "Downtown Stadium",
                            EventTicketPrice = 75.00m,
                            EventNumOfTicket = 2,
                            EventUserContactEmail = "seller1@email.com",
                            EventDescription = "Exciting live rock concert!",
                            EventBuyerID = null,  // No buyer yet
                            EventBuyOfferAccepted = false
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
