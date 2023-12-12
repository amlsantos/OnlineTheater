namespace Logic.Entities;

public class Customer : Entity
{
    public Name Name { get; set; }
    public Email Email { get; }
    public CustomerStatus Status { get; private set; }
    public Dollars MoneySpent { get; set; }

    private readonly IList<PurchasedMovie> _purchasedMovies;
    public virtual IReadOnlyList<PurchasedMovie> PurchasedMovies => _purchasedMovies.ToList();

    public Customer() => _purchasedMovies = new List<PurchasedMovie>();
    
    public Customer(Name name, Email email) : this()
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        
        MoneySpent = Dollars.Of(0);
        Status = CustomerStatus.Regular;
    }

    public bool HasPurchasedMovie(Movie movie)
    {
        return PurchasedMovies.Any(x => x.Movie == movie && !x.ExpirationDate.IsExpired);
    }

    public void PurchaseMovie(Movie movie)
    {
        if (HasPurchasedMovie(movie))
            throw new Exception();
        
        var expirationDate =  movie.GetExpirationDate();
        var price = movie.CalculatePrice(Status);
        
        var purchasedMovie = new PurchasedMovie(movie, this, price, expirationDate);
        _purchasedMovies.Add(purchasedMovie);
        MoneySpent += price;
    }

    public Result CanPromote()
    {
        if (Status.IsAdvanced)
            return Result.Fail("The customer already has the Advanced status");
        
        if (PurchasedMovies.Count(x => x.ExpirationDate == ExpirationDate.Infinite || x.ExpirationDate.Date >= DateTime.UtcNow.AddDays(-30)) < 2)
            return Result.Fail("The customer has to have at least 2 active movies during the last 30 days");
        
        if (PurchasedMovies.Where(x => x.PurchaseDate > DateTime.UtcNow.AddYears(-1)).Sum(x => x.Price) < 100m)
            return Result.Fail("The customer has to have at least 100 dollars spent during the last year");

        return Result.Ok();
    }
    
    public void Promote()
    {
        if (CanPromote().IsFailure)
            throw new Exception();
        
        Status = Status.Promote();
    }
}