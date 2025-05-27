using Microsoft.Extensions.DependencyInjection;
using Scoring.FootballMatch;
using Scoring.GenericScoreBoard.Implementation;

namespace Scoring.GenericScoreBoard.Injection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddScoreboard(
        this IServiceCollection services, Action<ScoreboardConfigurationOptions> configureScoreboard)
    {
        // TODO: Consider how to ensure necessary config options are provided
        
        services.Configure(configureScoreboard);

        services.AddFootballMatch();
        services.AddScoped<IScoreBoard, ScoreBoard>();
        
        return services;
    } 
}