using Scoring.GenericScoreBoard.API;

namespace Scoring.GenericScoreBoard.InternalServices;

public interface IMatchDataStore
{
    void Add(IFootballMatch footballMatch);
    bool TryGet(string homeTeam, string awayTeam, out IFootballMatch footballMatch);
    void Remove(IFootballMatch footballMatch);
    IList<IFootballMatch> GetActive();
}