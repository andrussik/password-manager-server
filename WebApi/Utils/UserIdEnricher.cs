using System.Security.Claims;
using Serilog.Core;
using Serilog.Events;

namespace WebApi.Utils;

public class UserIdEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserIdEnricher() : this(new HttpContextAccessor()) { }

    public UserIdEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
            return;

        var logProperty = new LogEventProperty("UserId", new ScalarValue(userId));

        logEvent.AddPropertyIfAbsent(logProperty);
    }
}