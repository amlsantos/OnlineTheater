using Logic.Entities;
using Logic.Utils;

namespace Logic.Repositories;

public class CustomerRepository : Repository<Customer>
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
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