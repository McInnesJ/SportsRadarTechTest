using System.Diagnostics.CodeAnalysis;
using Scoring.FootballMatch;

namespace Scoring.MatchDataStore;

public interface IMatchDataStore
{
    void Add(IFootballMatch footballMatch);
    bool TryGetActiveMatch(string homeTeam, string awayTeam, [NotNullWhen(true)] out IFootballMatch? match);
    bool TryGetActiveMatchFor(string teamName, [NotNullWhen(true)] out IFootballMatch? match);
    void EndMatch(IFootballMatch footballMatch);
    IList<IFootballMatch> GetActive();
}