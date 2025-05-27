namespace Scoring.GenericScoreBoard.API;

public interface IFootballMatch
{
    string HomeTeam { get; }
    string AwayTeam { get; }
    int HomeTeamScore { get; }
    int AwayTeamScore { get; }

    void UpdateScore(int homeScore, int awayScore);
}