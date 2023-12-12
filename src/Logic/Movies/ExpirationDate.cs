using Logic.Common;

namespace Logic.Movies;

public class ExpirationDate : ValueObject<ExpirationDate>
{
    public static readonly ExpirationDate Infinite = new(null);

    public DateTime? Date { get; }

    public bool IsExpired => this != Infinite && Date < DateTime.UtcNow;

    private ExpirationDate(DateTime? date)
    {
        Date = date;
    }

    public static Result<ExpirationDate> Create(DateTime date)
    {
        return Result.Ok(new ExpirationDate(date));
    }

    protected override bool EqualsCore(ExpirationDate other)
    {
        return Date == other.Date;
    }

    protected override int GetHashCodeCore()
    {
        return Date.GetHashCode();
    }

    public static explicit operator ExpirationDate(DateTime? date)
    {
        if (date.HasValue)
            return Create(date.Value).Value;

        return Infinite;
    }

    public static implicit operator DateTime? (ExpirationDate? date)
    {
        if (date is null)
            return Infinite.Date;
        
        return date.Date;
    }
}