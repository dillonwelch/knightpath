namespace KnightPath.Tests;

[TestFixture]
public class ShortestPathCalculatorOnBoardTest
{
    [Test]
    public void OnBoardValid()
    {
        Assert.That(ShortestPathCalculator.OnBoard(0, 0, 8, 8));
    }

    [Test]
    public void OnBoardValidExtraRows()
    {
        Assert.That(ShortestPathCalculator.OnBoard(8, 0, 9, 8));
    }

    [Test]
    public void OnBoardValidExtraColumns()
    {
        Assert.That(ShortestPathCalculator.OnBoard(0, 8, 8, 9));
    }

    [Test]
    public void OnBoardValidExtraRowsAndColumns()
    {
        Assert.That(ShortestPathCalculator.OnBoard(8, 8, 9, 9));
    }

    [Test]
    public void OnBoardInvalidNegativeX()
    {
        Assert.That(ShortestPathCalculator.OnBoard(-1, 0, 8, 8), Is.False);
    }

    [Test]
    public void OnBoardInvalidNegativeY()
    {
        Assert.That(ShortestPathCalculator.OnBoard(0, -1, 8, 8), Is.False);
    }

    [Test]
    public void OnBoardInvalidOutOfBoundsX()
    {
        Assert.That(ShortestPathCalculator.OnBoard(8, 0, 8, 8), Is.False);
    }

    [Test]
    public void OnBoardInvalidOutOfBoundsY()
    {
        Assert.That(ShortestPathCalculator.OnBoard(0, 8, 8, 8), Is.False);
    }
}