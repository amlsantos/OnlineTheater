﻿
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;

namespace Logic.Entities;

public class Customer : Entity
{
    [Required]
    [MaxLength(100, ErrorMessage = "Name is too long")]
    public virtual string Name { get; set; }

    [Required]
    [EmailAddress]
    public virtual string Email { get; set; }

    public virtual CustomerStatus Status { get; set; }
    public virtual DateTime? StatusExpirationDate { get; set; }
    public virtual decimal MoneySpent { get; set; }
    public virtual IList<PurchasedMovie> PurchasedMovies { get; set; }
}