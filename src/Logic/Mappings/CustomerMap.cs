using Logic.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logic.Mappings;

public class CustomerMap : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> entity)
    {
        entity.ToTable("Customer", "dbo");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("CustomerID");

        entity.HasMany<PurchasedMovie>(e => e.PurchasedMovies)
            .WithOne(e => e.Customer);
        
        entity.Property(e => e.Name).HasColumnName("Name");
        entity.Property(e => e.Name).HasConversion(v => v.Value,
            v => CustomerName.Create(v).Value);
        
        entity.Property(e => e.Email).HasColumnName("Email");
        entity.Property(e => e.Email).HasConversion(v => v.Value,
            v => Email.Create(v).Value);
        
        entity.Property(e => e.Status).HasColumnName("Status");
        entity.Property(e => e.StatusExpirationDate).HasColumnName("StatusExpirationDate");
        
        entity.Property(e => e.MoneySpent).HasColumnName("MoneySpent");
        entity.Property(e => e.MoneySpent).HasConversion(v => v.Value, 
            v => Dollars.Of(v));
    }
}