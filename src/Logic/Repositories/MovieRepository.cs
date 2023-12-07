using Logic.Entities;

namespace Logic.Repositories;

public class MovieRepository : Repository<Movie>
{
    public MovieRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public IReadOnlyList<Movie> GetList()
    {
        return Context.Movies.ToList();
    }
}