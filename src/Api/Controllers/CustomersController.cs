using Logic.Dtos;
using Logic.Entities;
using Logic.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase 
{
    private readonly MovieRepository _movieRepository;
    private readonly CustomerRepository _customerRepository;
    
    public CustomersController(MovieRepository movieRepository, CustomerRepository customerRepository)
    {
        _movieRepository = movieRepository;
        _customerRepository = customerRepository;
    }
    
    [HttpGet("[action]")]
    public IActionResult Get(long id)
    {
        var customer = _customerRepository.GetById(id);
        if (customer is null)
            return NotFound();

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
                Movie = new MovieDto()
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
        var customers = _customerRepository.GetList();
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
        try
        {
            var nameOrError = CustomerName.Create(item.Name);
            var emailOrError = Email.Create(item.Email);
            var validationResult = Result.Combine(nameOrError, emailOrError);
            
            if (validationResult.IsFailure)
                return BadRequest(validationResult.Error);
            
            var email = emailOrError.Value.Value;
            var existingEmail = _customerRepository.GetByEmail(email);
            if (existingEmail != null)
                return BadRequest("Email is already in use: " + item.Email);

            var customer = new Customer(nameOrError.Value, emailOrError.Value);
            _customerRepository.Add(customer);
            _customerRepository.SaveChanges();

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }

    [HttpPut("[action]")]
    public IActionResult Update(long customerId, [FromBody] UpdateCustomerDto entity)
    {
        try
        {
            var nameOrError = CustomerName.Create(entity.Name);
            if (nameOrError.IsFailure)
                return BadRequest(nameOrError.Error);

            var existingCustomer = _customerRepository.GetById(customerId);
            if (existingCustomer is null)
                return BadRequest("Invalid customer id: " + customerId);

            existingCustomer.Name = nameOrError.Value;
            
            _customerRepository.SaveChanges();

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }

    [HttpPut("[action]")]
    public IActionResult PurchaseMovie(long customerId, [FromBody] long movieId)
    {
        try
        {
            var movie = _movieRepository.GetById(movieId);
            if (movie is null)
                return BadRequest("Invalid movie id: " + movieId);

            var customer = _customerRepository.GetById(customerId);
            if (customer is null)
                return BadRequest("Invalid customer id: " + customerId);
            
            if (customer.PurchasedMovies.Any(x => x.Movie.Id == movie.Id && !x.ExpirationDate.IsExpired))
                return BadRequest("The movie is already purchased: " + movie.Name);
            
            customer.PurchaseMovie(movie);
            _customerRepository.SaveChanges();
            
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }

    [HttpPost("[action]")]
    public IActionResult PromoteCustomer(long customerId)
    {
        try
        {
            var existingCustomer = _customerRepository.GetById(customerId);
            if (existingCustomer is null)
                return BadRequest("Invalid customer id: " + customerId);
            
            if (existingCustomer.Status.IsAdvanced)
                return BadRequest("The customer already has the Advanced status");
            
            var success = existingCustomer.Promote();
            if (!success)
                return BadRequest("Cannot promote the customer");
            
            _customerRepository.SaveChanges();
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }
}