using FluentValidation;
using TodoApp.Application.Contracts.Persistence;
using TodoApp.Application.DTOs;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Validators;

public class UpdateTodoTaskDtoValidator: AbstractValidator<UpdateTodoTaskDto>
{
    private readonly ITodoTaskRepository _repository;
    public UpdateTodoTaskDtoValidator(ITodoTaskRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Id)
            .NotNull()
            .MustAsync(TodoTaskExists);
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(5).WithMessage("{PropertyName} must be fewer than 5 characters");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(10).WithMessage("{PropertyName} must be fewer than 10 characters");
    }

    private async Task<bool> TodoTaskExists(Guid id, CancellationToken arg2)
    {
        var todoTask = await _repository.GetByIdAsync(id);

        return todoTask != null;
    }
}