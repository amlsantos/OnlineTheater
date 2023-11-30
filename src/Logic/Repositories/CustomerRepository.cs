using Logic.Entities;
using Logic.Utils;
using Microsoft.EntityFrameworkCore;

namespace Logic.Repositories;

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

    public IReadOnlyList<Customer> GetList()
    {
        return Context.Customers.ToList()
            .Select(x =>
            {
                x.PurchasedMovies = null;
                return x;
            })
            .ToList();
    }

    public Customer GetByEmail(string email)
    {
        return Context.Customers.SingleOrDefault(x => x.Email == email);
    }
}