﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logic.Movies;

public class MovieMap : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> entity)
    {
        entity.ToTable("Movie", "dbo");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("MovieID");
        entity.Property(e => e.Name).HasColumnName("Name");
        entity.Property(e => e.LicensingModel).HasColumnName("LicensingModel");
        
        entity.HasDiscriminator(e => e.LicensingModel)
            .HasValue<TwoDaysMovie>(LicensingModel.TwoDays)
            .HasValue<LifeLongMovie>(LicensingModel.LifeLong);
    }
}