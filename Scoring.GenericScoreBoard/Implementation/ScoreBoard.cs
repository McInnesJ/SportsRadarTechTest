using Scoring.GenericScoreBoard.API;
using Scoring.GenericScoreBoard.InternalServices;

namespace Scoring.GenericScoreBoard.Implementation;

public class ScoreBoard(
    IMatchDataStore matchDataStore,
    ITeamValidator teamValidator) : IScoreBoard
{
    public void StartMatch(string homeTeam, string awayTeam)
    {
        throw new NotImplementedException();
    }

    public void EndMatch(string homeTeam, string awayTeam)
    {
        throw new NotImplementedException();
    }

    public IFootballMatch GetMatch(string homeTeam, string awayTeam)
    {
        throw new NotImplementedException();
    }

    public IList<IFootballMatch> GetCurrentMatches()
    {
        throw new NotImplementedException();
    }
}