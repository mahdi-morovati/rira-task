﻿using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
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
        result.Should().BeOfType<TodoTaskDto>();
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
        var result = await _service.CreateTaskAsync(newTask);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<TodoTaskDto>();
        result.Title.ShouldBe(newTask.Title);
        result.Description.ShouldBe(newTask.Description);

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

    [Fact]
    public async Task UpdateTask_ShouldUpdateTask_WhenTaskExists()
    {
        // Arrange
        var id = new Guid("c068b3f8-46d6-41e0-a519-18b40d7f7fab");
        var updateDto = new UpdateTodoTaskDto
        {
            Title = "Updt",
            Description = "Updt Desc"
        };

        // Act
        var result = await _service.UpdateTaskAsync(id, updateDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<TodoTaskDto>();
        result.Title.ShouldBe(updateDto.Title);
        result.Description.ShouldBe(updateDto.Description);
        
        _mockRepo.Verify(r =>
            r.UpdateAsync(It.Is<TodoTask>(la =>
                la.Id == id &&
                la.Title == updateDto.Title &&
                la.Description == updateDto.Description
            )), Times.Once);
    }

    [Fact]
    public async Task UpdateTask_ShouldThrowBadRequest_WhenTaskDoesNotExists()
    {
        // Arrange
        var id = new Guid("d222f030-8b0d-4e18-b70d-7f09bba030b3");
        var updateDto = new UpdateTodoTaskDto
        {
            Title = "Updt",
            Description = "Updt Desc"
        };

        // Act
        // Assert
        var exception = await Should.ThrowAsync<BadRequestException>(()
            => _service.UpdateTaskAsync(id, updateDto));
        exception.Message.ShouldBe("TodoTask not found");
    }

    [Fact]
    public async Task UpdateTask_ShouldReturnBadRequest_WhenInputIsInvqalid()
    {
        // Arrange
        var id = new Guid("c068b3f8-46d6-41e0-a519-18b40d7f7fab");
        var updateDto = new UpdateTodoTaskDto
        {
            Title = "",
            Description = "Updt Desc"
        };

        // Act
        // Assert
        var exception = await Should.ThrowAsync<BadRequestException>(()
            => _service.UpdateTaskAsync(id, updateDto));
        exception.Message.ShouldBe("Invalid TodoTask");
    }

    [Fact]
    public async Task UpdateTask_ShouldThrowException_WhenUpdateFails()
    {
        // Arrange
        var id = new Guid("c068b3f8-46d6-41e0-a519-18b40d7f7fab");
        var updateDto = new UpdateTodoTaskDto
        {
            Title = "Updt",
            Description = "Updt Desc"
        };
        
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<TodoTask>())).ThrowsAsync(new Exception("Update failed"));
        
        // Act
        // Assert
        await Assert.ThrowsAsync<Exception>(() => _service.UpdateTaskAsync(id, updateDto));

    }
    
    [Fact]
    public async Task DeleteTask_ShouldDeleteTask_WhenTaskExists()
    {
        // Arrange
        var existingTaskId = new Guid("c068b3f8-46d6-41e0-a519-18b40d7f7fab");

        // Act
        await _service.DeleteTaskAsync(existingTaskId);

        // Assert
        _mockRepo.Verify(r => r.DeleteAsync(It.Is<TodoTask>(t => t.Id == existingTaskId)), Times.Once);
    }

    
    [Fact]
    public async Task DeleteTask_ShouldThrowBadRequest_WhenTaskDoesNotExist()
    {
        // Arrange
        var nonExistentTaskId = new Guid("c068b3f8-46d6-41e0-a519-18b40d7f7fab");
        
        _mockRepo.Setup(r => r.GetByIdAsync(nonExistentTaskId)).ReturnsAsync((TodoTask)null);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _service.DeleteTaskAsync(nonExistentTaskId));
    }

    [Fact]
    public async Task DeleteTask_ShouldThrowException_WhenDatabaseErrorOccurs()
    {
        // Arrange
        var existingTaskId = new Guid("c068b3f8-46d6-41e0-a519-18b40d7f7fab");
        
        _mockRepo.Setup(r => r.DeleteAsync(It.IsAny<TodoTask>())).ThrowsAsync(new Exception("Database Error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.DeleteTaskAsync(existingTaskId));
        Assert.Equal("Database Error", exception.Message);
    }
    
    
}