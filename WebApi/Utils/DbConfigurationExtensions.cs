using Microsoft.EntityFrameworkCore;

namespace WebApi.Utils;

public static class DbConfigurationExtensions
{
    public static IConfigurationBuilder UseDbConfiguration(
        this IConfigurationBuilder builder,
        Action<DbContextOptionsBuilder> optionsAction)
    {
        return builder.Add(new DbConfigurationSource(optionsAction));
    }
}