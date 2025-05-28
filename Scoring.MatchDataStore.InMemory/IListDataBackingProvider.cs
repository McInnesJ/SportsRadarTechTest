using Scoring.Scoreboard.Football;

namespace Scoring.MatchDataStore.InMemory;

public interface IListDataBackingProvider
{
    /// <summary>
    /// Get a list based data backing for IFootballMatchs
    /// </summary>
    public IList<IFootballMatch> GetDataBacking();
}