using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using Shouldly;
using TodoApp.Application.Contracts.Logging;
using TodoApp.Application.Contracts.Persistence;
using TodoApp.Application.DTOs;
using TodoApp.Application.Exceptions;
using TodoApp.Application.MappingProfiles;
using TodoApp.Application.Services;
using TodoApp.Application.UnitTests.Mocks;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.UnitTests.Services;

public class TodoTaskServiceTests
{
    private readonly Mock<ITodoTaskRepository> _mockRepo;
    private readonly TodoTaskService _service;
    private readonly Mock<IAppLogger<TodoTaskService>> _mockLogger;
    private readonly IMapper _mapper;


    public TodoTaskServiceTests()
    {
        _mockLogger = new Mock<IAppLogger<TodoTaskService>>();
        _mockRepo = MockTodoTaskRepository.GetMockTodoTaskRepository();

        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<TaskMappingProfiles>();
        });
        _mapper = mapperConfig.CreateMapper();
    
        _service = new TodoTaskService(_mockRepo.Object, _mockLogger.Object, _mapper);
    }


    [Fact]
    public async Task GetAllTasks_ShouldReturnListOfTasks()
    {
        // Arrange & Act
        var result = await _service.GetAllTasksAsync();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IReadOnlyCollection<TodoTaskDto>>();
        result.Should().HaveCount(2);
        result.First().Title.Should().Be("task 1");

    }

    [Fact]
    public async Task GetTaskById_ShouldReturnTask_WhenTaskExists()
    {
        // Arrange
        var existingTaskId = new Guid("c068b3f8-46d6-41e0-a519-18b40d7f7fab");

        // Act
        var result = await _service.GetTaskByIdAsync(existingTaskId);

        // Assert
        result.Should().NotBeNull();  
        result.ShouldBeOfType<TodoTaskDto>();  
        result.Id.ShouldBe(existingTaskId); 
    }

    [Fact]
    public async Task GetTaskById_ShouldReturnNull_WhenTaskDoesNotExist()
    {
        // Arrange
        var notExistingTaskId = Guid.Empty;

        // Act
        var result = await _service.GetTaskByIdAsync(notExistingTaskId);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async Task CreateTask_ShouldAddTaskSuccessfully()
    {
        // Arrange
        var newTask = new CreateTodoTaskDto
        {
            Title = "task",
            Description = "task desc"
        };
        
        // Act
        await _service.CreateTaskAsync(newTask);

        // Assert
        _mockRepo.Verify(r => r.AddAsync(It.Is<TodoTask>(t =>
            t.Title == newTask.Title &&
            t.Description == newTask.Description
        )), Times.Once);

    }

    [Theory]
    [InlineData("", "Title is required")]
    [InlineData("bnsdfdfgdf", "Title must be fewer than 5 characters")]
    public async Task CreateTask_ShouldThrowValidationException_WhenTitleIsInvalid(string title, string errorMessage)
    {
        // Arrange
        var newTaskDto = new CreateTodoTaskDto
        {
            Title = title,
            Description = "test description"
        };

        // Act
        var exception = await Should.ThrowAsync<BadRequestException>(() => _service.CreateTaskAsync(newTaskDto));

        // Assert
        exception.Message.ShouldBe("Invalid TodoTask");
        exception.ValidationErrors.ShouldContainKey("Title");
        exception.ValidationErrors["Title"].ShouldContain(errorMessage);

        _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<TodoTask>()), Times.Never);
    }

    [Theory]
    [InlineData("", "Description is required")]
    [InlineData("bnsdfdfgdfgfddfsadsdasddf", "Description must be fewer than 10 characters")]
    public async Task CreateTask_ShouldThrowValidationException_WhenDescriptionIsInvalid(string description, string errorMessage)
    {
        // Arrange
        var newTaskDto = new CreateTodoTaskDto
        {
            Title = "title",
            Description = description
        };

        // Act
        var exception = await Should.ThrowAsync<BadRequestException>(() => _service.CreateTaskAsync(newTaskDto));

        // Assert
        exception.Message.ShouldBe("Invalid TodoTask");
        exception.ValidationErrors.ShouldContainKey("Description");
        exception.ValidationErrors["Description"].ShouldContain(errorMessage);

        _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<TodoTask>()), Times.Never);
    }
    
}