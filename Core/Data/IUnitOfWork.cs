namespace Core.Data;

public interface IUnitOfWork
{
    Task SaveChanges();
    Task BeginTransaction();
    Task Commit();
}