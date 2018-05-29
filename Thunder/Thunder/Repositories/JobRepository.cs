using SQLite;
using Thunder.IServices;
using Thunder.Models;

namespace Thunder.Repositories
{
    public class JobRepository : Repository<Job>, IJobRepository
    {
        public JobRepository(SQLiteAsyncConnection connection) : base(connection)
        {
            _connection.CreateTableAsync<Job>().Wait();
        }

        private SQLiteAsyncConnection _connection => base.Connection as SQLiteAsyncConnection;
    }
}
