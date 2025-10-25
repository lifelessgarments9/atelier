namespace MyApi.Models.DTOs;
using System.ComponentModel.DataAnnotations;

public class OrderDto
{

    public int? Id { get; set; }  // Nullable для создания (без ID)
    
    [Required(ErrorMessage = "Имя клиента обязательно")]
    public string CustomerName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Telegram username обязателен")]
    public string TgUsername { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Телефон обязателен")]
    [Phone(ErrorMessage = "Неверный формат телефона")]
    public string Phone { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Выберите хотя бы одну услугу")]
    [MinLength(1, ErrorMessage = "Выберите хотя бы одну услугу")]
    public List<int> ServiceIds { get; set; } = new();
    
    public List<string> ServiceNames { get; set; } = new();
    
    [Required(ErrorMessage = "Укажите общую стоимость")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Стоимость должна быть больше 0")]
    public decimal TotalPrice { get; set; }
    
    public string Status { get; set; } = "New";
    

    public DateTime? CreatedAt { get; set; }
    

    public DateTime? UpdatedAt { get; set; }
}