using SQLite;
using Thunder.IServices;

namespace Thunder.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(SQLiteAsyncConnection connection)
        {
            Users = new UserRepository(connection);
            Jobs = new JobRepository(connection);
        }

        public IUserRepository Users { get; private set; }
        public IJobRepository Jobs { get; private set; }
    }
}
