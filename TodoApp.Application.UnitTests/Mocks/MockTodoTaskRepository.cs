using Moq;
using TodoApp.Application.Contracts.Persistence;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.UnitTests.Mocks;

public class MockTodoTaskRepository
{
    public static Mock<ITodoTaskRepository> GetMockTodoTaskRepository()
    {
        var todoTasks = new List<TodoTask>
        {
            new TodoTask()
            {
                Id = new Guid("c068b3f8-46d6-41e0-a519-18b40d7f7fab"),
                Title = "task 1",
                Description = "desc 1"
            },
            new TodoTask()
            {
                Id = new Guid("cf97b1c6-11ed-43f0-bef0-418394aec1f4"),
                Title = "task 2",
                Description = "desc 2"
            }
        };

        var mockRepo = new Mock<ITodoTaskRepository>();
        
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(todoTasks);
        
        mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => todoTasks.FirstOrDefault(lt => Equals(lt.Id, id)));


        return mockRepo;
    }
}