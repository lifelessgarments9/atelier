using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Controllers;
using MyApi.Models;
using MyApi.Models.DTOs;

namespace Backend.Tests;

public class UsersControllerTests
{
    private AtelierContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AtelierContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AtelierContext(options);
    }

    private UsersController CreateController(AtelierContext context)
    {
        return new UsersController(context);
    }
    
    private User CreateValidUser(int id = 1)
    {
        return new User
        {
            Id = id,
            Username = $"user{id}",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("t68oog86f"),
            TelegramChatId = 1000000 + id,
            IsActive = true,
            Role = "Customer"
        };
    }
    private UserDto CreateValidUserDto(int? id = null)
    {
        return new UserDto
        {
            Id = id,
            TgUsername = "testuser",
            Password = "t68oog86f",
        };
    }
    
    //GetAll
    [Fact]
    public void GetAll_ReturnsOnlyActiveUsers()
    {
        var context = CreateContext();
        
        var activeUser = CreateValidUser(1);
        activeUser.IsActive = true;
        
        var inactiveUser = CreateValidUser(2);
        inactiveUser.IsActive = false;
        
        context.Users.AddRange(activeUser, inactiveUser);
        context.SaveChanges();

        var controller = CreateController(context);

        var result = controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result);
        var users = Assert.IsAssignableFrom<IEnumerable<UserDto>>(ok.Value);

        Assert.Single(users);
        Assert.Equal("user1", users.First().TgUsername);
    }
    
    //GetById — пользователь не найден
    [Fact]
    public void GetById_UserNotFound_ReturnsNotFound()
    {
        var context = CreateContext();
        var controller = CreateController(context);

        var result = controller.GetById(1);

        Assert.IsType<NotFoundResult>(result);
    }
    
    //успешное создание пользователя
    [Fact]
    public void Create_ValidUser_ReturnsCreated()
    {
        var context = CreateContext();
        var controller = CreateController(context);

        var dto = CreateValidUserDto();

        var result = controller.Create(dto);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Single(context.Users);
        Assert.Equal("testuser", context.Users.First().Username);
    }
    
    //Create — username существует
    [Fact]
    public void Create_DuplicateUsername_ReturnsBadRequest()
    {
        var context = CreateContext();
        
        var existingUser = CreateValidUser();
        context.Users.Add(existingUser);
        context.SaveChanges();

        var controller = CreateController(context);

        var dto = CreateValidUserDto();
        dto.TgUsername = "user1"; // Дублируем username

        var result = controller.Create(dto);

        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    //успешное обновление
    [Fact]
    public void Update_ExistingUser_ReturnsOk()
    {
        var context = CreateContext();
        
        var user = CreateValidUser();
        context.Users.Add(user);
        context.SaveChanges();

        var controller = CreateController(context);

        var dto = CreateValidUserDto();
        dto.TgUsername = "updateduser";
        dto.FirstName = "Updated";

        var result = controller.Update(1, dto);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("updateduser", context.Users.First().Username);
    }
    
    //успешная деактивация
    [Fact]
    public void Deactivate_User_ReturnsOk()
    {
        var context = CreateContext();
        
        var user = CreateValidUser();
        context.Users.Add(user);
        context.SaveChanges();

        var controller = CreateController(context);

        var result = controller.Deactivate(1);

        Assert.IsType<OkObjectResult>(result);
        Assert.False(context.Users.First().IsActive);
    }
    
    //GetByUsername
    [Fact]
    public void GetByUsername_ExistingActiveUser_ReturnsOk()
    {
        var context = CreateContext();
        
        var user = CreateValidUser();
        context.Users.Add(user);
        context.SaveChanges();

        var controller = CreateController(context);

        var result = controller.GetByUsername("user1");

        var ok = Assert.IsType<OkObjectResult>(result);
        var dto = Assert.IsType<UserDto>(ok.Value);

        Assert.Equal("user1", dto.TgUsername);
    }
}