using Serilog;
using Serilog.Configuration;

namespace WebApi.Utils;

public static class UserLoggerConfigurationExtensions
{
    public static LoggerConfiguration WithUserId(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
        return enrichmentConfiguration.With<UserIdEnricher>();
    }
}