namespace MyApi.Models;
using System.ComponentModel.DataAnnotations;
public class UserDto
{
    public int? Id { get; set; }  // Nullable для создания
        
    [Required(ErrorMessage = "Telegram username обязателен")]
    public string TgUsername { get; set; } = string.Empty;
        
    // Password только для создания (обязателен при создании, игнорируется при обновлении)
    public string? Password { get; set; }
        
    [Required(ErrorMessage = "Имя обязательно")]
    public string FirstName { get; set; } = string.Empty;
        
    [Required(ErrorMessage = "Фамилия обязательна")]
    public string LastName { get; set; } = string.Empty;
        
    [Phone(ErrorMessage = "Неверный формат телефона")]
    public string? Phone { get; set; }
        
    public string Role { get; set; } = "Customer";
        
    // Только для возврата (игнорируются при создании/обновлении)
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool? IsActive { get; set; }
}