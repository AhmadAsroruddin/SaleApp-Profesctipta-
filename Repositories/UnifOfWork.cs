using Microsoft.EntityFrameworkCore.Storage;
using SaleApp.Data;
using SaleApp.Models;
using SaleApp.Repositories.Interface;

namespace SaleApp.Repositories;

public class UnitOfWork:IUnitOfWork
{
    private readonly ApplicationDbContext dbContext;
    public IOrderRepository Order{ get; set; }
    
    public UnitOfWork(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
        Order = new OrderRepository(this.dbContext);
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