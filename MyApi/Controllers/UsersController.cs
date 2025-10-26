using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Models;
using MyApi.Models.DTOs;
namespace MyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AtelierContext _context;

    public UsersController(AtelierContext context)
    {
        _context = context;
    }

    // GET: api/users - возвращаем UserDto
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _context.Users
            .Where(u => u.IsActive)
            .Select(u => new UserDto
            {
                Id = u.Id,
                TgUsername = u.Username,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Phone = u.Phone,
                Role = u.Role,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                IsActive = u.IsActive
            })
            .ToList();
            
        return Ok(users);
    }

    // GET: api/users/5 - возвращаем UserDto
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null || !user.IsActive)
            return NotFound();
            
        return Ok(MapToDto(user));
    }

    // POST: api/users - принимаем UserDto (без ID)
    [HttpPost]
    public IActionResult Create([FromBody] UserDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Проверяем что ID не передан (для создания)
        if (userDto.Id.HasValue)
            return BadRequest("Не указывайте ID при создании пользователя");

        // Проверяем обязательность пароля при создании
        if (string.IsNullOrEmpty(userDto.Password))
            return BadRequest("Пароль обязателен при создании пользователя");

        // Проверяем уникальность TgUsername
        if (_context.Users.Any(u => u.Username == userDto.TgUsername))
            return BadRequest("Пользователь с таким Telegram username уже существует");

        // Хэшируем пароль
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

        var newUser = new User
        {
            Username = userDto.TgUsername,
            PasswordHash = passwordHash,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Phone = userDto.Phone,
            Role = userDto.Role ?? "Customer", //Role = userDto.Role
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Users.Add(newUser);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, MapToDto(newUser));
    }

    // PUT: api/users/5 - принимаем UserDto
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UserDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = _context.Users.Find(id);
        if (user == null || !user.IsActive)
            return NotFound();

        // Проверяем что ID в пути совпадает с ID в DTO (если указан)
        if (userDto.Id.HasValue && userDto.Id.Value != id)
            return BadRequest("ID в пути не совпадает с ID в теле запроса");

        // Проверяем уникальность TgUsername (если меняется)
        if (userDto.TgUsername != user.Username && 
            _context.Users.Any(u => u.Username == userDto.TgUsername))
            return BadRequest("Пользователь с таким Telegram username уже существует");

        user.Username = userDto.TgUsername;
        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.Phone = userDto.Phone;
        user.Role = userDto.Role ?? user.Role; //Role = userDto.Role
        user.UpdatedAt = DateTime.UtcNow;

        // Обновляем пароль только если передан
        if (!string.IsNullOrEmpty(userDto.Password))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        }

        _context.SaveChanges();

        return Ok(MapToDto(user));
    }

    // PATCH: api/users/5/deactivate - деактивация
    [HttpPatch("{id}/deactivate")]
    public IActionResult Deactivate(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
            return NotFound();

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
        
        _context.SaveChanges();
        return Ok(new { message = "Пользователь деактивирован" });
    }

    // DELETE: api/users/5 - мягкое удаление
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
            return NotFound();

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
        
        _context.SaveChanges();
        return NoContent();
    }

    // GET: api/users/5/orders - заказы пользователя
    [HttpGet("{id}/orders")]
    public IActionResult GetUserOrders(int id)
    {
        var user = _context.Users
            .Include(u => u.Orders)
            .FirstOrDefault(u => u.Id == id && u.IsActive);
            
        if (user == null)
            return NotFound();

        var orders = user.Orders
            .Select(o => new
            {
                o.Id,
                o.CustomerName,
                o.Status,
                o.TotalPrice,
                o.CreatedAt
            })
            .ToList();

        return Ok(orders);
    }

    // Метод для преобразования Entity → DTO
    private UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            TgUsername = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            IsActive = user.IsActive
        };
    }
    
    // GET: api/users/by-username/{username} - поиск пользователя по username
    [HttpGet("by-username/{username}")]
    public IActionResult GetByUsername(string username)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.Username == username && u.IsActive);
        
        if (user == null)
            return NotFound();
            
        return Ok(MapToDto(user));
    }
    
}

/* (http взаимодействие с бд)
 функционал:

Получить всех пользователей [HttpGet] *

Получить пользователя по ID [HttpGet("{id}")] id

Создать нового пользователя [HttpPost] +

Удалить пользователя [HttpDelete("{id}")] - 

GET: api/users/by-username/{username} - поиск пользователя по username
*/