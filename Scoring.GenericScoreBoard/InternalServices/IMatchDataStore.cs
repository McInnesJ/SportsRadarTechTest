using Scoring.GenericScoreBoard.API;

namespace Scoring.GenericScoreBoard.InternalServices;

public interface IMatchDataStore
{
    void Add(IFootballMatch footballMatch);
    bool TryGetActiveMatch(string homeTeam, string awayTeam, out IFootballMatch footballMatch);
    bool TryGetActiveMatchFor(string teamName, out IFootballMatch match);
    void EndMatch(IFootballMatch footballMatch);
    IList<IFootballMatch> GetActive();
}