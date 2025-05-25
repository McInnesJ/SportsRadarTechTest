using ScoreBoard.API;

namespace ScoreBoard.Implementation;

public class ScoreBoard(ITeamValidator teamValidator) : IScoreBoard
{
    public void StartMatch(string homeTeam, string awayTeam)
    {
        throw new NotImplementedException();
    }

    public void EndMatch(string homeTeam, string awayTeam)
    {
        throw new NotImplementedException();
    }

    public IMatch GetMatch(string homeTeam, string awayTeam)
    {
        throw new NotImplementedException();
    }

    public IList<IMatch> GetCurrentMatches()
    {
        throw new NotImplementedException();
    }
}