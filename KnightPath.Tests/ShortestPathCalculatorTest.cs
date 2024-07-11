namespace KnightPath.Tests;

[TestFixture]
public class ShortestPathCalculatorTest
{
    [Test]
    public void OnBoardValid()
    {
        Assert.That(ShortestPathCalculator.OnBoard(0, 0));
    }

    [Test]
    public void OnBoardInvalidNegativeX()
    {
        Assert.That(ShortestPathCalculator.OnBoard(-1, 0), Is.False);
    }

    [Test]
    public void OnBoardInvalidNegativeY()
    {
        Assert.That(ShortestPathCalculator.OnBoard(0, -1), Is.False);
    }

    [Test]
    public void OnBoardInvalidOutOfBoundsX()
    {
        Assert.That(ShortestPathCalculator.OnBoard(8, 0), Is.False);
    }

    [Test]
    public void OnBoardInvalidOutOfBoundsY()
    {
        Assert.That(ShortestPathCalculator.OnBoard(0, 8), Is.False);
    }

    [Test]
    public void CalculateOutOfBounds()
    {
        var actual = ShortestPathCalculator.CalculateShortestPath("A2", "A9");
        var expected = new List<string>();
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CalculateNoMove()
    {
        var actual = ShortestPathCalculator.CalculateShortestPath("A2", "A2");
        var expected = new List<string> { "A2" };
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CalculateOneMove()
    {
        var actual = ShortestPathCalculator.CalculateShortestPath("A2", "B4");
        var expected = new List<string> { "A2", "B4" };
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CalculateTwoMoves()
    {
        var actual = ShortestPathCalculator.CalculateShortestPath("G7", "C5");
        var expected = new List<string> { "G7", "E6", "C5" };
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CalculateThreeMoves()
    {
        var actual = ShortestPathCalculator.CalculateShortestPath("A1", "D5");
        var expected = new List<string> { "A1", "C2", "B4", "D5" };
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CalculateFourMoves()
    {
        var actual = ShortestPathCalculator.CalculateShortestPath("D4", "B2");
        var expected = new List<string> { "D4", "B3", "C1", "D3", "B2" };
        Assert.That(actual, Is.EqualTo(expected));
    }
}