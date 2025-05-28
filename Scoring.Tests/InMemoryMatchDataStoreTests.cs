using System.Data;
using Moq;
using Scoring.MatchDataStore.InMemory;
using Scoring.Scoreboard.Football;
using Scoring.Scoreboard.Football.Generic;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Scoring.Tests;

[TestClass]
public class InMemoryMatchDataStoreTests
{
    private const string HomeTeam = "Norway";
    private const string AwayTeam = "Sweden";
    
    private Mock<IListDataBackingProvider> _dataBackingProvider;
    private IList<IFootballMatch> _activeGames;
    private IList<IFootballMatch> _inactiveGames;
    
    private InMemoryMatchDataStore _sut;
    
    [TestInitialize]
    public void Setup()
    {
        _activeGames = new List<IFootballMatch>();
        _inactiveGames = new List<IFootballMatch>();
        
        _dataBackingProvider = new Mock<IListDataBackingProvider>();
        _dataBackingProvider.SetupSequence(p => p.GetDataBacking())
            .Returns(_activeGames)
            .Returns(_inactiveGames);
        
        _sut = new InMemoryMatchDataStore(_dataBackingProvider.Object);
    }

    #region Add

    [TestMethod]
    public void Add_MatchAddedToActiveGames()
    {
        // Arrange
        var match = new BasicFootballMatch(HomeTeam, AwayTeam);
        
        // Act
        _sut.Add(match);
        
        // Assert
        Assert.AreEqual(1, _activeGames.Count);
        Assert.AreEqual(match, _activeGames[0]);
    }

    #endregion Add

    #region TryGetActiveMatch
    
    [TestMethod]
    public void TryGetActiveMatch_MatchDoesNotExistInActive_ReturnFalse()
    {
        // Act
        var result = _sut.TryGetActiveMatch(HomeTeam, AwayTeam, out var match);

        // Assert
        Assert.IsFalse(result);
        Assert.IsNull(match);
    }

    [TestMethod]
    public void TryGetActiveMatch_MatchExistInInactive_ReturnsFalse()
    {
        // Arrange
        var match = new BasicFootballMatch(HomeTeam, AwayTeam);
        _inactiveGames.Add(match);
        
        // Act
        var result = _sut.TryGetActiveMatch(HomeTeam, AwayTeam, out var returnedMatch);

        // Assert
        Assert.IsFalse(result);
        Assert.IsNull(returnedMatch);
    }

    [TestMethod]
    public void TryGetActiveMatch_MatchExistsInActive_ReturnsTrue()
    {
        // Arrange
        var match = new BasicFootballMatch(HomeTeam, AwayTeam);
        _activeGames.Add(match);

        // Act
        var result = _sut.TryGetActiveMatch(HomeTeam, AwayTeam, out var returnedMatch);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(match.HomeTeam, returnedMatch!.HomeTeam);
        Assert.AreEqual(match.AwayTeam, returnedMatch.AwayTeam);
    }

    [TestMethod]
    public void TryGetActiveMatch_MatchExistsInActiveAndInactive_ReturnsTrue()
    {
        // Arrange
        var match = new BasicFootballMatch(HomeTeam, AwayTeam);
        _activeGames.Add(match);
        _inactiveGames.Add(match);

        // Act
        var result = _sut.TryGetActiveMatch(HomeTeam, AwayTeam, out var returnedMatch);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(match.HomeTeam, returnedMatch!.HomeTeam);
        Assert.AreEqual(match.AwayTeam, returnedMatch.AwayTeam);
    }

    #endregion TryGetActiveMatch
 
    #region TryGetActiveMatchFor

    [TestMethod]
    public void TryGetActiveMatchFor_MatchDoesNotExistInActive_ReturnFalse()
    {
        // Arrange
        _activeGames.Add(new BasicFootballMatch(HomeTeam, AwayTeam));
        
        // Act
        var result = _sut.TryGetActiveMatchFor("Scotland", out var match);

        // Assert
        Assert.IsFalse(result);
        Assert.IsNull(match);
    }

    [TestMethod]
    public void TryGetActiveMatchFor_MatchExistInInactive_ReturnsFalse()
    {
        // Arrange
        _activeGames.Add(new BasicFootballMatch("Scotland", "France"));
        _inactiveGames.Add(new BasicFootballMatch(HomeTeam, AwayTeam));

        // Act
        var result = _sut.TryGetActiveMatch(HomeTeam, AwayTeam, out var returnedMatch);

        // Assert
        Assert.IsFalse(result);
        Assert.IsNull(returnedMatch);
    }

