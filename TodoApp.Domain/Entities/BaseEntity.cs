namespace TodoApp.Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime? DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
}