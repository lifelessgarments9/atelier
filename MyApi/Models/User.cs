using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApi.Models
{
    [Table("Users")] 
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        public long? TelegramChatId { get; set; }
        public bool IsTelegramVerified { get; set; } = false;
        
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
        public string Role { get; set; } = "Customer";

        // Навигация
        public ICollection<Order>? Orders { get; set; }
    }
}