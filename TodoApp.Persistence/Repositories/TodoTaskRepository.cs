using TodoApp.Application.Contracts.Persistence;
using TodoApp.Domain.Entities;
using TodoApp.Persistence.Contexts;

namespace TodoApp.Persistence.Repositories;

public class TodoTaskRepository: GenericRepository<TodoTask>, ITodoTaskRepository
{
    public TodoTaskRepository(TodoAppContext context) : base(context)
    {
    }
}