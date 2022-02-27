using Core.Data;

namespace Infrastructure.Db;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    
    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChanges() => await _dbContext.SaveChangesAsync();

    public async Task BeginTransaction() => await _dbContext.Database.BeginTransactionAsync();

    public async Task Commit() => await _dbContext.Database.CommitTransactionAsync();
}