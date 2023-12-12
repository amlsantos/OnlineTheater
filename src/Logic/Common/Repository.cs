using Logic.Utils;

namespace Logic.Common;

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
}