using Microsoft.Extensions.DependencyInjection;

namespace Scoring.FootballMatch;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFootballMatch(this IServiceCollection services)
    {
        services.AddSingleton<IFootballMatch, BasicFootballMatch>();
        return services;
    }
}