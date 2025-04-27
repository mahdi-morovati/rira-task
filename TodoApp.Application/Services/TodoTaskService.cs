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

    public async Task<IReadOnlyCollection<TodoTaskDto>> GetAllTasksAsync()
    {
        var tasks = await _repository.GetAllAsync();
        return _mapper.Map<IReadOnlyCollection<TodoTaskDto>>(tasks);
    }

    public async Task<TodoTaskDto?> GetTaskByIdAsync(Guid id)
    {
        var task = await _repository.GetByIdAsync(id);
        return _mapper.Map<TodoTaskDto>(task);
    }

    public async Task<TodoTaskDto> CreateTaskAsync(CreateTodoTaskDto request)
    {
        //validate incoming data
        var validator = new CreateTodoTaskDtoValidator();
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation errors in create TodoTask for {0}", nameof(TodoTask));
            throw new BadRequestException("Invalid TodoTask", validationResult);
        }

        // convert to domain entity object
        var todoTask = _mapper.Map<TodoTask>(request);

        await _repository.AddAsync(todoTask);

        var todoTaskDto = _mapper.Map<TodoTaskDto>(todoTask);

        return todoTaskDto;
    }

    public async Task<TodoTaskDto> UpdateTaskAsync(Guid id, UpdateTodoTaskDto request)
    {
        var validator = new UpdateTodoTaskDtoValidator();
        var validationResult = await validator.ValidateAsync(request);
        
        var task = await _repository.GetByIdAsync(id);
        if (task == null)
        {
            throw new BadRequestException("TodoTask not found");
        }
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation errors in update request for {0} - {1}", nameof(TodoTask), id); 
            throw new BadRequestException("Invalid TodoTask", validationResult);
        }
        
        
        // convert to domain entity object
        _mapper.Map(request, task);

        // add to database
        await _repository.UpdateAsync(task);
        
        var todoTaskDto = _mapper.Map<TodoTaskDto>(task);

        return todoTaskDto;
    }
    
    public async Task DeleteTaskAsync(Guid id)
    {
        var task = await _repository.GetByIdAsync(id);
    
        if (task == null)
        {
            throw new BadRequestException("TodoTask not found");
        }

        await _repository.DeleteAsync(task);
    }

}