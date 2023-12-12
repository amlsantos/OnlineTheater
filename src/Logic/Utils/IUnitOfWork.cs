using Logic.Customers;
using Logic.Movies;
using Logic.PurchasedMovies;

namespace Logic.Utils;

public interface IUnitOfWork
{
    PurchaseMovieRepository PurchaseMovies { get; }
    CustomerRepository Customers { get; }
    MovieRepository Movies { get; }
    
    int SaveChanges();
}