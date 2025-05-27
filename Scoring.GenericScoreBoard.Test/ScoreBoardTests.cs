using System.Data;
using Moq;
using Scoring.GenericScoreBoard.API;
using Scoring.GenericScoreBoard.Implementation;
using Scoring.GenericScoreBoard.InternalServices;
// Justification: Test file, want to create properties in the setup method
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Scoring.GenericScoreBoard.Test;

[TestClass]
public class ScoreBoardTests
{
    private const string HomeTeam = "Norway";
    private const string AwayTeam = "Sweden";
    
    private Mock<ITeamValidator> _teamValidator;
    private Mock<IMatchDataStore> _matchDataStore;
    
    private ScoreBoard _sut;

    [TestInitialize]
    public void Setup()
    {
        _teamValidator = new Mock<ITeamValidator>();
        _matchDataStore = new Mock<IMatchDataStore>();
        
        _sut = new ScoreBoard(_matchDataStore.Object, _teamValidator.Object);
    }
    
    #region StartMatch
    
    [TestMethod]
    public void StartMatch_BothTeamsValid_MatchCreated_NilNil()
    {
        // Arrange
        _teamValidator.Setup(tv => tv.IsValid(HomeTeam)).Returns(true);
        _teamValidator.Setup(tv => tv.IsValid(AwayTeam)).Returns(true);
        
        // Act
        _sut.StartMatch(HomeTeam, AwayTeam);
        
        // Assert
        _matchDataStore.Verify(ds => ds.Add(It.Is<IFootballMatch>(m => 
            m.HomeTeam == HomeTeam
            && m.AwayTeam == AwayTeam
            && m.HomeTeamScore == 0
            && m.AwayTeamScore == 0)), Times.Once);
    }

    [TestMethod]
    public void StartMatch_HomeTeamInvalid_ExceptionThrown_NoMatchCreated()
    {
        // Arrange
        _teamValidator.Setup(tv => tv.IsValid(HomeTeam)).Returns(false);
        _teamValidator.Setup(tv => tv.IsValid(AwayTeam)).Returns(true);

        // Act
        var ex = Assert.ThrowsException<ArgumentException>(() => _sut.StartMatch(HomeTeam, AwayTeam));

        // Assert
        Assert.AreEqual("'Norway' is not a valid team name", ex.Message);

        _matchDataStore.Verify(ds => ds.Add(It.Is<IFootballMatch>(m =>
            m.HomeTeam == HomeTeam
            && m.AwayTeam == AwayTeam
            && m.HomeTeamScore == 0
            && m.AwayTeamScore == 0)), Times.Never);
    }

    [TestMethod]
    public void StartMatch_BothTeamsInvalid_ExceptionThrown_NoMatchCreated()
    {
        // Arrange
        _teamValidator.Setup(tv => tv.IsValid(HomeTeam)).Returns(false);
        _teamValidator.Setup(tv => tv.IsValid(AwayTeam)).Returns(false);

        // Act
        var ex = Assert.ThrowsException<ArgumentException>(() => _sut.StartMatch(HomeTeam, AwayTeam));

        // Assert
        Assert.AreEqual("Neither team provided is valid", ex.Message);

        _matchDataStore.Verify(ds => ds.Add(It.Is<IFootballMatch>(m =>
            m.HomeTeam == HomeTeam
            && m.AwayTeam == AwayTeam
            && m.HomeTeamScore == 0
            && m.AwayTeamScore == 0)), Times.Never);
    }

    [TestMethod]
    public void StartMatch_TeamAlreadyInActiveMatch_ExceptionThrown_NoMatchCreated()
    {
        // Arrange
        _teamValidator.Setup(tv => tv.IsValid(HomeTeam)).Returns(true);
        _teamValidator.Setup(tv => tv.IsValid(AwayTeam)).Returns(true);

        // Act
        var ex = Assert.ThrowsException<ArgumentException>(() => _sut.StartMatch(HomeTeam, AwayTeam));

        // Assert
        Assert.AreEqual("Norway is already playing against Scotland", ex.Message);

        _matchDataStore.Verify(ds => ds.Add(It.Is<IFootballMatch>(m =>
            m.HomeTeam == HomeTeam
            && m.AwayTeam == AwayTeam
            && m.HomeTeamScore == 0
            && m.AwayTeamScore == 0)), Times.Never);
    }

    #endregion StartMatch
    
    #region EndMatch

    [TestMethod]
    public void EndMatch_MatchFound_RemovedFromDataStore()
    {
        // Arrange
        IFootballMatch returnedMatch = new FootballMatch(HomeTeam, AwayTeam);
        _matchDataStore.Setup(ds => ds.TryGet(HomeTeam, AwayTeam, out returnedMatch)).Returns(true);

        // Act
        _sut.EndMatch(HomeTeam, AwayTeam);

        // Assert
        _matchDataStore.Verify(ds => ds.Remove(It.Is<IFootballMatch>(m =>
            m.HomeTeam == HomeTeam
            && m.AwayTeam == AwayTeam)), Times.Once);
    }

    [TestMethod]
    public void EndMatch_MatchNotFound_ExceptionThrown()
    {
        // Arrange
        IFootballMatch returnedMatch;
        _matchDataStore.Setup(ds => ds.TryGet(HomeTeam, AwayTeam, out returnedMatch)).Returns(false);

        // Act
        var ex = Assert.ThrowsException<DataException>(() => _sut.EndMatch(HomeTeam, AwayTeam));

        // Assert
        Assert.AreEqual("Match not found", ex.Message);
        
        _matchDataStore.Verify(ds => ds.Remove(It.Is<IFootballMatch>(m =>
            m.HomeTeam == HomeTeam
            && m.AwayTeam == AwayTeam)), Times.Never);
    }

