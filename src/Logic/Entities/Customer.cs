namespace Logic.Entities;

public class Customer : Entity
{
    public virtual CustomerName Name { get; set; }
    public virtual Email Email { get; set; }
    public virtual CustomerStatus Status { get; set; }
    public virtual ExpirationDate StatusExpirationDate { get; set; }
    public virtual Dollars MoneySpent { get; set; }
    public virtual IList<PurchasedMovie> PurchasedMovies { get; set; }
}