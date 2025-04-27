using AutoMapper;
using TodoApp.Application.Contracts.Logging;
using TodoApp.Application.Contracts.Persistence;
using TodoApp.Application.DTOs;
using TodoApp.Application.Exceptions;
using TodoApp.Application.Validators;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Services;

public class TodoTaskService
{
    private readonly ITodoTaskRepository _repository;
    private readonly IAppLogger<TodoTaskService> _logger;
    private readonly IMapper _mapper;

    public TodoTaskService(ITodoTaskRepository repository, IAppLogger<TodoTaskService> logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<TodoTask>> GetAllTasksAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<TodoTask?> GetTaskByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateTaskAsync(CreateTodoTaskDto createDto)
    {
        //validate incoming data
        var validator = new CreateTodoTaskDtoValidator();
        var validationResult = await validator.ValidateAsync(createDto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation errors in create TodoTask for {0}", nameof(TodoTask)); 
            throw new BadRequestException("Invalid TodoTask", validationResult);
        }
        
        // convert to domain entity object
        var todoTask = _mapper.Map<TodoTask>(createDto);
        
        await _repository.AddAsync(todoTask);
    }
    
}