using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace WebApi.Utils;

public class DbConfigurationProvider : ConfigurationProvider
{
    public DbConfigurationProvider(Action<DbContextOptionsBuilder> optionsAction)
    {
        OptionsAction = optionsAction;
    }

    private Action<DbContextOptionsBuilder> OptionsAction { get; }

    public override void Load()
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();

        OptionsAction(builder);

        using var dbContext = new AppDbContext(builder.Options);

        dbContext.Database.EnsureCreated();

        Data = dbContext.Settings.ToDictionary(c => c.Key, c => c.Value);
    }
}