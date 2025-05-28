namespace Scoring.Scoreboard.Football;

public interface IFootballMatch
{
    string HomeTeam { get; }
    string AwayTeam { get; }
    int HomeTeamScore { get; }
    int AwayTeamScore { get; }

    /// <summary>
    /// Update the score. Score updates must be incremental and one team at a time.
    /// </summary>
    /// <param name="homeScore"></param>
    /// <param name="awayScore"></param>
    void UpdateScore(int homeScore, int awayScore);
}