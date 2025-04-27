using FluentValidation;
using TodoApp.Application.DTOs;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Validators;

public class CreateTodoTaskDtoValidator: AbstractValidator<CreateTodoTaskDto>
{
    public CreateTodoTaskDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(5).WithMessage("{PropertyName} must be fewer than 5 characters");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(10).WithMessage("{PropertyName} must be fewer than 10 characters");
    }
}