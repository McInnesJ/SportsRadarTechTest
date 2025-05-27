using Scoring.FootballMatch;

namespace Scoring.MatchDataStore.InMemory;

public class InMemoryMatchDataStore(IListDataBackingProvider dataBackingProvider) : IMatchDataStore
{
    public void Add(IFootballMatch footballMatch)
    {
        throw new NotImplementedException();
    }

    public bool TryGetActiveMatch(string homeTeam, string awayTeam, out IFootballMatch footballMatch)
    {
        throw new NotImplementedException();
    }

    public bool TryGetActiveMatchFor(string teamName, out IFootballMatch match)
    {
        throw new NotImplementedException();
    }

    public void EndMatch(IFootballMatch footballMatch)
    {
        throw new NotImplementedException();
    }

    public IList<IFootballMatch> GetActive()
    {
        throw new NotImplementedException();
    }
}