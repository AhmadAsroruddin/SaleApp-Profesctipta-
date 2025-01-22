using Microsoft.EntityFrameworkCore.Storage;
using SaleApp.Data;
using SaleApp.Models;
using SaleApp.Repositories.Interface;

namespace SaleApp.Repositories;

public class UnitOfWork:IUnitOfWork
{
    private readonly ApplicationDbContext dbContext;
    public IOrderRepository Order{ get; set; }
    public ICustomerRepository Customer{ get; set; }
    public IItemRepository Item{ get; set; }
    
    public UnitOfWork(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
        Order = new OrderRepository(this.dbContext);
        Customer = new CustomerRepository(this.dbContext);
        Item = new ItemRepository(this.dbContext);
    }
     public void Save()
    {
        dbContext.SaveChanges();
    }

    public IDbContextTransaction CreateTransaction()
    {
        return dbContext.Database.BeginTransaction();
    }
}