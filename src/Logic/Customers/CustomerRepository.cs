using Logic.Common;
using Logic.Utils;
using Microsoft.EntityFrameworkCore;

namespace Logic.Customers;

public class CustomerRepository : Repository<Customer>
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override Customer? GetById(long id)
    {
        return Context.Customers
            .Include(c => c.PurchasedMovies)
            .ThenInclude(p => p.Movie)
            .FirstOrDefault(c => c.Id == id);
    }

    public IReadOnlyList<Customer> GetList() => Context.Customers.ToList();

    public Customer GetByEmail(string email)
    {
        var emailVO = Email.Create(email).Value;
        return Context.Customers.FirstOrDefault(c => c.Email == emailVO);
    }
}