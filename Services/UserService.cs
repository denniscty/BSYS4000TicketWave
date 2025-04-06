using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TicketWave.Models;

namespace TicketWave.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Register a new user
        public async Task<bool> RegisterUserAsync(string username, string email, string password)
        {
            var existingUser = await _userManager.FindByNameAsync(username);
            if (existingUser != null || await _userManager.FindByEmailAsync(email) != null)
            {
                return false; // Username or email already taken
            }

            var newUser = new User
            {
                UserName = username,
                Email = email,
                Role = "User"
            };

            var result = await _userManager.CreateAsync(newUser, password);
            return result.Succeeded;
        }

        // Authenticate user (Login)
        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return null;

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
            return result.Succeeded ? user : null;
        }

        // Get user by ID
        public async Task<User?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
    }
}
