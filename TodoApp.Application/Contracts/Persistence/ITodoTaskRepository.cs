using TodoApp.Domain.Entities;

namespace TodoApp.Application.Contracts.Persistence;

public interface ITodoTaskRepository: IGenericRepository<TodoTask>
{
    
}