using Microsoft.EntityFrameworkCore;

namespace Utilities;

/// <summary>
/// Resources
/// </summary>
public static class RS
{
    private static readonly Dictionary<(string culture, string key), string> s_resources = new();

    public static void LoadResources()
    {
        using var dbContext = GetResourceDbContext();

        var resources = dbContext.Resources.Include(x => x.Culture).ToList();
        
        foreach (var resource in resources)
            s_resources.Add((resource.Culture!.Code, resource.Key), resource.Value);
    }

    private static string? GetValueAndUpdate(string culture, string resourceKey)
    {
        var exists = s_resources.TryGetValue((culture, resourceKey), out var value);

        if (exists)
            return value!;

        using var dbContext = GetResourceDbContext();

        value = dbContext.Resources
            .Include(x => x.Culture)
            .FirstOrDefault(x => x.Key == resourceKey && x.Culture!.Code == culture)
            ?.Value;

        if (value is not null)
            s_resources.Add((culture, resourceKey), value);

        return value;
    }

    /// <summary>
    /// Gets the value associated with the specified key using current culture.
    /// </summary>
    /// <param name="resourceKey">Resource key</param>
    /// <returns>Resource value</returns>
    public static string Get(string resourceKey)
    {
        var culture = Thread.CurrentThread.CurrentCulture.Name;
        var value = GetValueAndUpdate(culture, resourceKey);

        if (value is null)
            throw new Exception($"Resource {resourceKey} for culture {culture} not found.");

        return value;
    }

    /// <summary>
    /// Gets the value associated with the specified key using current culture.
    /// </summary>
    /// <param name="resourceKey">Resource key</param>
    /// <returns>Resource value</returns>
    public static string? GetValueOrDefault(string resourceKey) =>
        GetValueAndUpdate(Thread.CurrentThread.CurrentCulture.Name, resourceKey);

    public static string[] GetSupportedCultures()
    {
        using var dbContext = GetResourceDbContext();

        return dbContext.Cultures.Select(x => x.Code).ToArray();
    }

    private static ResourceDbContext GetResourceDbContext()
    {
        var builder = new DbContextOptionsBuilder<ResourceDbContext>();
        
        AppData.EfOptionsAction(builder);
        
        return new ResourceDbContext(builder.Options);
    }

    private class ResourceDbContext : DbContext
    {
        public ResourceDbContext(DbContextOptions<ResourceDbContext> options) : base(options) { }
        
        public DbSet<Culture> Cultures { get; set; } = default!;
        public DbSet<Resource> Resources { get; set; } = default!;
    }

    private class Culture
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = default!;
    }
    
    private class Resource
    {
        public Guid Id { get; set; }
        public string Key { get; set; } = default!;
        public string Value { get; set; } = default!;

        public Guid CultureId { get; set; } = default!;
        public Culture? Culture { get; set; }
    }
}