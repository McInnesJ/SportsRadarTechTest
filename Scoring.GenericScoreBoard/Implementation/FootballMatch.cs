using Scoring.GenericScoreBoard.API;

namespace Scoring.GenericScoreBoard.Implementation;

public class FootballMatch(string homeTeam, string awayTeam) : IFootballMatch
{
    public string HomeTeam { get; } = homeTeam;
    public string AwayTeam { get; } = awayTeam;
    public int HomeTeamScore { get; }
    public int AwayTeamScore { get; }

    public void UpdateScore(int homeScore, int awayScore)
    {
        throw new NotImplementedException();
    }
}