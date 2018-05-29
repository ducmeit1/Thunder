namespace Thunder.IServices
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IJobRepository Jobs { get; }
    }
}