    #endregion EndMatch
    
    #region GetMatch

    [TestMethod]
    public void GetMatch_MatchFound_MatchReturned()
    {
        // Arrange
        IFootballMatch returnedMatch = new FootballMatch(HomeTeam, AwayTeam);
        _matchDataStore.Setup(ds => ds.TryGet(HomeTeam, AwayTeam, out returnedMatch)).Returns(true);

        // Act
        _sut.GetMatch(HomeTeam, AwayTeam);

        // Assert
        _matchDataStore.Verify(ds => ds.Remove(It.Is<IFootballMatch>(m =>
            m.HomeTeam == HomeTeam
            && m.AwayTeam == AwayTeam)), Times.Once);
    }

    [TestMethod]
    public void GetMatch_MatchNotFound_ExceptionThrown()
    {
        // Arrange
        IFootballMatch returnedMatch;
        _matchDataStore.Setup(ds => ds.TryGet(HomeTeam, AwayTeam, out returnedMatch)).Returns(false);

        // Act
        var ex = Assert.ThrowsException<DataException>(() => _sut.EndMatch(HomeTeam, AwayTeam));

        // Assert
        Assert.AreEqual("Match not found", ex.Message);

        _matchDataStore.Verify(ds => ds.Remove(It.Is<IFootballMatch>(m =>
            m.HomeTeam == HomeTeam
            && m.AwayTeam == AwayTeam)), Times.Never);
    }

    #endregion GetMatch

    #region GetCurrentMatches

    [TestMethod]
    public void GetCurrentMatches_NoActiveMatches_EmptyListReturned()
    {
        // Arrange
        _matchDataStore.Setup(ds => ds.GetActive()).Returns(new List<IFootballMatch>());

        // Act
        var matches = _sut.GetCurrentMatches();

        // Assert
        Assert.AreEqual(0, matches.Count);
    }

    [TestMethod]
    public void GetCurrentMatches_MatchesInCorrectOrder_OrderPreservedAndReturned()
    {
        // Arrange
        var match1 = new FootballMatch("Norway", "Scotland");
        match1.UpdateScore(3, 4);
        
        var match2 = new FootballMatch("Italy", "France");
        match2.UpdateScore(2, 2);
        
        var match3 = new FootballMatch("Denmark", "Netherlands");
        match3.UpdateScore(1, 0);

        var storedMatches = new List<IFootballMatch> { match1, match2, match3 };
        _matchDataStore.Setup(ds => ds.GetActive()).Returns(storedMatches);

        // Act
        var matches = _sut.GetCurrentMatches();

        // Assert
        CollectionAssert.AreEqual(storedMatches, matches.ToList());
    }

    [TestMethod]
    public void GetCurrentMatches_MatchesInWrongOrder_MatchesSortedAndReturned()
    {
        // Arrange
        var match1 = new FootballMatch("Italy", "France");
        match1.UpdateScore(2, 2);

        var match2 = new FootballMatch("Denmark", "Netherlands");
        match2.UpdateScore(1, 0);

        var match3 = new FootballMatch("Norway", "Scotland");
        match3.UpdateScore(3, 4);

        var storedMatches = new List<IFootballMatch> { match1, match2, match3 };
        _matchDataStore.Setup(ds => ds.GetActive()).Returns(storedMatches);

        // Act
        var matches = _sut.GetCurrentMatches();

        // Assert
        var expectedOutput = new List<IFootballMatch> { match3, match1, match2 };
        CollectionAssert.AreEqual(expectedOutput, matches.ToList());
    }

    [TestMethod]
    public void GetCurrentMatches_MatchesWithSameScore_MatchesSortedAndReturned()
    {
        // Arrange
        var match1 = new FootballMatch("Italy", "France");
        match1.UpdateScore(2, 2);

        var match2 = new FootballMatch("Denmark", "Netherlands");
        match2.UpdateScore(3, 4);

        var match3 = new FootballMatch("Norway", "Scotland");
        match3.UpdateScore(3, 4);

        var storedMatches = new List<IFootballMatch> { match1, match2, match3 };
        _matchDataStore.Setup(ds => ds.GetActive()).Returns(storedMatches);

        // Act
        var matches = _sut.GetCurrentMatches();

        // Assert
        var expectedOutput = new List<IFootballMatch> { match3, match2, match1 };
        CollectionAssert.AreEqual(expectedOutput, matches.ToList());
    }
    
    [TestMethod]
        public void GetCurrentMatches_MatchesWithSameScoreTotal_MatchesSortedAndReturned()
        {
            // Arrange
            var match1 = new FootballMatch("Italy", "France");
            match1.UpdateScore(2, 2);
    
            var match2 = new FootballMatch("Denmark", "Netherlands");
            match2.UpdateScore(6, 1);
    
            var match3 = new FootballMatch("Norway", "Scotland");
            match3.UpdateScore(3, 4);
    
            var storedMatches = new List<IFootballMatch> { match1, match2, match3 };
            _matchDataStore.Setup(ds => ds.GetActive()).Returns(storedMatches);
    
            // Act
            var matches = _sut.GetCurrentMatches();
    
            // Assert
            var expectedOutput = new List<IFootballMatch> { match3, match2, match1 };
            CollectionAssert.AreEqual(expectedOutput, matches.ToList());
        }

    #endregion GetCurrentMatches
}
