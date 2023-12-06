namespace Logic.Entities;

public class PurchasedMovie : Entity
{
    public virtual long MovieId { get; protected set; }
    public virtual Movie Movie { get; protected set; }
    
    public virtual long CustomerId { get; protected set; }
    public virtual Customer Customer { get; protected set; }
    
    public virtual Dollars Price { get; protected set; }
    public virtual DateTime PurchaseDate { get; protected set; }
    public virtual ExpirationDate ExpirationDate { get; protected set; }

    protected PurchasedMovie() {}
    
    internal PurchasedMovie(Movie movie, Customer customer, Dollars price, ExpirationDate expirationDate)
    {
        if (price is null || price.IsZero)
            throw new ArgumentException(nameof(price));
        
        if (expirationDate == null || expirationDate.IsExpired)
            throw new ArgumentException(nameof(expirationDate));
        
        Movie = movie ?? throw new ArgumentException(nameof(movie));
        Customer = customer ?? throw new ArgumentException(nameof(customer));
        Price = price;
        ExpirationDate = expirationDate;
        PurchaseDate = DateTime.Now;
    }
}