﻿using Logic.Entities;
using Logic.Utils;

namespace Logic.Repositories;

public class PurchaseMovieRepository : Repository<PurchasedMovie>
{
    public PurchaseMovieRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public IReadOnlyList<PurchasedMovie> GetList()
    {
        return Context.PurchasedMovies.ToList();
    }
}