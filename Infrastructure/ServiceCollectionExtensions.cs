using Database;
using Infrastructure.DataAccess;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .Configure<EntityContextOptions>(configuration.GetSection("DbSection"))
            .AddScoped<IJournalDataService, JournalDataService>()
            .AddScoped<IJournalService, JournalService>()
            .AddScoped<ITreeDataService, TreeDataService>()
            .AddScoped<ITreeService, TreeService>()
            .AddTransient<ILogService, LogService>()
            .AddTransient<ILogDataService, LogDataService>();
    }
}