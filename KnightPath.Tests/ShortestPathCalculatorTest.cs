namespace KnightPath.Tests;

[TestFixture]
public class ShortestPathCalculatorTest
{
    [Test]
    public void CalculateInvalidStart()
    {
        ArgumentException e = Assert.Throws<ArgumentException>(
            () => ShortestPathCalculator.CalculateShortestPath("Meow", "A2")
        );
        Assert.That(e.Message, Is.EqualTo("Position 'Meow' must contain exactly two characters."));
    }

    [Test]
    public void CalculateInvalidEnd()
    {
        ArgumentException e = Assert.Throws<ArgumentException>(
            () => ShortestPathCalculator.CalculateShortestPath("A2", "Meow")
        );
        Assert.That(e.Message, Is.EqualTo("Position 'Meow' must contain exactly two characters."));
    }

    [Test]
    public void CalculateNoMove()
    {
        IList<string> actual = ShortestPathCalculator.CalculateShortestPath("A2", "A2");
        List<string> expected = ["A2"];
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CalculateOneMove()
    {
        IList<string> actual = ShortestPathCalculator.CalculateShortestPath("A2", "B4");
        List<string> expected = ["A2", "B4"];
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CalculateTwoMoves()
    {
        IList<string> actual = ShortestPathCalculator.CalculateShortestPath("G7", "C5");
        List<string> expected = ["G7", "E6", "C5"];
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CalculateThreeMoves()
    {
        IList<string> actual = ShortestPathCalculator.CalculateShortestPath("A1", "D5");
        List<string> expected = ["A1", "C2", "B4", "D5"];
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CalculateFourMoves()
    {
        IList<string> actual = ShortestPathCalculator.CalculateShortestPath("D4", "B2");
        List<string> expected = ["D4", "B3", "C1", "D3", "B2"];
        Assert.That(actual, Is.EqualTo(expected));
    }
}
