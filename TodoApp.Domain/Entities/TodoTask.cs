﻿namespace TodoApp.Domain.Entities;

public class TodoTask: BaseEntity
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime DueDate { get; set; }
}