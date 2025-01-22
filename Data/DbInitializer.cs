using SaleApp.Models;

namespace SaleApp.Data;

public class DbInitializer : IDbInitializer
{
    private readonly ApplicationDbContext _db;

    public DbInitializer(ApplicationDbContext db)
    {
        _db = db;
    }

    public void Initialize()
    {
        if(_db.Customer.Count() == 0)
        {
            List<Customer> customers = [
                new Customer{CUSTOMER_NAME = "PROFES"},
                new Customer{CUSTOMER_NAME = "TITAN"},
                new Customer{CUSTOMER_NAME = "DIPS"},
            ];
            _db.AddRange(customers);
            _db.SaveChanges();
        }

    }
}