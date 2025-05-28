using Scoring.Scoreboard.Football;

namespace Scoring.MatchDataStore.InMemory;

public interface IListDataBackingProvider
{
    public IList<IFootballMatch> GetDataBacking();
}