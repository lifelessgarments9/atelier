using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Controllers;
using MyApi.Models;
using MyApi.Models.DTOs;

namespace Backend.Tests;

public class OrderControllerTests
{
    private AtelierContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AtelierContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AtelierContext(options);
    }
    
    private OrdersController CreateController(AtelierContext context)
    {
        return new OrdersController(context);
    }
    
    private Order CreateValidOrder(int id = 1)
    {
        return new Order
        {
            Id = id,
            CustomerName = $"Customer {id}",
            TgUsername = "testuser",
            Phone = $"+123456789{id}",
            ServiceIds = new List<int> { 1, 2, 3 },
            ServiceNames = new List<string> { "Service 1", "Service 2", "Service 3" },
            TotalPrice = 1000,
            Status = "New",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private OrderDto CreateValidOrderDto(int? id = null)
    {
        return new OrderDto
        {
            Id = id,
            CustomerName = "Test Client",
            TgUsername = "testuser",
            Phone = "+123456789",
            ServiceIds = new List<int> { 1, 2 },
            ServiceNames = new List<string> { "Service A", "Service B" },
            TotalPrice = 1500,
            Status = "New",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
    
    private User CreateValidUser(int id = 1)
    {
        return new User
        {
            Id = id,
            Username = "testuser",
            IsActive = true
        };
    }
    
    //GetAll 
    [Fact]
    public void GetAll_ReturnsOk()
    {
        var context = CreateContext();
        context.Orders.Add(CreateValidOrder());
        context.SaveChanges();

        var controller = CreateController(context);

        var result = controller.GetAll();

        Assert.IsType<OkObjectResult>(result);
    }
    
    //GetById заказ не найден
    [Fact]
    public void GetById_NotFound()
    {
        var context = CreateContext();
        var controller = CreateController(context);

        var result = controller.GetById(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }
    
    //успешное создание
    [Fact]
    public void Create_ValidOrder_ReturnsCreated()
    {
        var context = CreateContext();

        context.Users.Add(CreateValidUser());
        context.SaveChanges();

        var controller = CreateController(context);

        var dto = CreateValidOrderDto();

        var result = controller.Create(dto);

        Assert.IsType<CreatedAtActionResult>(result);
        Assert.Single(context.Orders);
    }
    
    //create - пользователь не найден
    [Fact]
    public void Create_UserNotFound_ReturnsBadRequest()
    {
        var context = CreateContext();
        var controller = CreateController(context);

        var dto = CreateValidOrderDto();
        dto.TgUsername = "unknown";

        var result = controller.Create(dto);

        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    //успешное обновление
    [Fact]
    public void UpdateStatus_ValidOrder_ReturnsOk()
    {
        var context = CreateContext();

        var order = CreateValidOrder();
        context.Orders.Add(order);
        context.SaveChanges();

        var controller = CreateController(context);

        var result = controller.UpdateStatus(1, "Done");

        Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Done", context.Orders.First().Status);
    }
    
    //успешное удаление
    [Fact]
    public void Delete_ExistingOrder_ReturnsNoContent()
    {
        var context = CreateContext();

        context.Orders.Add(CreateValidOrder());
        context.SaveChanges();

        var controller = CreateController(context);

        var result = controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
        Assert.Empty(context.Orders);
    }
}