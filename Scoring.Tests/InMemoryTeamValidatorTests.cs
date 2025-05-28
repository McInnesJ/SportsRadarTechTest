using Scoring.TeamValidator.InMemory;

namespace Scoring.Tests;

[TestClass]
public class InMemoryTeamValidatorTests
{

    [TestMethod]
    public void IsValid_TeamNameIsValid_ReturnsTrue()
    {
        // Arrange
        var sut = new InMemoryTeamValidator(new List<string> { "Norway", "Sweden" });
        
        // Act & Assert
        Assert.IsTrue(sut.IsValid("Norway"));
    }

    [DataTestMethod]
    [DataRow("Scotland")]
    [DataRow("")]
    [DataRow("norway")]
    [DataRow("Norway U21")]
    public void IsValid_TeamNameIsNotValid_ReturnsFalse(string teamName)
    {
        // Arrange
        var sut = new InMemoryTeamValidator(new List<string> { "Norway", "Sweden" });

        // Act & Assert
        Assert.IsFalse(sut.IsValid(teamName));
    }
}