using Scoring.GenericScoreBoard.Implementation;

namespace Scoring.GenericScoreBoard.Test;

[TestClass]
public class FootballMatchTests
{
    private const string HomeTeam = "Norway";
    private const string AwayTeam = "Scotland";
    
    [TestMethod]
    public void FootballMatch_InitialState()
    {
        // Arrange & Act
        var match = new FootballMatch(HomeTeam, AwayTeam);
        
        // Assert
        Assert.AreEqual(HomeTeam, match.HomeTeam);
        Assert.AreEqual(AwayTeam, match.AwayTeam);
        Assert.AreEqual(0, match.HomeTeamScore);
        Assert.AreEqual(0, match.AwayTeamScore);
    }

    [DataTestMethod]
    [DataRow(1, 0)]
    [DataRow(0, 1)]
    public void UpdateScore_ScoreSet(int homeScore, int awayScore)
    {
        // Arrange
        var match = new FootballMatch(HomeTeam, AwayTeam);
        
        // Act
        match.UpdateScore(1,0);

        // Assert
        Assert.AreEqual(homeScore, match.HomeTeamScore);
        Assert.AreEqual(awayScore, match.AwayTeamScore);
    }

    [TestMethod]
    public void UpdateScore_ByMoreThanOneGoal_ExceptionThrown()
    {
        // Not a requirement listed, but seems sensible given only one goal can ever be scored at a time and the cadence of a football match is such that even with human updates,
        // it would be trivial to do a single update per goal. I would probably change the interface to reflect this (i.e. methods along the lines of 'HomeTeamScores() AwayTeamScores()
        // but the requirements specified this method signature.
        
        // Arrange
        var match = new FootballMatch(HomeTeam, AwayTeam);
        match.UpdateScore(1, 0);

        // Act
        var ex = Assert.ThrowsException<ArgumentException>(() => match.UpdateScore(1, 2));

        // Assert
        Assert.AreEqual("Score can only be incremented one team at a time.", ex.Message);
    }

    [TestMethod]
    public void UpdateScore_OneGoalPerTeam_ExceptionThrown()
    {
        // Arrange
        var match = new FootballMatch(HomeTeam, AwayTeam);

        // Act
        var ex = Assert.ThrowsException<ArgumentException>(() => match.UpdateScore(1, 1));

        // Assert
        Assert.AreEqual("Score can only be incremented one team at a time", ex.Message);
    }

    [TestMethod]
    public void UpdateScore_ScoreDecreased_ExceptionThrown()
    {
        // This is an interesting case as there is a scenario where a goal might be awarded and subsequently revoked.
        // For this use case I would have a dedicated endpoint to reduce erroneous updates. 
        // In a Web API world, this endpoint could even have different permissions required such that only a manager could reset an incorrect score
        
        // Arrange
        var match = new FootballMatch(HomeTeam, AwayTeam);
        match.UpdateScore(1, 0);
        match.UpdateScore(1, 1);

        // Act
        var ex = Assert.ThrowsException<ArgumentException>(() => match.UpdateScore(1, 0));

        // Assert
        Assert.AreEqual("Score can only be incremented one team at a time", ex.Message);
    }
}