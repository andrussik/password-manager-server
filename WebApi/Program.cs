using System.Text;
using System.Text.Json.Serialization;
using Core;
using Domain.Entities;
using Infrastructure;
using Infrastructure.DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.SystemConsole.Themes;
using Utilities;
using WebApi.Filters;
using WebApi.Utils;
using Log = Serilog.Log;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

AppData.Configuration = builder.Configuration;
builder.Configuration.UseDbConfiguration(AppData.EfOptionsAction);

RS.LoadResources();
DbSeed.Seed();

builder.Logging.ClearProviders();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .Enrich.WithUserId()
    .Enrich.WithClientIp()
    .Enrich.WithClientAgent()
    .Enrich.FromLogContext()
    // .WriteTo.PostgreSQL()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration[CK.ELASTIC_URL]))
        {
            ModifyConnectionSettings = x => x.BasicAuthentication(builder.Configuration[CK.ELASTIC_USER],
                builder.Configuration[CK.ELASTIC_PASSWORD]).ThrowExceptions(),
            IndexFormat =
                $"password-manager-server-logs-{builder.Environment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
            AutoRegisterTemplate = true,
            NumberOfReplicas = 0,
            NumberOfShards = 1,
            TypeName = null
        }
    )
#if DEBUG
    .WriteTo.Console(theme: ConsoleTheme.None)
#endif
    .CreateLogger();

builder.Host.UseSerilog();

#if DEBUG
Serilog.Debugging.SelfLog.Enable(Console.WriteLine);
#endif

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy => policy
        .WithOrigins("*")
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});

builder.Services.AddControllers(options =>
    {
        options.InputFormatters.Insert(0, JsonPatchInputFormatter.GetJsonPatchInputFormatter());
        options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
        options.Filters.Add<ExceptionFilter>();
        options.ModelMetadataDetailsProviders.Add(new ValidationMetadataProvider());
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    })
    .AddNewtonsoftJson()
    .AddDataAnnotationsLocalization();

builder.Services.AddSingleton<IValidationAttributeAdapterProvider, ValidationMetadataProvider>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var tokenValidationParameters = new TokenValidationParameters
{
    ValidAudience = builder.Configuration[CK.JWT_AUDIENCE],
    ValidIssuer = builder.Configuration[CK.JWT_ISSUER],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[CK.JWT_KEY])),
    ClockSkew = TimeSpan.Zero
};
builder.Services.AddSingleton(tokenValidationParameters);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = tokenValidationParameters);

builder.Services.AddInfrastructure();
builder.Services.AddCoreServices();

builder.Services.AddLocalization();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

var cultures = RS.GetSupportedCultures();
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(cultures.First(x => x == Culture.English.Code))
    .AddSupportedCultures(cultures)
    .AddSupportedUICultures(cultures);
localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

app.UseRequestLocalization(localizationOptions);

app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Log.Information("Server started.");

app.Run();