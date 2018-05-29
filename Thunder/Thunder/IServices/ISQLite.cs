using SQLite;

namespace Thunder.IServices
{
    public interface ISQLite
    {
        SQLiteAsyncConnection GetSqLiteAsyncConnection();
    }
}
