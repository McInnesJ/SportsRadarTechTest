using Scoring.FootballMatch;

namespace Scoring.MatchDataStore.InMemory;

public interface IListDataBackingProvider
{
    public IList<IFootballMatch> GetDataBacking();
}