using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Controllers;
using MyApi.Models;

namespace Backend.Tests;

public class ServicesControllerTest
{
    private AtelierContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AtelierContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AtelierContext(options);
    }

    private ServicesController CreateController(AtelierContext context)
    {
        return new ServicesController(context);
    }
    
    // Фабричный метод для Service
    private Service CreateValidService(int id = 1)
    {
        return new Service
        {
            Id = id,
            Name = $"Service {id}",
            Price = 100
        };
    }
    
    //GetAll
    [Fact]
    public void GetAll_ReturnsOk()
    {
        var context = CreateContext();
        context.Services.Add(CreateValidService());
        context.SaveChanges();

        var controller = CreateController(context);

        var result = controller.GetAll();

        Assert.IsType<OkObjectResult>(result);
    }
    
    //GetById — услуга не найдена
    [Fact]
    public void GetById_ServiceNotFound_ReturnsNotFound()
    {
        var context = CreateContext();
        var controller = CreateController(context);

        var result = controller.GetById(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }
    
    //успешное создание услуги
    [Fact]
    public void Create_ValidService_ReturnsCreated()
    {
        var context = CreateContext();
        var controller = CreateController(context);

        var service = CreateValidService();

        var result = controller.Create(service);

        Assert.IsType<CreatedAtActionResult>(result);
        Assert.Single(context.Services);
    }
    
    //успешное обновление
    [Fact]
    public void Update_ExistingService_ReturnsOk()
    {
        var context = CreateContext();

        var service = CreateValidService();
        context.Services.Add(service);
        context.SaveChanges();

        var controller = CreateController(context);

        var updated = CreateValidService();
        updated.Name = "Updated";
        updated.Price = 200;

        var result = controller.Update(1, updated);

        Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Updated", context.Services.First().Name);
        Assert.Equal(200, context.Services.First().Price);
    }
    
    //успешное удаление
    [Fact]
    public void Delete_ExistingService_ReturnsNoContent()
    {
        var context = CreateContext();

        context.Services.Add(CreateValidService());
        context.SaveChanges();

        var controller = CreateController(context);

        var result = controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
        Assert.Empty(context.Services);
    }
}