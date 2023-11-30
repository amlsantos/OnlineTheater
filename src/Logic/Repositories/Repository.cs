using Logic.Entities;
using Logic.Utils;

namespace Logic.Repositories;

public abstract class Repository<T> where T : Entity
{
    protected readonly ApplicationDbContext Context;
    protected Repository(ApplicationDbContext context)
    {
        Context = context;
    }

    public virtual T? GetById(long id)
    {
        return Context.Set<T>().Find(id);
    }

    public void Add(T entity)
    {
        Context.Set<T>().Add(entity);
    }

    public void Remove(T entity)
    {
        Context.Set<T>().Remove(entity);
    }
    
    public int SaveChanges()
    {
        return Context.SaveChanges();
    }
    
    public async Task<int> SaveChangesAsync()
    {
        return await Context.SaveChangesAsync();
    }
}