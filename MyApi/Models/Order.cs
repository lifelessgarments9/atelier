using System.ComponentModel.DataAnnotations;
namespace MyApi.Models;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Orders")]
public class Order
{
    [Key]
    public int Id { get; set; }

    // связь с пользователем
    public int? UserId { get; set; }
    public User? User { get; set; }

    // данные клиента
    [Required]
    public string CustomerName { get; set; }

    [Required]
    public string TgUsername { get; set; }

    [Required]
    public string Phone { get; set; }

    // массивы для ServiceIds и ServiceNames
    public List<int> ServiceIds { get; set; } = new();
    public List<string> ServiceNames { get; set; } = new();

    [Required]
    public decimal TotalPrice { get; set; }

    [Required]
    public string Status { get; set; } = "New";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}