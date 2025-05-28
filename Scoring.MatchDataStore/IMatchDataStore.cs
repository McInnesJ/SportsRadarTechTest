using System.Diagnostics.CodeAnalysis;
using Scoring.Scoreboard.Football;

namespace Scoring.MatchDataStore;

public interface IMatchDataStore
{
    /// <summary>
    /// Add a new match to the data store.
    /// </summary>
    /// <param name="footballMatch"></param>
    void Add(IFootballMatch footballMatch);
    
    /// <summary>
    /// Get the active match between the corresponding teams.
    /// </summary>
    /// <param name="homeTeam"></param>
    /// <param name="awayTeam"></param>
    /// <param name="match"></param>
    /// <returns>True if the match is found, false otherwise</returns>
    bool TryGetActiveMatch(string homeTeam, string awayTeam, [NotNullWhen(true)] out IFootballMatch? match);
    
    /// <summary>
    /// Get the active match that the specified team is involved in.
    /// </summary>
    /// <param name="teamName"></param>
    /// <param name="match"></param>
    /// <returns>True if the team is involved in an active match, false otherwise</returns>
    bool TryGetActiveMatchFor(string teamName, [NotNullWhen(true)] out IFootballMatch? match);
    
    /// <summary>
    /// End the match, marking it as no longer active.
    /// </summary>
    /// <param name="footballMatch"></param>
    void EndMatch(IFootballMatch footballMatch);
    
    /// <returns>A list of all active matches</returns>
    IList<IFootballMatch> GetActive();
}