    [TestMethod]
    public void TryGetActiveMatchFor_HomeTeamHasActiveMatch_ReturnsTrue()
    {
        // Arrange
        var match = new BasicFootballMatch(HomeTeam, AwayTeam);
        _activeGames.Add(match);

        // Act
        var result = _sut.TryGetActiveMatchFor(HomeTeam, out var returnedMatch);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(match.HomeTeam, returnedMatch!.HomeTeam);
        Assert.AreEqual(match.AwayTeam, returnedMatch.AwayTeam);
    }

    [TestMethod]
    public void TryGetActiveMatchFor_AwayTeamHasActiveMatch_ReturnsTrue()
    {
        // Arrange
        var match = new BasicFootballMatch(HomeTeam, AwayTeam);
        _activeGames.Add(match);

        // Act
        var result = _sut.TryGetActiveMatchFor(AwayTeam, out var returnedMatch);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(match.HomeTeam, returnedMatch!.HomeTeam);
        Assert.AreEqual(match.AwayTeam, returnedMatch.AwayTeam);
    }

    [TestMethod]
    public void TryGetActiveMatchFor_HomeTeamHasActiveAndInactiveMatch_ReturnsTrue()
    {
        // Arrange
        var match = new BasicFootballMatch(HomeTeam, AwayTeam);
        _activeGames.Add(match);
        
        _inactiveGames.Add(new BasicFootballMatch(HomeTeam, "France"));

        // Act
        var result = _sut.TryGetActiveMatchFor(HomeTeam, out var returnedMatch);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(match.HomeTeam, returnedMatch!.HomeTeam);
        Assert.AreEqual(match.AwayTeam, returnedMatch.AwayTeam);
    }

    [TestMethod]
    public void TryGetActiveMatchFor_AwayTeamHasActiveAndInactiveMatch_ReturnsTrue()
    {
        // Arrange
        var match = new BasicFootballMatch(HomeTeam, AwayTeam);
        _activeGames.Add(match);

        _inactiveGames.Add(new BasicFootballMatch("Scotland", AwayTeam));

        // Act
        var result = _sut.TryGetActiveMatchFor(AwayTeam, out var returnedMatch);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(match.HomeTeam, returnedMatch!.HomeTeam);
        Assert.AreEqual(match.AwayTeam, returnedMatch.AwayTeam);
    }

    #endregion TryGetActiveMatchFor

    #region EndMatch

    [TestMethod]
    public void EndMatch_ActiveMatchExists_MovedToInactive()
    {
        // Arrange
        var match = new BasicFootballMatch(HomeTeam, AwayTeam);
        _activeGames.Add(match);

        // Act
        _sut.EndMatch(match);

        // Assert
        Assert.AreEqual(0, _activeGames.Count);
        Assert.AreEqual(1, _inactiveGames.Count);
        Assert.AreEqual(match, _inactiveGames[0]);
    }

    [TestMethod]
    public void EndMatch_ActiveAndInactiveMatchExists_MovedToInactive()
    {
        // Arrange
        var match = new BasicFootballMatch(HomeTeam, AwayTeam);
        _activeGames.Add(match);
        _inactiveGames.Add(match);

        // Act
        _sut.EndMatch(match);

        // Assert
        Assert.AreEqual(0, _activeGames.Count);
        Assert.AreEqual(2, _inactiveGames.Count);
        Assert.AreEqual(match, _inactiveGames[0]);
        Assert.AreEqual(match, _inactiveGames[1]);
    }

    [TestMethod]
    public void EndMatch_ActiveMatchDoesNotExists_ThrowsException()
    {
        // Arrange
        var match = new BasicFootballMatch(HomeTeam, AwayTeam);
        _inactiveGames.Add(match);

        // Act
        var ex = Assert.ThrowsException<DataException>(() => _sut.EndMatch(match));

        // Assert
        Assert.AreEqual("No active match found", ex.Message);
        Assert.AreEqual(0, _activeGames.Count);
        Assert.AreEqual(1, _inactiveGames.Count);
    }

    #endregion EndMatch
    
    #region GetActive

    [TestMethod]
    public void GetActive_ActiveMatchesOnlyReturned()
    {
        // Arrange
        var inactiveMatch = new BasicFootballMatch(HomeTeam, AwayTeam);
        _inactiveGames.Add(inactiveMatch);
        
        var activeMatch1 = new BasicFootballMatch("Scotland", "France");
        var activeMatch2 = new BasicFootballMatch("Italy", "Spain");
        _activeGames.Add(activeMatch1);
        _activeGames.Add(activeMatch2);

        // Act
        var activeMatchesReturned = _sut.GetActive();

        // Assert
        CollectionAssert.AreEquivalent(new[] { activeMatch1, activeMatch2 }, activeMatchesReturned.ToArray());
    }

    #endregion GetActive
}