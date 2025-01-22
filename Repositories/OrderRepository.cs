using SaleApp.Data;
using SaleApp.Models;
using SaleApp.Repositories.Interface;

namespace SaleApp.Repositories;

public class OrderRepository:Repository<Order>, IOrderRepository
{
     private ApplicationDbContext _db;

    public OrderRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
}