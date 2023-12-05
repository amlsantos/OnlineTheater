using Logic.Entities;

namespace Logic.Extensions;

public static class DateTimeExtensions
{
    public static ExpirationDate ToExpirationDate(this DateTime? datetime)
    {
        if (!datetime.HasValue)
            return ExpirationDate.Infinite;

        return ExpirationDate.Create(datetime.Value).Value;
    }
}