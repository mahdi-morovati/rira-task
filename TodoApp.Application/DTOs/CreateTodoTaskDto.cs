using System.ComponentModel.DataAnnotations;

namespace TodoApp.Application.DTOs;

public class CreateTodoTaskDto
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = String.Empty;   
    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; } = String.Empty;
}