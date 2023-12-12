using Logic.Common;
using Logic.Utils;

namespace Logic.Movies;

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