using System.Text;
using Core;
using Infrastructure;
using Infrastructure.Db;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Utilities;
using WebApp.Filters;
using WebApp.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEfConfiguration(options =>
    options.UseSqlServer(builder.Configuration[ConfigurationKeys.PASSWORD_MANAGER_DB]));

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
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
    options.Filters.Add<ExceptionFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var tokenValidationParameters = new TokenValidationParameters
{
    ValidAudience = builder.Configuration[ConfigurationKeys.JWT_AUDIENCE],
    ValidIssuer = builder.Configuration[ConfigurationKeys.JWT_ISSUER],
    IssuerSigningKey =
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[ConfigurationKeys.JWT_KEY])),
    ClockSkew = TimeSpan.Zero
};
builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = tokenValidationParameters);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCoreServices();


var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();