using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TicketWave.Data;
using TicketWave.Models;

var builder = WebApplication.CreateBuilder(args);

// -------------------
// 🔧 Configure Services
// -------------------
builder.Services.AddRazorPages();

// Database
builder.Services.AddDbContext<TicketWaveContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TicketWaveContext") 
                      ?? throw new InvalidOperationException("Connection string 'TicketWaveContext' not found.")));

// Identity
builder.Services.AddIdentityCore<User>(options =>
{
    options.User.RequireUniqueEmail = true;
})
.AddSignInManager<SignInManager<User>>()
.AddEntityFrameworkStores<TicketWaveContext>();

// Authentication (Cookies)
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme, options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

builder.Services.AddAuthorization();

// -------------------
// 🔧 Build App
// -------------------
var app = builder.Build();

// Seed test data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

// -------------------
// 🔧 Middleware
// -------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.Run();
