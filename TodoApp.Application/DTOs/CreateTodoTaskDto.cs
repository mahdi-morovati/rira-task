namespace TodoApp.Application.DTOs;

public class CreateTodoTaskDto
{
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
}