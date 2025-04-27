using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.DTOs;
using TodoApp.Application.Services;

namespace TodoApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoTasksController : ControllerBase
{
    private readonly TodoTaskService _service;

    public TodoTasksController(TodoTaskService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<TodoTaskDto>>> GetAll()
    {
        var tasks = await _service.GetAllTasksAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoTaskDto>> GetById(Guid id)
    {
        var task = await _service.GetTaskByIdAsync(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TodoTaskDto>> Create([FromBody] CreateTodoTaskDto dto)
    {
        var createdTask = await _service.CreateTaskAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TodoTaskDto>> Update(Guid id, [FromBody] UpdateTodoTaskDto dto)
    {
        var updatedTask = await _service.UpdateTaskAsync(id, dto);
        return Ok(updatedTask);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _service.DeleteTaskAsync(id);
        return NoContent();
    }
}
