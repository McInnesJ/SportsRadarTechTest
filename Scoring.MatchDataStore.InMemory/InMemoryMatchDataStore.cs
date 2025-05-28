using System.Data;
using System.Diagnostics.CodeAnalysis;
using Scoring.Scoreboard.Football;

namespace Scoring.MatchDataStore.InMemory;

public class InMemoryMatchDataStore(IListDataBackingProvider dataBackingProvider) : IMatchDataStore
{
    private readonly IList<IFootballMatch> _activeMatches = dataBackingProvider.GetDataBacking();
    private readonly IList<IFootballMatch> _inactiveMatches = dataBackingProvider.GetDataBacking();
    
    public void Add(IFootballMatch footballMatch)
    {
        _activeMatches.Add(footballMatch);
    }

    public bool TryGetActiveMatch(string homeTeam, string awayTeam, [NotNullWhen(true)] out IFootballMatch? match)
    {
        match = _activeMatches.FirstOrDefault(m => m.HomeTeam == homeTeam && m.AwayTeam == awayTeam);
        return match is not null;
    }

    public bool TryGetActiveMatchFor(string teamName, [NotNullWhen(true)] out IFootballMatch? match)
    {
        match = _activeMatches.FirstOrDefault(m => m.HomeTeam == teamName);
        if (match is not null)
        {
            return true;
        }
        
        match = _activeMatches.FirstOrDefault(m => m.AwayTeam == teamName);
        return match is not null;
    }

    public void EndMatch(IFootballMatch footballMatch)
    {
        var matchWasRemoved = _activeMatches.Remove(footballMatch);
        if (!matchWasRemoved)
        {
            throw new DataException("No active match found");
        }
        
        _inactiveMatches.Add(footballMatch);
    }

    public IList<IFootballMatch> GetActive()
    {
        return new List<IFootballMatch>(_activeMatches);
    }
}