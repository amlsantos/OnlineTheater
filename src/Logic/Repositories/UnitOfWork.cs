namespace Logic.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    
    public PurchaseMovieRepository PurchaseMovies { get; }
    public CustomerRepository Customers { get; }
    public MovieRepository Movies { get; }

    public UnitOfWork(ApplicationDbContext context, PurchaseMovieRepository purchaseMovieRepository, CustomerRepository customersRepository, MovieRepository movieRepository)
    {
        _context = context;
        PurchaseMovies = purchaseMovieRepository;
        Customers = customersRepository;
        Movies = movieRepository;
    }
    
    public int SaveChanges() => _context.SaveChanges();
}