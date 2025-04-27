using TodoApp.Application.Contracts.Persistence;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Services;

public class TodoTaskService
{
    private readonly ITodoTaskRepository _repository;

    public TodoTaskService(ITodoTaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<TodoTask>> GetAllTasksAsync()
    {
        return await _repository.GetAllAsync();
    }
}