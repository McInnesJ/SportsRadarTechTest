using Microsoft.Extensions.DependencyInjection;

namespace Scoring.Scoreboard.Football.Generic;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddScoreboard(
        this IServiceCollection services, Action<IScoreboardBuilder> configureScoreboard)
    {
        services.Configure(configureScoreboard);

        services.AddScoped<IFootballMatch, BasicFootballMatch>();
        services.AddScoped<IScoreBoard, ScoreBoard>();

        var builder = new ScoreboardBuilder
        {
            Services = services
        };
        configureScoreboard(builder);
        
        return services;
    } 
}