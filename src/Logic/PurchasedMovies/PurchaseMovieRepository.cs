using Logic.Common;
using Logic.Utils;

namespace Logic.PurchasedMovies;

public class PurchaseMovieRepository : Repository<PurchasedMovie>
{
    public PurchaseMovieRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public IReadOnlyList<PurchasedMovie> GetList()
    {
        return Context.PurchasedMovies.ToList();
    }
}