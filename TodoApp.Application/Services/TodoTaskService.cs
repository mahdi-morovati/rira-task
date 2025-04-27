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

    public async Task<TodoTask?> GetTaskByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateTaskAsync(TodoTask todoTask)
    {
        await _repository.AddAsync(todoTask);
    }
}