using System.Data;
using Moq;
using Scoring.GenericScoreBoard;
using Scoring.MatchDataStore;
using Scoring.Scoreboard.Football;
using Scoring.Scoreboard.Football.Generic;

// Justification: Test file, want to create properties in the setup method
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Scoring.Tests;

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
    public void StartMatch_AwayTeamInvalid_ExceptionThrown_NoMatchCreated()
    {
        // Arrange
        _teamValidator.Setup(tv => tv.IsValid(HomeTeam)).Returns(true);
        _teamValidator.Setup(tv => tv.IsValid(AwayTeam)).Returns(false);

        // Act
        var ex = Assert.ThrowsException<ArgumentException>(() => _sut.StartMatch(HomeTeam, AwayTeam));

        // Assert
        Assert.AreEqual("'Sweden' is not a valid team name", ex.Message);

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
        Assert.AreEqual("Neither team name provided is valid", ex.Message);

        _matchDataStore.Verify(ds => ds.Add(It.Is<IFootballMatch>(m =>
            m.HomeTeam == HomeTeam
            && m.AwayTeam == AwayTeam
            && m.HomeTeamScore == 0
            && m.AwayTeamScore == 0)), Times.Never);
    }

    [TestMethod]
    public void StartMatch_HomeTeamAlreadyInActiveMatch_ExceptionThrown_NoMatchCreated()
    {
        // Arrange
        _teamValidator.Setup(tv => tv.IsValid(HomeTeam)).Returns(true);
        _teamValidator.Setup(tv => tv.IsValid(AwayTeam)).Returns(true);
        
        IFootballMatch? homeTeamMatch = new BasicFootballMatch(HomeTeam, "Scotland");
        _matchDataStore.Setup(ds => ds.TryGetActiveMatchFor(HomeTeam, out homeTeamMatch)).Returns(true);
        IFootballMatch? awayTeamMatch;
        _matchDataStore.Setup(ds => ds.TryGetActiveMatchFor(AwayTeam, out awayTeamMatch)).Returns(false);

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

    [TestMethod]
    public void StartMatch_AwayTeamAlreadyInActiveMatch_ExceptionThrown_NoMatchCreated()
    {
        // Arrange
        _teamValidator.Setup(tv => tv.IsValid(HomeTeam)).Returns(true);
        _teamValidator.Setup(tv => tv.IsValid(AwayTeam)).Returns(true);

        IFootballMatch? homeTeamMatch;
        _matchDataStore.Setup(ds => ds.TryGetActiveMatchFor(HomeTeam, out homeTeamMatch)).Returns(false);
        IFootballMatch? awayTeamMatch = new BasicFootballMatch("Scotland", AwayTeam);
        _matchDataStore.Setup(ds => ds.TryGetActiveMatchFor(AwayTeam, out awayTeamMatch)).Returns(true);

        // Act
        var ex = Assert.ThrowsException<ArgumentException>(() => _sut.StartMatch(HomeTeam, AwayTeam));

        // Assert
        Assert.AreEqual("Sweden is already playing against Scotland", ex.Message);

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
        IFootballMatch? returnedMatch = new BasicFootballMatch(HomeTeam, AwayTeam);
        _matchDataStore.Setup(ds => ds.TryGetActiveMatch(HomeTeam, AwayTeam, out returnedMatch)).Returns(true);

        // Act
        _sut.EndMatch(HomeTeam, AwayTeam);

        // Assert
        _matchDataStore.Verify(ds => ds.EndMatch(It.Is<IFootballMatch>(m =>
            m.HomeTeam == HomeTeam
            && m.AwayTeam == AwayTeam)), Times.Once);
    }

    [TestMethod]
    public void EndMatch_MatchNotFound_ExceptionThrown()
    {
        // Arrange
        IFootballMatch? returnedMatch;
        _matchDataStore.Setup(ds => ds.TryGetActiveMatch(HomeTeam, AwayTeam, out returnedMatch)).Returns(false);

        // Act
        var ex = Assert.ThrowsException<DataException>(() => _sut.EndMatch(HomeTeam, AwayTeam));

        // Assert
        Assert.AreEqual("No match found between Norway and Sweden", ex.Message);
        
        _matchDataStore.Verify(ds => ds.EndMatch(It.Is<IFootballMatch>(m =>
            m.HomeTeam == HomeTeam
            && m.AwayTeam == AwayTeam)), Times.Never);
    }

    #endregion EndMatch
    
    #region GetMatch

    [TestMethod]
    public void GetMatch_MatchFound_MatchReturned()
    {
        // Arrange
        IFootballMatch? expectedMatch = new BasicFootballMatch(HomeTeam, AwayTeam);
        _matchDataStore.Setup(ds => ds.TryGetActiveMatch(HomeTeam, AwayTeam, out expectedMatch)).Returns(true);

        // Act
        var actualMatch = _sut.GetMatch(HomeTeam, AwayTeam);

        // Assert
        Assert.AreEqual(HomeTeam, actualMatch.HomeTeam);
        Assert.AreEqual(AwayTeam, actualMatch.AwayTeam);
    }

    [TestMethod]
    public void GetMatch_MatchNotFound_ExceptionThrown()
    {
        // Arrange
        IFootballMatch? returnedMatch;
        _matchDataStore.Setup(ds => ds.TryGetActiveMatch(HomeTeam, AwayTeam, out returnedMatch)).Returns(false);

        // Act
        var ex = Assert.ThrowsException<DataException>(() => _sut.EndMatch(HomeTeam, AwayTeam));

        // Assert
        Assert.AreEqual("No match found between Norway and Sweden", ex.Message);
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
        var match1 = CreateMatch("Norway", "Scotland", 3, 4);
        var match2 = CreateMatch("Italy", "France", 2, 2);
        var match3 = CreateMatch("Denmark", "Netherlands", 1, 0);

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
        var match1 = CreateMatch("Italy", "France", 2, 2);
        var match2 = CreateMatch("Denmark", "Netherlands", 1, 0);
        var match3 = CreateMatch("Norway", "Scotland", 3, 4);

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
        var match1 = CreateMatch("Italy", "France", 2, 2);
        var match2 = CreateMatch("Denmark", "Netherlands", 3, 4);
        var match3 = CreateMatch("Norway", "Scotland", 3, 4);

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
            var match1 = CreateMatch("Italy", "France", 2, 2);
            var match2 = CreateMatch("Denmark", "Netherlands", 6, 1);
            var match3 = CreateMatch("Norway", "Scotland", 3, 4);
    
            var storedMatches = new List<IFootballMatch> { match1, match2, match3 };
            _matchDataStore.Setup(ds => ds.GetActive()).Returns(storedMatches);
    
            // Act
            var matches = _sut.GetCurrentMatches();
    
            // Assert
            var expectedOutput = new List<IFootballMatch> { match3, match2, match1 };
            CollectionAssert.AreEqual(expectedOutput, matches.ToList());
        }

    #endregion GetCurrentMatches

    private IFootballMatch CreateMatch(string homeTeam, string awayTeam, int homeScore, int awayScore)
    {
        var match = new Mock<IFootballMatch>();
        match.SetupGet(m => m.HomeTeam).Returns(homeTeam);
        match.SetupGet(m => m.AwayTeam).Returns(awayTeam);
        match.SetupGet(m => m.HomeTeamScore).Returns(homeScore);
        match.SetupGet(m => m.AwayTeamScore).Returns(awayScore);
        
        return match.Object;
    }
}
