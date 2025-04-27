using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;

namespace TodoApp.Persistence.Contexts;

public class TodoAppContext: DbContext
{
    public TodoAppContext(DbContextOptions<TodoAppContext> options): base(options)
    {
        
    }

    public DbSet<TodoTask> Tasks { get; set; }
}