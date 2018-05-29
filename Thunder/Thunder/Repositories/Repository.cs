using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Thunder.IServices;

namespace Thunder.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly SQLiteAsyncConnection Connection;

        public Repository(SQLiteAsyncConnection connection)
        {
            Connection = connection;
        }

        public async Task<int> Add(TEntity entity)
        {
            return await Connection.InsertAsync(entity);
        }

        public async Task<int> AddRange(IEnumerable<TEntity> entities)
        {
            return await Connection.InsertAllAsync(entities);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Connection.FindAsync<TEntity>(predicate);
        }

        public AsyncTableQuery<TEntity> AsQueryable()
        {
            return Connection.Table<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> FindRange(Expression<Func<TEntity, bool>> predicate)
        {
            return await Connection.Table<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await Connection.Table<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await Connection.FindAsync<TEntity>(id);
        }

        public async Task<int> Remove(TEntity entity)
        {
            return await Connection.DeleteAsync(entity);
        }

        public async Task<int> Update(TEntity entity)
        {
            return await Connection.UpdateAsync(entity);
        }
    }
}
