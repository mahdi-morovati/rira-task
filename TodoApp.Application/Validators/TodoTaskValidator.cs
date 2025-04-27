using FluentValidation;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Validators;

public class TodoTaskValidator: AbstractValidator<TodoTask>
{
    public TodoTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(20).WithMessage("{PropertyName} must be fewer than 20 characters");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(1000).WithMessage("{PropertyName} must be fewer than 1000 characters");
    }
}