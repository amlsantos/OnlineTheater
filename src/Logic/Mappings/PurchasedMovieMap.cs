using Logic.Entities;
using Logic.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logic.Mappings;

public class PurchasedMovieMap : IEntityTypeConfiguration<PurchasedMovie>
{
    public void Configure(EntityTypeBuilder<PurchasedMovie> entity)
    {
        entity.ToTable("PurchasedMovie", "dbo");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("PurchasedMovieID");

        entity.HasOne<Customer>(e => e.Customer)
            .WithMany(e => e.PurchasedMovies);
        entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

        entity.HasOne<Movie>(e => e.Movie)
            .WithMany()
            .HasForeignKey(e => e.MovieId)
            .HasConstraintName("MovieId");
        entity.Property(e => e.MovieId).HasColumnName("MovieID");
        
        entity.Property(e => e.Price).HasColumnName("Price");
        entity.Property(e => e.Price).HasConversion(v => v.Value, v => Dollars.Of(v));
        
        entity.Property(e => e.PurchaseDate).HasColumnName("PurchaseDate");

        var expirationDate = entity.OwnsOne<ExpirationDate>(e => e.ExpirationDate);
        expirationDate.Property(e => e.Date).HasColumnName("ExpirationDate");
        expirationDate.Property(e => e.Date).IsRequired(false);
        expirationDate.Property(e => e.Date).HasConversion(v => v, v => v.ToExpirationDate());
    }
}