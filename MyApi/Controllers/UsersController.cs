using Microsoft.AspNetCore.Mvc;

using MyApi.Models;
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

        // Получить всех пользователей
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _context.Users.ToList();
            return Ok(users); // Возвращает JSON-массив
        }

        // Получить пользователя по ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // Создать нового пользователя
        [HttpPost]
        public IActionResult Create([FromBody] User newUser)
        {
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
        }

        // Удалить пользователя
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();

            return NoContent();
        }
}

/* (http взаимодействие с бд)
 функционал:

Получить всех пользователей [HttpGet] *

Получить пользователя по ID [HttpGet("{id}")] id

Создать нового пользователя [HttpPost] +

Удалить пользователя [HttpDelete("{id}")] - 
*/