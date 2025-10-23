using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    // Получить все заказы
    [HttpGet]
    public IActionResult GetAll()
    {
        var orders = _context.Orders
            .OrderByDescending(o => o.CreatedAt)
            .ToList();
        return Ok(orders);
    }

    // Получить заказ по ID
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var order = _context.Orders.Find(id);
        if (order == null)
            return NotFound($"Заказ с ID {id} не найден.");
        return Ok(order);
    }

    // Добавить новый заказ
    [HttpPost]
    public IActionResult Create([FromBody] Order newOrder)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        newOrder.CreatedAt = DateTime.UtcNow;
        newOrder.UpdatedAt = DateTime.UtcNow;
        newOrder.Status = string.IsNullOrEmpty(newOrder.Status) ? "Новый" : newOrder.Status;

        _context.Orders.Add(newOrder);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = newOrder.Id }, newOrder);
    }

    // Обновить статус заказа
    [HttpPatch("{id}/status")]
    public IActionResult UpdateStatus(int id, [FromBody] string newStatus)
    {
        var order = _context.Orders.Find(id);
        if (order == null)
            return NotFound();

        order.Status = newStatus;
        order.UpdatedAt = DateTime.UtcNow;

        _context.SaveChanges();
        return Ok(order);
    }

    // Полное обновление (PUT)
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Order updated)
    {
        var order = _context.Orders.Find(id);
        if (order == null)
            return NotFound();

        order.CustomerName = updated.CustomerName;
        order.TgUsername = updated.TgUsername;
        order.Phone = updated.Phone;
        order.ServiceIds = updated.ServiceIds;
        order.ServiceNames = updated.ServiceNames;
        order.TotalPrice = updated.TotalPrice;
        order.Status = updated.Status;
        order.UpdatedAt = DateTime.UtcNow;

        _context.SaveChanges();
        return Ok(order);
    }

    // Удалить заказ
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