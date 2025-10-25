using Microsoft.AspNetCore.Mvc;


using MyApi.Models;
namespace MyApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly AtelierContext _context;

    public ServicesController(AtelierContext context)
    {
        _context = context;
    }

    // 🔹 Получить все услуги
    [HttpGet]
    public IActionResult GetAll()
    {
        var services = _context.Services.ToList();
        return Ok(services);
    }

    // 🔹 Получить услугу по ID
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var service = _context.Services.Find(id);
        if (service == null)
            return NotFound($"Услуга с ID {id} не найдена.");
        return Ok(service);
    }

    // 🔹 Добавить новую услугу
    [HttpPost]
    public IActionResult Create([FromBody] Service newService)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Services.Add(newService);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = newService.Id }, newService);
    }

    // 🔹 Обновить услугу
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Service updatedService)
    {
        var service = _context.Services.Find(id);
        if (service == null)
            return NotFound();

        service.Name = updatedService.Name;
        service.Price = updatedService.Price;
        _context.SaveChanges();

        return Ok(service);
    }

    // 🔹 Удалить услугу
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var service = _context.Services.Find(id);
        if (service == null)
            return NotFound();

        _context.Services.Remove(service);
        _context.SaveChanges();

        return NoContent();
    }
}

/* (http взаимодействие с бд)
 функционал:

Получить все услуги [HttpGet] *

Получить услугу по ID [HttpGet("{id}")] id

Добавить новую услугу [HttpPost] +

Обновить услугу [HttpPut("{id}")] upd

Удалить услугу [HttpDelete("{id}")] -
*/