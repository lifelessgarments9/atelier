using Microsoft.AspNetCore.Mvc;

using MyApi.Models;
using MyApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AtelierContext _context;
        public AuthController(AtelierContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.TgUsername == dto.TgUsername))
                return BadRequest("Username already exists");

            using var sha = SHA256.Create();
            var hash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)));

            var user = new User
            {
                TgUsername = dto.TgUsername,
                PasswordHash = hash,
                Role = "Customer",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { user.Id, user.TgUsername });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            using var sha = SHA256.Create();
            var hash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)));

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.TgUsername == dto.TgUsername && u.PasswordHash == hash);

            if (user == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { user.Id, user.TgUsername, user.Role });
        }
    }
}