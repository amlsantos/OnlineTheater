namespace Logic.Entities;

public abstract class Movie : Entity
{
    public virtual string Name { get; init; }
    public virtual LicensingModel LicensingModel { get; init; }

    public abstract ExpirationDate GetExpirationDate();
    
    public Dollars CalculatePrice(CustomerStatus status)
    {
        var modifier = 1 - status.GetDiscount();
        return CalculatePriceCore() * modifier;
    }

    protected abstract Dollars CalculatePriceCore();
}

public class TwoDaysMovie : Movie
{
    public override ExpirationDate GetExpirationDate() => (ExpirationDate)DateTime.UtcNow.AddDays(2);
    protected override Dollars CalculatePriceCore() => Dollars.Of(4);
}

public class LifeLongMovie : Movie
{
    public override ExpirationDate GetExpirationDate() => ExpirationDate.Infinite;
    protected override Dollars CalculatePriceCore() => Dollars.Of(8);
}