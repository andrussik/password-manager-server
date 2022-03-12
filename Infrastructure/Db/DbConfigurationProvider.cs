using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Db;

public class DbConfigurationProvider : ConfigurationProvider
{
    public DbConfigurationProvider(Action<DbContextOptionsBuilder> optionsAction)
    {
        OptionsAction = optionsAction;
    }

    Action<DbContextOptionsBuilder> OptionsAction { get; }

    public override void Load()
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();

        OptionsAction(builder);

        using var dbContext = new AppDbContext(builder.Options);
        
        if (dbContext.Settings == null)
            throw new Exception("Null DB context");
            
        dbContext.Database.EnsureCreated();

        Data = dbContext.Settings.ToDictionary(c => c.Key, c => c.Value);
    }
}