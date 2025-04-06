using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using TicketWave.Models;

namespace TicketWave.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<TicketWaveContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            // Ensure database exists and apply any pending migrations
            context.Database.Migrate();

            // ✅ Seed Users
            if (!context.Users.Any())
            {
                var users = new[]
                {
                    new { UserName = "seller1", Email = "seller1@email.com", Password = "Test@123" },
                    new { UserName = "buyer1", Email = "buyer1@email.com", Password = "Test@123" },
                    new { UserName = "seller2", Email = "seller2@email.com", Password = "Test@123" },
                    new { UserName = "buyer2", Email = "buyer2@email.com", Password = "Test@123" }
                };

                foreach (var u in users)
                {
                    var user = new User
                    {
                        UserName = u.UserName,
                        Email = u.Email,
                        Role = "User"
                    };

                    await userManager.CreateAsync(user, u.Password);
                }
            }

            // ✅ Seed EventTickets
            if (!context.EventTickets.Any())
            {
                var seller = await userManager.FindByNameAsync("seller1");
                if (seller != null)
                {
                    context.EventTickets.Add(new EventTickets
                    {
                        EventName = "Rock Concert",
                        EventListUserID = int.Parse(seller.Id), // Might require adjustment if EventListUserID was int
                        EventDateTime = DateTime.Now.AddDays(30),
                        EventLocation = "Downtown Stadium",
                        EventTicketPrice = 75.00m,
                        EventNumOfTicket = 2,
                        EventUserContactEmail = seller.Email,
                        EventDescription = "Exciting live rock concert!",
                        EventBuyerID = null,
                        EventBuyOfferAccepted = false
                    });

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
