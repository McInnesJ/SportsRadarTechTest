namespace Scoring.GenericScoreBoard;

public interface ITeamValidator
{
    /// <summary>
    /// Verify the team name specified is valid (for example, involved in the current competition)
    /// </summary>
    /// <param name="teamName"></param>
    /// <returns>True if valid, false otherwise</returns>
    bool IsValid(string teamName);
}