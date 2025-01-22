using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using SaleApp.Data;
using SaleApp.Repositories.Interface;

namespace SaleApp.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _db;

        protected DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public IQueryable<T> Queryable(bool tracked = false) => tracked ? dbSet : dbSet.AsNoTracking();

        public T Get(Expression<Func<T, bool>> filter, List<string>? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking();

            query = query.Where(filter);



            if (!includeProperties.IsNullOrEmpty())
            {
                includeProperties!.ForEach(include =>
                {
                    query = query.Include(include);
                });
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, List<string>? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties.IsNullOrEmpty() == false)
            {
                includeProperties!.ForEach(include =>
                {
                    query = query.Include(include);
                });
            }

            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public IEnumerable<T> GetAllPaginated(Expression<Func<T, bool>>? filter = null, List<string>? includeProperties = null, bool tracked = false, int pageIndex = 1, int pageSize = 10)
        {
            IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties.IsNullOrEmpty() == false)
            {
                includeProperties!.ForEach(include =>
                {
                    query = query.Include(include);
                });
            }

            query = query.Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize);

            return query.ToList();
        }

        public int CountData(Expression<Func<T, bool>>? filter = null, List<string>? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties.IsNullOrEmpty() == false)
            {
                includeProperties!.ForEach(include =>
                {
                    query = query.Include(include);
                });
            }

            return query.Count();
        }
    }
}