using AutoMapper;
using TodoApp.Application.DTOs;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.MappingProfiles;

public class TaskMappingProfiles : Profile
{
    public TaskMappingProfiles()
    {
        CreateMap<TodoTask, TodoTaskDto>().ReverseMap();
        CreateMap<CreateTodoTaskDto, TodoTask>();
    }
}