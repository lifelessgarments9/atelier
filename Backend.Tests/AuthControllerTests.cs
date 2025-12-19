using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using MyApi.Controllers;
using MyApi.Models;
using MyApi.Models.DTOs;
using MyApi.Services;

namespace Backend.Tests;

public class AuthControllerTests
{
    private AtelierContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AtelierContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AtelierContext(options);
    }
    //контроллер с моками
    private AuthController CreateController(AtelierContext context)
    {
        var telegramMock = new Mock<ITelegramService>();
        telegramMock
            .Setup(t => t.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        var loggerMock = new Mock<ILogger<AuthController>>();

        return new AuthController(
            context,
            telegramMock.Object,
            loggerMock.Object
        );
    }
    
    //Успешная регистрация
    [Fact]
    public async Task Register_NewUser_ReturnsOk()
    {
        var context = CreateContext();
        var controller = CreateController(context);

        var dto = new RegisterDto
        {
            TgUsername = "test1",
            Password = "sf8su5335"
        };

        var result = await controller.Register(dto);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, ok.StatusCode);

        Assert.Single(context.PendingRegistrations);
    }

    //Регистрация с существующим username
    [Fact]
    public async Task Register_ExistingUser_ReturnsBadRequest()
    {
        var context = CreateContext();
        context.Users.Add(new User { Username = "test1" });
        context.SaveChanges();

        var controller = CreateController(context);

        var dto = new RegisterDto
        {
            TgUsername = "test1",
            Password = "4h334h"
        };

        var result = await controller.Register(dto);

        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    //Успешный логин
    [Fact]
    public async Task Login_ValidCredentials_ReturnsOk()
    {
        var context = CreateContext();

        context.Users.Add(new User
        {
            Username = "testuser",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("u8up89")
        });
        context.SaveChanges();

        var controller = CreateController(context);

        var dto = new RegisterDto
        {
            TgUsername = "testuser",
            Password = "u8up89"
        };

        var result = await controller.Login(dto);

        Assert.IsType<OkObjectResult>(result);
    }
    
    //Неверный пароль
    [Fact]
    public async Task Login_InvalidPassword_ReturnsBadRequest()
    {
        var context = CreateContext();

        context.Users.Add(new User
        {
            Username = "testuser",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("8h9ubo")
        });
        context.SaveChanges();

        var controller = CreateController(context);

        var dto = new RegisterDto
        {
            TgUsername = "testuser",
            Password = "trddrt6"
        };

        var result = await controller.Login(dto);

        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    //неверный код Verify
    [Fact]
    public async Task Verify_InvalidCode_ReturnsBadRequest()
    {
        var context = CreateContext();

        context.PendingRegistrations.Add(new PendingRegistration
        {
            Username = "testuser",
            VerificationCode = "111111",
            ExpiresAt = DateTime.UtcNow.AddMinutes(10)
        });
        context.SaveChanges();

        var controller = CreateController(context);

        var dto = new VerifyTelegramDto
        {
            TgUsername = "testuser",
            Code = "000000"
        };

        var result = await controller.Verify(dto);

        Assert.IsType<BadRequestObjectResult>(result);
    }
}
