using System.Net.Http.Json;
using System.Text;

namespace MyApi.Services
{
    public class TelegramService
    {
        private readonly string _botToken;
        private readonly HttpClient _httpClient;
        private readonly ILogger<TelegramService> _logger;

        public TelegramService(IConfiguration config, ILogger<TelegramService> logger)
        {
            _botToken = config["BotConfig:Token"];
            _httpClient = new HttpClient();
            _logger = logger;
        }

        public async Task<bool> SendMessageAsync(string username, string message)
        {
            try
            {
                var cleanUsername = username.Trim().Replace("@", "");
                
                var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";
                var data = new
                {
                    chat_id = $"@{cleanUsername}",
                    text = message,
                    parse_mode = "Markdown"
                };

                var json = System.Text.Json.JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Сообщение отправлено пользователю @{cleanUsername}");
                    return true;
                }
                else
                {
                    try
                    {
                        var errorResponse = System.Text.Json.JsonSerializer.Deserialize<TelegramErrorResponse>(responseContent);
                        _logger.LogWarning($"Telegram API error: {errorResponse?.Description}");
                    }
                    catch
                    {
                        _logger.LogWarning($"Telegram raw error: {responseContent}");
                    }
                    
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка отправки сообщения пользователю @{username}");
                return false;
            }
        }

        public async Task<bool> SendMessageByChatId(long chatId, string message)
        {
            try
            {
                var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";
                var data = new
                {
                    chat_id = chatId,
                    text = message,
                    parse_mode = "Markdown"
                };

                var json = System.Text.Json.JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка отправки сообщения в чат {chatId}");
                return false;
            }
        }
    }

    public class TelegramErrorResponse
    {
        public bool Ok { get; set; }
        public int ErrorCode { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}