namespace ScoreBoard.API;

public interface IMatch
{
    void UpdateScore(int homeScore, int awayScore);
}