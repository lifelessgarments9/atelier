using System.ComponentModel.DataAnnotations;
namespace MyApi.Models;

public class Service
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public decimal Price { get; set; }
}