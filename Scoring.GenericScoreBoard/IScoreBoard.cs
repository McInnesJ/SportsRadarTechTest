using Scoring.FootballMatch;

namespace Scoring.GenericScoreBoard;

public interface IScoreBoard
{
    void StartMatch(string homeTeam, string awayTeam);
    void EndMatch(string homeTeam, string awayTeam);

    IFootballMatch GetMatch(string homeTeam, string awayTeam);
    
    IList<IFootballMatch> GetCurrentMatches();
}