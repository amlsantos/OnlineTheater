using Logic.Entities;
using Logic.Extensions;
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
            v => Name.Create(v).Value);
        
        entity.Property(e => e.Email).HasColumnName("Email");
        entity.Property(e => e.Email).HasConversion(v => v.Value,
            v => Email.Create(v).Value);
        
        var status = entity.OwnsOne<CustomerStatus>(e => e.Status);
        status.Property(e => e.Type).HasColumnName("Status");
        status.Property(e => e.ExpirationDate).HasColumnName("StatusExpirationDate");
        status.Property(e => e.ExpirationDate).HasConversion(v => v.Date, v => v.ToExpirationDate());
        status.Property(e => e.ExpirationDate).IsRequired(false);
        
        entity.Property(e => e.MoneySpent).HasColumnName("MoneySpent");
        entity.Property(e => e.MoneySpent).HasConversion(v => v.Value, 
            v => Dollars.Of(v));
    }
}