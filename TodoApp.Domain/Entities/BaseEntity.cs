namespace TodoApp.Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
}