using Microsoft.Extensions.DependencyInjection;
using Scoring.Scoreboard.Football;

namespace Scoring.MatchDataStore.InMemory;

public static class ScoreboardBuilderExtensions
{
    public static IScoreboardBuilder WithInMemoryDataStore(this IScoreboardBuilder scoreboardBuilder)
    {
        if (scoreboardBuilder is ScoreboardBuilder builder)
        {
            builder.Services.AddScoped<IListDataBackingProvider, ListDataBackingProvider>();
            builder.Services.AddScoped<IMatchDataStore, InMemoryMatchDataStore>();
        }
        
        return scoreboardBuilder;
    }
}