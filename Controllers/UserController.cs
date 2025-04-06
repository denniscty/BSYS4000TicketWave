using Microsoft.AspNetCore.Mvc;
using TicketWave.Services;
using TicketWave.Models;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    // Register a new user
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.RegisterUserAsync(request.UserName, request.Email, request.Password);

        if (!result)
            return BadRequest("User registration failed. Email or Username might already exist.");

        return Ok("User registered successfully.");
    }

    // User Login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto request)
    {
        var user = await _userService.AuthenticateUserAsync(request.UserName, request.Password);

        if (user == null)
            return Unauthorized("Invalid username or password.");

        return Ok(new { message = "Login successful", user });
    }

    // Get user details
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
            return NotFound("User not found.");

        return Ok(user);
    }
}
