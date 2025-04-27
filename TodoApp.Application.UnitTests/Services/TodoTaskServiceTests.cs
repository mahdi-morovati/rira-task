using FluentAssertions;
using Moq;
using Shouldly;
using TodoApp.Application.Contracts.Persistence;
using TodoApp.Application.Services;
using TodoApp.Application.UnitTests.Mocks;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.UnitTests.Services;

public class TodoTaskServiceTests
{
    private readonly Mock<ITodoTaskRepository> _mockRepo;
    private readonly TodoTaskService _service;

    public TodoTaskServiceTests()
    {
        _mockRepo = MockTodoTaskRepository.GetMockTodoTaskRepository();
        _service = new TodoTaskService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllTasks_ShouldReturnListOfTasks()
    {
        // Arrange & Act
        var result = await _service.GetAllTasksAsync();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IReadOnlyCollection<TodoTask>>();
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
        result.ShouldBeOfType<TodoTask>();  
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
        var newTask = new TodoTask
        {
            Id = Guid.NewGuid(),
            Title = "new task",
            Description = "new task desc"
        };
        
        // Act
        await _service.CreateTaskAsync(newTask);

        // Assert
        _mockRepo.Verify(r => r.AddAsync(newTask), Times.Once);
        
    }
}