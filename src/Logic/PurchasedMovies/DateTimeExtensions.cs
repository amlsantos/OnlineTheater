using Logic.Movies;

namespace Logic.PurchasedMovies;

public static class DateTimeExtensions
{
    public static ExpirationDate ToExpirationDate(this DateTime? datetime)
    {
        if (!datetime.HasValue)
            return ExpirationDate.Infinite;

        return ExpirationDate.Create(datetime.Value).Value;
    }
}