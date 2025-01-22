using System.Linq.Expressions;

namespace SaleApp.Repositories.Interface
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, List<string>? includeProperties = null, bool tracked = false);
        IEnumerable<T> GetAllPaginated(Expression<Func<T, bool>>? filter = null, List<string>? includeProperties = null, bool tracked = false, int pageIndex = 1, int pageSize = 10);
        int CountData(Expression<Func<T, bool>>? filter = null, List<string>? includeProperties = null, bool tracked = false);

        T Get(Expression<Func<T, bool>> filter, List<string>? includeProperties = null, bool tracked = false);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        public IQueryable<T> Queryable(bool tracked = false);
        void RemoveRange(IEnumerable<T> entities);

    }
}