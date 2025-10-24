using System.ComponentModel.DataAnnotations;
namespace MyApi.Models;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Services")]
public class Service
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public decimal Price { get; set; }
}