using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;

namespace TodoApp.Persistence.Contexts;

public class TodoAppContext: DbContext
{
    public TodoAppContext(DbContextOptions<TodoAppContext> options): base(options)
    {
        
    }

    public DbSet<TodoTask> Tasks { get; set; }
    
    /// <summary>
    /// Store current timestamp for DateCreated and DateModified columns in database
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in base.ChangeTracker.Entries<BaseEntity>()
                     .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
        {
            entry.Entity.DateModified = DateTime.Now;
            if (entry.State == EntityState.Added)
            {
                entry.Entity.DateCreated = DateTime.Now;
                entry.Entity.Id = Guid.NewGuid();
            }
            else
            {
                // Preserve the original value of DateCreated for modified entities
                entry.Property(nameof(BaseEntity.DateCreated)).IsModified = false;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
    
}