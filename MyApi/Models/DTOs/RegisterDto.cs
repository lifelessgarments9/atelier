namespace MyApi.Models.DTOs;
using System.ComponentModel.DataAnnotations;

public class RegisterDto
{
    public string? TgUsername { get; set; }
    public string? Phone { get; set; }

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class LoginDto
{
    public string? TgUsername { get; set; }
    public string? Phone { get; set; }

    [Required]
    public string Password { get; set; } = string.Empty;
}