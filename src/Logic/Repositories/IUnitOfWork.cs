namespace Logic.Repositories;

public interface IUnitOfWork
{
    PurchaseMovieRepository PurchaseMovies { get; }
    CustomerRepository Customers { get; }
    MovieRepository Movies { get; }
    
    int SaveChanges();
}