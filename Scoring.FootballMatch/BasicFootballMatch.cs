namespace Scoring.FootballMatch;

public class BasicFootballMatch(string homeTeam, string awayTeam) : IFootballMatch
{
    public string HomeTeam { get; } = homeTeam;
    public string AwayTeam { get; } = awayTeam;
    public int HomeTeamScore { get; private set; }
    public int AwayTeamScore { get; private set; }

    public void UpdateScore(int homeScore, int awayScore)
    {
        if (homeScore == HomeTeamScore + 1 && awayScore == AwayTeamScore)
        {
            HomeTeamScore = homeScore;
            return;
        }

        if (awayScore == AwayTeamScore + 1 && homeScore == HomeTeamScore)
        {
            AwayTeamScore = awayScore;
            return;
        }

        if (homeScore == HomeTeamScore && awayScore == AwayTeamScore)
        {
            throw new ArgumentException("No update required. Score already set to requested values");
        }

        throw new ArgumentException("Score can only be incremented one team at a time");
    }
}