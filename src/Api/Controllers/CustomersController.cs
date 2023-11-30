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

        return Ok(customer);
    }

    [HttpGet("[action]")]
    public IActionResult GetList()
    {
        var customers = _customerRepository.GetList();
        return Ok(customers);
    }

    [HttpPost("[action]")]
    public IActionResult Create([FromBody] Customer customer)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Values
                    .SelectMany(v => v.Errors);
                
                return BadRequest(errors);
            }
                

            if (_customerRepository.GetByEmail(customer.Email) != null)
                return BadRequest("Email is already in use: " + customer.Email);

            customer.Id = 0;
            customer.Status = CustomerStatus.Regular;
            
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
    public IActionResult Update(long id, [FromBody] Customer updatedCustomer)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Values
                    .SelectMany(v => v.Errors);
                
                return BadRequest(errors);
            }

            var existingCustomer = _customerRepository.GetById(id);
            if (existingCustomer is null)
                return BadRequest("Invalid customer id: " + id);

            existingCustomer.Name = updatedCustomer.Name;
            
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
            {
                return BadRequest("The customer already has the Advanced status");
            }

            var success = _customerService.PromoteCustomer(customer);
            if (!success)
            {
                return BadRequest("Cannot promote the customer");
            }

            _customerRepository.SaveChanges();
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }
}