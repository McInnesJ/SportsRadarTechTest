using System.Data;
using Scoring.GenericScoreBoard;
using Scoring.MatchDataStore;

namespace Scoring.Scoreboard.Football.Generic;

public class ScoreBoard(
    IMatchDataStore matchDataStore,
    ITeamValidator teamValidator) : IScoreBoard
{
    public void StartMatch(string homeTeam, string awayTeam)
    {
        if (matchDataStore.TryGetActiveMatchFor(homeTeam, out var homeTeamMatch))
        {
            throw new ArgumentException($"{homeTeam} is already playing against {homeTeamMatch.AwayTeam}");
        }

        if (matchDataStore.TryGetActiveMatchFor(awayTeam, out var awayTeamMatch))
        {
            throw new ArgumentException($"{awayTeam} is already playing against {awayTeamMatch.HomeTeam}");
        }

        var homeTeamIsValid = teamValidator.IsValid(homeTeam);
        var awayTeamIsValid = teamValidator.IsValid(awayTeam);

        if (!homeTeamIsValid && !awayTeamIsValid)
        {
            throw new ArgumentException("Neither team name provided is valid");
        }
        
        if (!teamValidator.IsValid(homeTeam))
        {
            throw new ArgumentException($"'{homeTeam}' is not a valid team name");
        }

        if (!teamValidator.IsValid(awayTeam))
        {
            throw new ArgumentException($"'{awayTeam}' is not a valid team name");
        }

        matchDataStore.Add(new BasicFootballMatch(homeTeam, awayTeam));
    }

    public void EndMatch(string homeTeam, string awayTeam)
    {
        if (!matchDataStore.TryGetActiveMatch(homeTeam, awayTeam, out var match))
        {
            throw new DataException($"No match found between {homeTeam} and {awayTeam}");
        }

        matchDataStore.EndMatch(match);
    }

    public IFootballMatch GetMatch(string homeTeam, string awayTeam)
    {
        return matchDataStore.TryGetActiveMatch(homeTeam, awayTeam, out var match)
            ? match
            : throw new DataException($"No match found between {homeTeam} and {awayTeam}");
    }

    public IList<IFootballMatch> GetCurrentMatches()
    {
        return matchDataStore.GetActive()
            .Select((match, index) => new { Match = match, Index = index })
            .OrderByDescending(sum => sum.Match.HomeTeamScore + sum.Match.AwayTeamScore)
            .ThenByDescending(i => i.Index)
            .Select(match => match.Match)
            .ToList();
    }
}