namespace Scoring.Scoreboard.Football;

public interface IScoreBoard
{
    /// <summary>
    /// Start a new match. All matches start at 0-0
    /// </summary>
    /// <param name="homeTeam"></param>
    /// <param name="awayTeam"></param>
    void StartMatch(string homeTeam, string awayTeam);
    
    /// <summary>
    /// End the match and remove it from the scoreboard
    /// </summary>
    /// <param name="homeTeam"></param>
    /// <param name="awayTeam"></param>
    void EndMatch(string homeTeam, string awayTeam);

    /// <summary>
    /// Get the match between the specified teams from the scoreboard
    /// </summary>
    /// <param name="homeTeam"></param>
    /// <param name="awayTeam"></param>
    /// <returns>IFootballMatch between the specified teams</returns>
    IFootballMatch GetMatch(string homeTeam, string awayTeam);
    
    /// <summary>
    /// List all matches, ordered by combined score then most recently started
    /// </summary>
    /// <returns>Sorted IList of type IFootballMatch</returns>
    IList<IFootballMatch> GetCurrentMatches();
}