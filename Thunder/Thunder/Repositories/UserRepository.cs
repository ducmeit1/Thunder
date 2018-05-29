using SQLite;
using Thunder.IServices;
using Thunder.Models;

namespace Thunder.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(SQLiteAsyncConnection connection) : base(connection)
        {
            _connection.CreateTableAsync<User>().Wait();
            //Task.Run(async () =>
            //{
            //    await _connection.InsertAsync(new User()
            //    {
            //        Name = "Duc",
            //        Address = "Brunei",
            //        ContactNumber = "5325315",
            //        Email = "ducmeit2015@gmail.com",
            //        IsVerified = true,
            //        Money = 100,
            //        Password = "123456"
            //    });
            //}).Wait();
        }

        public SQLiteAsyncConnection _connection => base.Connection as SQLiteAsyncConnection;
    }
}
