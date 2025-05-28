using Microsoft.Extensions.DependencyInjection;
using Scoring.GenericScoreBoard;
using Scoring.Scoreboard.Football;

namespace Scoring.TeamValidator.InMemory;

public static class ScoreboardBuilderExtensions
{
    public static IScoreboardBuilder WithInMemoryTeamValidator(this IScoreboardBuilder scoreboardBuilder,
        IList<string> validTeamNames)
    {
        if (scoreboardBuilder is ScoreboardBuilder builder)
        {
            builder.Services.AddSingleton<ITeamValidator>(new InMemoryTeamValidator(validTeamNames));
        }

        return scoreboardBuilder;
    }
}