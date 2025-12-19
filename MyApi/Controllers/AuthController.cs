using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Models;
using MyApi.Models.DTOs;
using MyApi.Services;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AtelierContext _context;
        private readonly ITelegramService _telegram;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AtelierContext context, ITelegramService telegram, ILogger<AuthController> logger)
        {
            _context = context;
            _telegram = telegram;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var cleanUsername = request.TgUsername.Replace("@", "").Trim();
            
            // Проверяем, нет ли уже пользователя с таким username
            
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == cleanUsername);
                
            if (existingUser != null)
            {
                return BadRequest(new { message = "Пользователь с таким username уже существует" });
            }
            

            // Генерируем токен для deep link
            var token = Guid.NewGuid().ToString("N");
            var deepLink = $"https://t.me/AtelierAuthBot?start=register_{token}";

            // Сохраняем ожидающую регистрацию
            var pendingReg = new PendingRegistration
            {
                Token = token,
                Username = cleanUsername,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30)
            };

            _context.PendingRegistrations.Add(pendingReg);
            await _context.SaveChangesAsync();

            // Пытаемся отправить сообщение, но не блокируем процесс
            try
            {
                var messageSent = await _telegram.SendMessageAsync(cleanUsername,
                    $"🔐 Регистрация в Atelier\n\n" +
                    $"Для завершения регистрации перейдите по ссылке:\n{deepLink}");

                if (!messageSent)//false
                {
                    _logger.LogWarning($"Не удалось отправить сообщение пользователю @{cleanUsername}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Ошибка отправки сообщения пользователю @{cleanUsername}");
            }

            // Всегда возвращаем успех с deep link
            return Ok(new { 
                message = "Регистрация начата",
                deepLink = deepLink
            });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] VerifyTelegramDto request)
        {
            var cleanUsername = request.TgUsername.Replace("@", "").Trim();
            
            var pendingReg = await _context.PendingRegistrations
                .FirstOrDefaultAsync(pr => pr.Username == cleanUsername && 
                                         pr.VerificationCode == request.Code &&
                                         pr.ExpiresAt > DateTime.UtcNow);

            if (pendingReg == null)
            {
                return BadRequest(new { message = "Неверный код или код устарел" });
            }

            // Создаем пользователя
            var user = new User
            {
                Username = cleanUsername,
                PasswordHash = pendingReg.PasswordHash,
                TelegramChatId = pendingReg.TelegramChatId,
                IsTelegramVerified = true
            };

            _context.Users.Add(user);
            _context.PendingRegistrations.Remove(pendingReg);
            await _context.SaveChangesAsync();

            return Ok(new { 
                message = "Telegram успешно подтвержден",
                username = cleanUsername
            });
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] RegisterDto request)
        {
            var cleanUsername = request.TgUsername.Replace("@", "").Trim();
            
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == cleanUsername);
                
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest(new { message = "Неверный username или пароль" });
            }

            return Ok(new { 
                message = "Вход выполнен успешно",
                username = user.Username,
            });
        }
    }
}