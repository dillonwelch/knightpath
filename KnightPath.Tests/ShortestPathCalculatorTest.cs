namespace KnightPath.Tests;

[TestFixture]
public class ShortestPathCalculatorTest
{
    [Test]
    public void CalculateOutOfBoundsRowX()
    {
        ArgumentException e = Assert.Throws<ArgumentException>(() => ShortestPathCalculator.CalculateShortestPath("Q2", "A2"));
        Assert.That(e.Message, Is.EqualTo("Invalid row identifier 'Q' in position 'Q2'."));
    }

    [Test]
    public void CalculateOutOfBoundsColumnX()
    {
        ArgumentException e = Assert.Throws<ArgumentException>(() => ShortestPathCalculator.CalculateShortestPath("A9", "A2"));
        Assert.That(e.Message, Is.EqualTo("Invalid column identifier '9' in position 'A9'."));
    }

    [Test]
    public void CalculateOutOfBoundsRowY()
    {
        ArgumentException e = Assert.Throws<ArgumentException>(() => ShortestPathCalculator.CalculateShortestPath("A2", "Q2"));
        Assert.That(e.Message, Is.EqualTo("Invalid row identifier 'Q' in position 'Q2'."));
    }


    [Test]
    public void CalculateOutOfBoundsColumnY()
    {
        ArgumentException e = Assert.Throws<ArgumentException>(() => ShortestPathCalculator.CalculateShortestPath("A2", "A9"));
        Assert.That(e.Message, Is.EqualTo("Invalid column identifier '9' in position 'A9'."));
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