using SaleApp.Data;
using SaleApp.Models;
using SaleApp.Repositories.Interface;

namespace SaleApp.Repositories;

public class ItemRepository:Repository<Item>, IItemRepository
{
    private ApplicationDbContext _db;

    public ItemRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
}