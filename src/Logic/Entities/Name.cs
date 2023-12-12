namespace Logic.Entities;

public class Name : ValueObject<Name>
{
    public string Value { get; }

    private Name(string value)
    {
        Value = value;
    }

    public static Result<Name> Create(string customerName)
    {
        customerName = (customerName ?? string.Empty).Trim();

        if (customerName.Length == 0)
            return Result.Fail<Name>("Customer name should not be empty");

        if (customerName.Length > 100)
            return Result.Fail<Name>("Customer name is too long");

        return Result.Ok(new Name(customerName));
    }
    
    protected override bool EqualsCore(Name other)
    {
        return Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);
    }

    protected override int GetHashCodeCore()
    {
        return Value.GetHashCode();
    }
}