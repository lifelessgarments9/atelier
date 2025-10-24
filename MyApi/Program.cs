using Microsoft.EntityFrameworkCore;
using MyApi.Models;

var builder = WebApplication.CreateBuilder(args);

//настройка кодировки
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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

//endpoints
app.MapGet("/", () => new  //?
{
    Message = "API ателье работает!",
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