using Scoring.GenericScoreBoard;

namespace Scoring.TeamValidator.InMemory;

public class InMemoryTeamValidator(IList<string> validTeamNames) : ITeamValidator
{
    public bool IsValid(string teamName)
    {
        return validTeamNames.Contains(teamName);
    }
}