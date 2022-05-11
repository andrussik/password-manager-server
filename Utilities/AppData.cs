using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Utilities;

public static class AppData
{
    public static IConfiguration Configuration = default!;
    
    public static readonly Action<DbContextOptionsBuilder> EfOptionsAction = o => o
        .UseNpgsql(Configuration[CK.DB_CONNECTION_STRING])
        .UseSnakeCaseNamingConvention();
}