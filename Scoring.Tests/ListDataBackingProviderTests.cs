using Scoring.FootballMatch;
using Scoring.MatchDataStore.InMemory;

namespace Scoring.Tests;

[TestClass]
public class ListDataBackingProviderTests
{
    [TestMethod]
    public void GetDataBackingProvider_ReturnsNewList()
    {
        // Arrange
        var sut = new ListDataBackingProvider();
        
        // Act
        var dataBacking = sut.GetDataBacking();
        
        // Assert
        Assert.IsNotNull(dataBacking);
        Assert.IsInstanceOfType(dataBacking, typeof(List<IFootballMatch>));
        Assert.AreEqual(0, dataBacking.Count);
    }
}