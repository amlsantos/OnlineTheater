﻿using Logic.Common;
using Logic.Customers;
using Logic.Movies;

namespace Logic.PurchasedMovies;

public class PurchasedMovie : Entity
{
    public virtual long MovieId { get; }
    public virtual Movie Movie { get; protected set; }
    
    public virtual long CustomerId { get; }
    public virtual Customer Customer { get; protected set; }
    
    public virtual Dollars Price { get; protected set; }
    public virtual DateTime PurchaseDate { get; protected set; }
    public virtual ExpirationDate ExpirationDate { get; protected set; }

    protected PurchasedMovie()
    {
        PurchaseDate = DateTime.Now;
    }

    internal PurchasedMovie(Movie movie, Customer customer, Dollars price, ExpirationDate expirationDate) : this()
    {
        Movie = movie ?? throw new ArgumentException(nameof(movie));
        MovieId = movie.Id;

        Customer = customer ?? throw new ArgumentException(nameof(customer));
        CustomerId = customer.Id;

        if (price is null || price.IsZero)
            throw new ArgumentException(nameof(price));
        Price = price;
        
        if (expirationDate == null || expirationDate.IsExpired)
            throw new ArgumentException(nameof(expirationDate));
        ExpirationDate = expirationDate;
    }
}