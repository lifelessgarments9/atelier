namespace MyApi.Models.DTOs
{
    public class RegisterDto
    {
        public string TgUsername { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}