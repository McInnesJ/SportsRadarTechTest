using Scoring.Scoreboard.Football;

namespace Scoring.MatchDataStore.InMemory;

public class ListDataBackingProvider : IListDataBackingProvider
{
    public IList<IFootballMatch> GetDataBacking()
    {
        return new List<IFootballMatch>();
    }
}