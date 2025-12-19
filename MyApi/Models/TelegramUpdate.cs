namespace MyApi.Models
{
    public class TelegramUpdate
    {
        public int UpdateId { get; set; }
        public TelegramMessage? Message { get; set; }
    }

    public class TelegramMessage
    {
        public int MessageId { get; set; }
        public TelegramUser From { get; set; } = new();
        public TelegramChat Chat { get; set; } = new();
        public string Text { get; set; } = string.Empty;
    }

    public class TelegramUser
    {
        public long Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
    }

    public class TelegramChat
    {
        public long Id { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}