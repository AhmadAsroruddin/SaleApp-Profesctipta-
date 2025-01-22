using SaleApp.Data;
using SaleApp.Models;
using SaleApp.Repositories.Interface;

namespace SaleApp.Repositories;

public class CustomerRepository:Repository<Customer>, ICustomerRepository
{
     private ApplicationDbContext _db;

    public CustomerRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
}