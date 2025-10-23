
using Microsoft.EntityFrameworkCore;
namespace MyApi.Models;

public class AtelierContext : DbContext
{
    public AtelierContext(DbContextOptions<AtelierContext> options) : base(options) //?
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Настройка индексов
        modelBuilder.Entity<User>()
            .HasIndex(u => u.TgUsername)
            .HasDatabaseName("idx_Users_TgUsername");

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.UserId)
            .HasDatabaseName("idx_Orders_UserId");

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.Status)
            .HasDatabaseName("idx_Orders_Status");

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.CreatedAt)
            .HasDatabaseName("idx_Orders_CreatedAt");

        // Настройка связи
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Хранение массивов в PostgreSQL
        modelBuilder.Entity<Order>()
            .Property(o => o.ServiceIds)
            .HasColumnType("integer[]");

        modelBuilder.Entity<Order>()
            .Property(o => o.ServiceNames)
            .HasColumnType("text[]");
    }
}