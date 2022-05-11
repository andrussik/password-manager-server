using Core.Interfaces;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Utilities;

namespace Infrastructure;

public static class ServiceInstaller
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<IUnitOfWork, AppDbContext>(AppData.EfOptionsAction);

        return services;
    }
}