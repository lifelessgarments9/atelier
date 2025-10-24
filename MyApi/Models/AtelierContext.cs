
using Microsoft.EntityFrameworkCore;
namespace MyApi.Models;

public class AtelierContext : DbContext
{
    public AtelierContext(DbContextOptions<AtelierContext> options) : base(options) 
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().ToTable("orders");
        modelBuilder.Entity<User>().ToTable("users"); 
        modelBuilder.Entity<Service>().ToTable("services");
        
        // Настройка имен столбцов для Order
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(o => o.Id).HasColumnName("id");
            entity.Property(o => o.CustomerName).HasColumnName("customername");
            entity.Property(o => o.TgUsername).HasColumnName("tgusername");
            entity.Property(o => o.Phone).HasColumnName("phone");
            entity.Property(o => o.ServiceIds).HasColumnName("serviceids");
            entity.Property(o => o.ServiceNames).HasColumnName("servicenames");
            entity.Property(o => o.TotalPrice).HasColumnName("totalprice");
            entity.Property(o => o.Status).HasColumnName("status");
            entity.Property(o => o.CreatedAt).HasColumnName("createdat");
            entity.Property(o => o.UpdatedAt).HasColumnName("updatedat");
            entity.Property(o => o.UserId).HasColumnName("userid");
        });

        //для User
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Id).HasColumnName("id");
            entity.Property(u => u.TgUsername).HasColumnName("tgusername");
            entity.Property(u => u.PasswordHash).HasColumnName("passwordhash"); 
            entity.Property(u => u.FirstName).HasColumnName("firstname");
            entity.Property(u => u.LastName).HasColumnName("lastname");
            entity.Property(u => u.Phone).HasColumnName("phone");
            entity.Property(u => u.CreatedAt).HasColumnName("createdat");
            entity.Property(u => u.UpdatedAt).HasColumnName("updatedat");
            entity.Property(u => u.IsActive).HasColumnName("isactive");
            entity.Property(u => u.Role).HasColumnName("role");
        });
        //для Service
        modelBuilder.Entity<Service>(entity =>
        {
            entity.Property(s => s.Id).HasColumnName("id");
            entity.Property(s => s.Name).HasColumnName("name");
            entity.Property(s => s.Price).HasColumnName("price");
            
        });
        
        base.OnModelCreating(modelBuilder);

        // Настройка индексов
        modelBuilder.Entity<User>()
            .HasIndex(u => u.TgUsername)
            .HasDatabaseName("idx_users_tgUsername");

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.UserId)
            .HasDatabaseName("idx_orders_userId");

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.Status)
            .HasDatabaseName("idx_orders_status");

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.CreatedAt)
            .HasDatabaseName("idx_orders_createdat");

        // Настройка связи
        modelBuilder.Entity<Order>()       //в вашем классе Order нет свойства UserId!
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