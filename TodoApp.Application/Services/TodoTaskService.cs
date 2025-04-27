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

    public async Task<TodoTaskDto> UpdateTaskAsync(UpdateTodoTaskDto request)
    {
        var validator = new UpdateTodoTaskDtoValidator(_repository);
        var validationResult = await validator.ValidateAsync(request);
        
        // var task = await _repository.GetByIdAsync(id);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation errors in update request for {0} - {1}", nameof(TodoTask), request.Id); 
            throw new BadRequestException("Invalid TodoTask", validationResult);
        }
        
        
        // convert to domain entity object
        var task = _mapper.Map<TodoTask>(request);

        // add to database
        await _repository.UpdateAsync(task);
        
        var todoTaskDto = _mapper.Map<TodoTaskDto>(task);

        return todoTaskDto;
    }
}