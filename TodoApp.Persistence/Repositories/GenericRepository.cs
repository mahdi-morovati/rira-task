using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Contracts.Persistence;
using TodoApp.Domain.Entities;
using TodoApp.Persistence.Contexts;

namespace TodoApp.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly TodoAppContext Context;

    public GenericRepository(TodoAppContext context)
    {
        Context = context;
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await Context.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await Context.Set<T>().AsNoTracking().FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task AddAsync(T entity)
    {
        await Context.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        Context.Entry(entity).State = EntityState.Modified;
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        Context.Remove(entity);
        await Context.SaveChangesAsync();
    }
}