using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Utilities;

namespace Infrastructure.DB;

public static class DbSeed
{
    private static readonly ILogger s_log = Log.ForContext(typeof(DbSeed));
    
    public static void Seed()
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();

        AppData.EfOptionsAction(builder);

        using var dbContext = new AppDbContext(builder.Options);

        s_log.Information("Seeding database started");

        SeedResources(dbContext);

        dbContext.SaveChanges();

        s_log.Information("Seeding database finished");
    }

    private static void SeedResources(AppDbContext dbContext)
    {
        var appResourceKeys = typeof(RK)
            .GetFields()
            .Select(x => x.GetRawConstantValue() as string)
            .ToList();

        var cultures = dbContext.Cultures.ToList();
        var resources = dbContext.Resources.ToList();

        foreach (var culture in cultures)
        {
            foreach (var appResourceKey in appResourceKeys)
            {
                if (resources.Any(x => x.CultureId == culture.Id && x.Key == appResourceKey)) continue;

                var resource = new Resource { CultureId = culture.Id, Key = appResourceKey!, Value = appResourceKey! };
                dbContext.Resources.Add(resource);

                s_log.Information("Seeding new resource for {culture}: {resource}", culture.Name, resource.Key);
            }
        }
    }
}