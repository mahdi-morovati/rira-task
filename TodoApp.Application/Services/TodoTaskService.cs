using TodoApp.Application.Contracts.Logging;
using TodoApp.Application.Contracts.Persistence;
using TodoApp.Application.Exceptions;
using TodoApp.Application.Validators;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Services;

public class TodoTaskService
{
    private readonly ITodoTaskRepository _repository;
    private readonly IAppLogger<TodoTaskService> _logger;

    public TodoTaskService(ITodoTaskRepository repository, IAppLogger<TodoTaskService> logger)
    {
        _repository = repository;
        _logger = logger;
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
        //validate incoming data
        var validator = new TodoTaskValidator();
        var validationResult = await validator.ValidateAsync(todoTask);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation errors in create TodoTask for {0}", nameof(TodoTask)); 
            throw new BadRequestException("Invalid TodoTask", validationResult);
        }
        
        await _repository.AddAsync(todoTask);
    }
    
}