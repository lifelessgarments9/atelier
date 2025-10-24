using Microsoft.AspNetCore.Mvc;

using MyApi.Models;
namespace MyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AtelierContext _context;

    public OrdersController(AtelierContext context)
    {
        _context = context;
    }

    // GET: api/orders - возвращаем OrderDto
    [HttpGet]
    public IActionResult GetAll()
    {
        var orders = _context.Orders
            .OrderByDescending(o => o.CreatedAt)
            .ToList()
            .Select(o => new OrderDto
            {
                Id = o.Id,
                CustomerName = o.CustomerName,
                TgUsername = o.TgUsername,
                Phone = o.Phone,
                ServiceIds = o.ServiceIds,
                ServiceNames = o.ServiceNames,
                TotalPrice = o.TotalPrice,
                Status = o.Status,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt
            });
        
        return Ok(orders);
    }

    // GET: api/orders/5 - возвращаем OrderDto
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var order = _context.Orders.Find(id);
        if (order == null)
            return NotFound($"Заказ с ID {id} не найден.");
        
        return Ok(MapToDto(order));
    }

    // POST: api/orders - принимаем OrderDto (без ID)
    [HttpPost]
    public IActionResult Create([FromBody] OrderDto orderDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Проверяем что ID не передан (для создания)
        if (orderDto.Id.HasValue)
            return BadRequest("Не указывайте ID при создании заказа");

        var newOrder = new Order
        {
            CustomerName = orderDto.CustomerName,
            TgUsername = orderDto.TgUsername,
            Phone = orderDto.Phone,
            ServiceIds = orderDto.ServiceIds,
            ServiceNames = orderDto.ServiceNames,
            TotalPrice = orderDto.TotalPrice,
            Status = orderDto.Status ?? "Новый",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Orders.Add(newOrder);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = newOrder.Id }, MapToDto(newOrder));
    }

    // PUT: api/orders/5 - принимаем OrderDto
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] OrderDto orderDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var order = _context.Orders.Find(id);
        if (order == null)
            return NotFound();

        // Проверяем что ID в пути совпадает с ID в DTO (если указан)
        if (orderDto.Id.HasValue && orderDto.Id.Value != id)
            return BadRequest("ID в пути не совпадает с ID в теле запроса");

        order.CustomerName = orderDto.CustomerName;
        order.TgUsername = orderDto.TgUsername;
        order.Phone = orderDto.Phone;
        order.ServiceIds = orderDto.ServiceIds;
        order.ServiceNames = orderDto.ServiceNames;
        order.TotalPrice = orderDto.TotalPrice;
        order.Status = orderDto.Status ?? order.Status;
        order.UpdatedAt = DateTime.UtcNow;

        _context.SaveChanges();
        
        return Ok(MapToDto(order));
    }

    // PATCH: api/orders/5/status
    [HttpPatch("{id}/status")]
    public IActionResult UpdateStatus(int id, [FromBody] string newStatus)
    {
        var order = _context.Orders.Find(id);
        if (order == null)
            return NotFound();

        order.Status = newStatus;
        order.UpdatedAt = DateTime.UtcNow;
        _context.SaveChanges();
        
        return Ok(new { message = "Статус обновлен", status = newStatus });
    }

    // DELETE: api/orders/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var order = _context.Orders.Find(id);
        if (order == null)
            return NotFound();

        _context.Orders.Remove(order);
        _context.SaveChanges();
        return NoContent();
    }

    // Метод для преобразования Entity → DTO
    private OrderDto MapToDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            TgUsername = order.TgUsername,
            Phone = order.Phone,
            ServiceIds = order.ServiceIds,
            ServiceNames = order.ServiceNames,
            TotalPrice = order.TotalPrice,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        };
    }
}

/* (http взаимодействие с бд)
 функционал:
Получить все заказы [HttpGet] *

Получить заказ по ID [HttpGet("{id}")] id

Добавить новый заказ [HttpPost] +

Обновить статус заказа [HttpPatch("{id}/status")] upd status

Полное обновление (PUT) upd

Удалить заказ [HttpDelete("{id}")] -
*/