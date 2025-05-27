using Scoring.FootballMatch;

namespace Scoring.MatchDataStore.InMemory;

public interface IListDataBackingProvider
{
    public ICollection<IFootballMatch> GetDataBacking();
}