using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Thunder.IServices
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        Task<TEntity> GetAsync(int id);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        AsyncTableQuery<TEntity> AsQueryable();
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> FindRange(Expression<Func<TEntity, bool>> predicate);
        Task<int> AddRange(IEnumerable<TEntity> entities);
        Task<int> Add(TEntity entity);
        Task<int> Remove(TEntity entity);
        Task<int> Update(TEntity entity);
    }
}
