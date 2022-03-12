using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Db;

public class DbConfigurationSource : IConfigurationSource
{
    private readonly Action<DbContextOptionsBuilder> _optionsAction;
    public DbConfigurationSource(Action<DbContextOptionsBuilder> optionsAction) => _optionsAction = optionsAction;

    public IConfigurationProvider Build(IConfigurationBuilder builder) => new DbConfigurationProvider(_optionsAction);
}