using Microsoft.EntityFrameworkCore;
using MyApi.Models;
using MyApi.Services;

var builder = WebApplication.CreateBuilder(args);

//настройки сервисов ДО builder.Build()

// Настройка кодировки
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// Подключение строки к базе
builder.Services.AddDbContext<AtelierContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Настройка CORS для фронтенда ДО Build()
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000") 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Регистрация TelegramService
builder.Services.AddScoped<TelegramService>();



var app = builder.Build();

// Middleware настройка ПОСЛЕ Build()

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Использование CORS 
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Endpoints
app.MapGet("/", () => new  
{
    Message = "API ателье",
    Endpoints = new[]
    {
        "GET /api/orders - список заказов",
        "GET /api/users - список пользователей", 
        "GET /api/services - список услуг",
        "GET /swagger - документация API"
    },
    Timestamp = DateTime.UtcNow
}); 

app.Run();