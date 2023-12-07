namespace Logic.Entities;

public class Customer : Entity
{
    public virtual CustomerName Name { get; set; }
    public virtual Email Email { get; protected set; }
    public CustomerStatus Status { get; set; }
    public virtual Dollars MoneySpent { get; protected set; }

    private readonly IList<PurchasedMovie> _purchasedMovies;
    public virtual IReadOnlyList<PurchasedMovie> PurchasedMovies => _purchasedMovies.ToList();

    public Customer() => _purchasedMovies = new List<PurchasedMovie>();
    
    public Customer(CustomerName name, Email email) : this()
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        
        MoneySpent = Dollars.Of(0);
        Status = CustomerStatus.Regular;
    }

    public void AddPurchaseMovie(Movie movie, ExpirationDate expirationDate, Dollars price)
    {
        var purchasedMovie = new PurchasedMovie(movie, this, price, expirationDate);
        _purchasedMovies.Add(purchasedMovie);
        MoneySpent += price;
    }

    public bool Promote()
    {
        // at least 2 active movies during the last 30 days
        if (PurchasedMovies.Count(x => x.ExpirationDate == ExpirationDate.Infinite || x.ExpirationDate.Date >= DateTime.UtcNow.AddDays(-30)) < 2)
            return false;

        // at least 100 dollars spent during the last year
        if (PurchasedMovies.Where(x => x.PurchaseDate > DateTime.UtcNow.AddYears(-1)).Sum(x => x.Price) < 100m)
            return false;

        Status = Status.Promote();

        return true;
    }
}