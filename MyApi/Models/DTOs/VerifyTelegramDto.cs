namespace MyApi.Models.DTOs
{
    public class VerifyTelegramDto
    {
        public string TgUsername { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}