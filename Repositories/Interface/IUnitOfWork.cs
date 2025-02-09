using Microsoft.EntityFrameworkCore.Storage;

namespace SaleApp.Repositories.Interface;

public interface IUnitOfWork{
    public void Save();
    public IDbContextTransaction CreateTransaction();
    public ICustomerRepository Customer{ get; }
    public IOrderRepository Order{ get; }
    public IItemRepository Item{ get; }
}