using Logic.Entities;
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
        entity.Property(e => e.PurchaseDate).HasColumnName("PurchaseDate");
        
        entity.Property(e => e.ExpirationDate).HasColumnName("ExpirationDate");
        // entity.Property(e => e.ExpirationDate).HasColumnType("datetime2");
    }
}