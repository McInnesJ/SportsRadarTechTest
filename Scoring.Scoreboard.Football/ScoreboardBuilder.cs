using Microsoft.Extensions.DependencyInjection;

namespace Scoring.Scoreboard.Football;

public class ScoreboardBuilder : IScoreboardBuilder
{
    public required IServiceCollection Services { get; init; }
}