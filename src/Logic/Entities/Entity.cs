namespace Logic.Entities;

public abstract class Entity
{
    public long Id { get; }

    public override bool Equals(object obj)
    {
        if (obj is not Entity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetRealType() != other.GetRealType())
            return false;

        if (Id == 0)
            return false;
        
        if (other.Id == 0)
            return false;

        return Id == other.Id;
    }

    public static bool operator ==(Entity a, Entity b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetRealType().ToString() + Id).GetHashCode();
    }
    
    private Type GetRealType()
    {
        var type = GetType();
        return this is Microsoft.EntityFrameworkCore.Proxies.Internal.IProxyLazyLoader
            ? type.BaseType
            : type;
    }
}