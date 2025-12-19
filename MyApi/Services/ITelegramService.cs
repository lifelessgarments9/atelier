
namespace MyApi.Services
{
    public interface ITelegramService
    {
        Task<bool> SendMessageAsync(string username, string message);
    }
}