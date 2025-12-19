using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Models;
using MyApi.Services;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TelegramWebhookController : ControllerBase
    {
        private readonly AtelierContext _context;
        private readonly TelegramService _telegram;
        private readonly ILogger<TelegramWebhookController> _logger;

        public TelegramWebhookController(AtelierContext context, TelegramService telegram, ILogger<TelegramWebhookController> logger)
        {
            _context = context;
            _telegram = telegram;
            _logger = logger;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> HandleWebhook([FromBody] TelegramUpdate update)
        {
            if (update.Message?.Text?.StartsWith("/start register_") == true)
            {
                var token = update.Message.Text.Replace("/start register_", "").Trim();
                var chatId = update.Message.Chat.Id;
                var username = update.Message.From.Username;

                var pendingReg = await _context.PendingRegistrations
                    .FirstOrDefaultAsync(pr => pr.Token == token && pr.ExpiresAt > DateTime.UtcNow);

                if (pendingReg == null)
                {
                    await _telegram.SendMessageByChatId(chatId, "❌ Недействительная или просроченная ссылка регистрации.");
                    return Ok();
                }

                // Сохраняем chat_id и генерируем код
                pendingReg.TelegramChatId = chatId;
                pendingReg.VerificationCode = new Random().Next(100000, 999999).ToString();
                await _context.SaveChangesAsync();

                // Отправляем код пользователю
                await _telegram.SendMessageByChatId(chatId, 
                    $"🔐 Код подтверждения Atelier\n\n" +
                    $"Ваш код: `{pendingReg.VerificationCode}`\n\n" +
                    $"Вернитесь на сайт и введите этот код для завершения регистрации.");
            }

            return Ok();
        }
    }
}