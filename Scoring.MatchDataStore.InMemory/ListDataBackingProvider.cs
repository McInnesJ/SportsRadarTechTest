using Scoring.FootballMatch;

namespace Scoring.MatchDataStore.InMemory;

public class ListDataBackingProvider : IListDataBackingProvider
{
    public ICollection<IFootballMatch> GetDataBacking()
    {
        return new List<IFootballMatch>();
    }
}