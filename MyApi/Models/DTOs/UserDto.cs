namespace MyApi.Models.DTOs;
using System.ComponentModel.DataAnnotations;
public class UserDto
{
    public int? Id { get; set; }  // Nullable для создания
        
    public string TgUsername { get; set; } = string.Empty;
        
    // Password только для создания (обязателен при создании, игнорируется при обновлении)
    public string? Password { get; set; }
        
    public string FirstName { get; set; } = string.Empty;
        
    public string LastName { get; set; } = string.Empty;
        
    [Phone(ErrorMessage = "Неверный формат телефона")]
    public string? Phone { get; set; }
        
    public string Role { get; set; } = "Customer";
        
    // Только для возврата (игнорируются при создании/обновлении)
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool? IsActive { get; set; }
}