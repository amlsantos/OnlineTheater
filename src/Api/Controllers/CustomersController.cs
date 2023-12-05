using Logic.Dtos;
using Logic.Entities;
using Logic.Repositories;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase 
{
    private readonly MovieRepository _movieRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly CustomerService _customerService;
    
    public CustomersController(MovieRepository movieRepository, CustomerRepository customerRepository, CustomerService customerService)
    {
        _movieRepository = movieRepository;
        _customerRepository = customerRepository;
        _customerService = customerService;
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
            Status = customer.Status.ToString(),
            StatusExpirationDate = customer.StatusExpirationDate,
            PurchasedMovies = customer.PurchasedMovies.Select(x => new PurchasedMovieDto
            {
                Price = x.Price,
                ExpirationDate = x.ExpirationDate,
                PurchaseDate = x.PurchaseDate,
                Movie = new MovieDto()
                {
                    Id = x.MovieId,
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
            Status = x.Status.ToString(),
            StatusExpirationDate = x.StatusExpirationDate
        }).ToList();
        
        return Ok(dtos);
    }

    [HttpPost("[action]")]
    public IActionResult Create([FromBody] CreateCustomerDto item)
    {
        try
        {
            var customerOrError = CustomerName.Create(item.Name);
            var emailOrError = Email.Create(item.Email);
            var validationResult = Result.Combine(customerOrError, emailOrError);
            
            if (validationResult.IsFailure)
                return BadRequest(validationResult.Error);
            
            if (_customerRepository.GetByEmail(emailOrError.Value.Value) != null)
                return BadRequest("Email is already in use: " + item.Email);

            var customer = new Customer
            {
                Name = customerOrError.Value,
                Email = emailOrError.Value,
                MoneySpent = Dollars.Of(0),
                Status = CustomerStatus.Regular,
                StatusExpirationDate = null
            };
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
    public IActionResult Update(long id, [FromBody] UpdateCustomerDto item)
    {
        try
        {
            var customerOrError = CustomerName.Create(item.Name);
            if (customerOrError.IsFailure)
                return BadRequest(customerOrError.Error);

            var existingCustomer = _customerRepository.GetById(id);
            if (existingCustomer is null)
                return BadRequest("Invalid customer id: " + id);

            existingCustomer.Name = customerOrError.Value;
            
            _customerRepository.SaveChanges();

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }

    [HttpPut("[action]")]
    public IActionResult PurchaseMovie(long id, [FromBody] long movieId)
    {
        try
        {
            var movie = _movieRepository.GetById(movieId);
            if (movie is null)
                return BadRequest("Invalid movie id: " + movieId);

            var customer = _customerRepository.GetById(id);
            if (customer is null)
                return BadRequest("Invalid customer id: " + id);

            if (customer.PurchasedMovies.Any(x => x.MovieId == movie.Id && (x.ExpirationDate == null || x.ExpirationDate.Value >= DateTime.UtcNow)))
            {
                return BadRequest("The movie is already purchased: " + movie.Name);
            }

            _customerService.PurchaseMovie(customer, movie);
            _customerRepository.SaveChanges();
            
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }

    [HttpPost("[action]")]
    public IActionResult PromoteCustomer(long id)
    {
        try
        {
            var customer = _customerRepository.GetById(id);
            if (customer is null)
                return BadRequest("Invalid customer id: " + id);

            if (customer.Status == CustomerStatus.Advanced && (customer.StatusExpirationDate == null || customer.StatusExpirationDate.Value < DateTime.UtcNow))
                return BadRequest("The customer already has the Advanced status");

            var success = _customerService.PromoteCustomer(customer);
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