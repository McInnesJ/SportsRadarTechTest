using Scoring.FootballMatch;

namespace Scoring.MatchDataStore;

public interface IMatchDataStore
{
    void Add(IFootballMatch footballMatch);
    bool TryGetActiveMatch(string homeTeam, string awayTeam, out IFootballMatch footballMatch);
    bool TryGetActiveMatchFor(string teamName, out IFootballMatch match);
    void EndMatch(IFootballMatch footballMatch);
    IList<IFootballMatch> GetActive();
}