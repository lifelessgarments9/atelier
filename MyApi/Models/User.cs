
using System.ComponentModel.DataAnnotations;
namespace MyApi.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string TgUsername { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string? Phone { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    public string Role { get; set; } = "Customer";

    // Навигация
    public ICollection<Order>? Orders { get; set; }

}