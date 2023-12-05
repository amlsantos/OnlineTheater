namespace Logic.Entities;

public class PurchasedMovie : Entity
{
    public virtual long MovieId { get; set; }
    public virtual Movie Movie { get; set; }
    
    public virtual long CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
    
    public virtual Dollars Price { get; set; }
    public virtual DateTime PurchaseDate { get; set; }
    public virtual ExpirationDate ExpirationDate { get; set; }
}