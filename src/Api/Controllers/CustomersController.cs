using System.Runtime.InteropServices.JavaScript;
using Logic.Dtos;
using Logic.Entities;
using Logic.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController : BaseController
{
    public CustomersController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    [HttpGet("[action]")]
    public IActionResult Get(long id)
    {
        var customer = UnitOfWork.Customers.GetById(id);
        if (customer is null)
            return Error("There is no user, for the given id: " + id);

        var dto = new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name.Value,
            Email = customer.Email.Value,
            MoneySpent = customer.MoneySpent,
            Status = customer.Status.Type.ToString(),
            StatusExpirationDate = customer.Status.ExpirationDate,
            PurchasedMovies = customer.PurchasedMovies.Select(x => new PurchasedMovieDto
            {
                Price = x.Price,
                ExpirationDate = x.ExpirationDate,
                PurchaseDate = x.PurchaseDate,
                Movie = new MovieDto
                {
                    Id = x.Movie.Id,
                    Name = x.Movie.Name
                }
            }).ToList()
        };

        return Ok(dto);
    }

    [HttpGet("[action]")]
    public IActionResult GetList()
    {
        var customers = UnitOfWork.Customers.GetList();
        var dtos = customers.Select(x => new CustomerInListDto
        {
            Id = x.Id,
            Name = x.Name.Value,
            Email = x.Email.Value,
            MoneySpent = x.MoneySpent,
            Status = x.Status.Type.ToString(),
            StatusExpirationDate = x.Status.ExpirationDate,
        }).ToList();

        return Ok(dtos);
    }

    [HttpPost("[action]")]
    public IActionResult Create([FromBody] CreateCustomerDto item)
    {
        var nameOrError = Name.Create(item.Name);
        var emailOrError = Email.Create(item.Email);
        var validationResult = Result.Combine(nameOrError, emailOrError);

        if (validationResult.IsFailure)
            return Error(validationResult.Error);

        var email = emailOrError.Value.Value;
        var existingEmail = UnitOfWork.Customers.GetByEmail(email);
        if (existingEmail != null)
            return Error("Email is already in use: " + item.Email);

        var customer = new Customer(nameOrError.Value, emailOrError.Value);
        UnitOfWork.Customers.Add(customer);

        return Ok();
    }

    [HttpPut("[action]")]
    public IActionResult Update(long customerId, [FromBody] UpdateCustomerDto entity)
    {
        var nameOrError = Name.Create(entity.Name);
        if (nameOrError.IsFailure)
            return Error(nameOrError.Error);

        var existingCustomer = UnitOfWork.Customers.GetById(customerId);
        if (existingCustomer is null)
            return Error("Invalid customer id: " + customerId);

        existingCustomer.Name = nameOrError.Value;

        return Ok();
    }

    [HttpPut("[action]")]
    public IActionResult PurchaseMovie(long customerId, [FromBody] long movieId)
    {
        var movie = UnitOfWork.Movies.GetById(movieId);
        if (movie is null)
            return Error("Invalid movie id: " + movieId);

        var customer = UnitOfWork.Customers.GetById(customerId);
        if (customer is null)
            return Error("Invalid customer id: " + customerId);

        if (customer.AlreadyPurchasedMovie(movie.Id))
            return Error("The movie with id: " + movie.Id + " is already purchased: " + movie.Name);

        customer.PurchaseMovie(movie);

        return Ok();
    }

    [HttpPost("[action]")]
    public IActionResult PromoteCustomer(long customerId)
    {
        var existingCustomer = UnitOfWork.Customers.GetById(customerId);
        if (existingCustomer is null)
            return Error("Invalid customer id: " + customerId);

        if (existingCustomer.Status.IsAdvanced)
            return Error("The customer already has the Advanced status");

        var success = existingCustomer.Promote();
        if (!success)
            return Error("Cannot promote the customer");

        return Ok();
    }
}