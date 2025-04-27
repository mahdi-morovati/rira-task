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
}