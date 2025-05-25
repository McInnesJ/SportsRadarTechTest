namespace ScoreBoard.API;

public interface IScoreBoard
{
    void StartMatch(string homeTeam, string awayTeam);
    void EndMatch(string homeTeam, string awayTeam);

    IMatch GetMatch(string homeTeam, string awayTeam);
    
    IList<IMatch> GetCurrentMatches();
}