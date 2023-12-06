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
}