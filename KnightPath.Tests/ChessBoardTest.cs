namespace KnightPath.Tests;

[TestFixture]
public class ChessBoardTest
{
    [Test]
    public void ValidatePositionNull()
    {
        ArgumentNullException e = Assert.Throws<ArgumentNullException>(
            () => ChessBoard.ValidatePosition(null!)
        );
        Assert.That(e.Message, Is.EqualTo("Value cannot be null. (Parameter 'position')"));
    }

    [Test]
    public void ValidatePositionEmpty()
    {
        ArgumentException e = Assert.Throws<ArgumentException>(
            () => ChessBoard.ValidatePosition("")
        );
        Assert.That(
            e.Message,
            Is.EqualTo(
                "The value cannot be an empty string or composed entirely of whitespace. (Parameter 'position')"
            )
        );
    }

    [Test]
    public void ValidatePositionWhitespace()
    {
        ArgumentException e = Assert.Throws<ArgumentException>(
            () => ChessBoard.ValidatePosition("      ")
        );
        Assert.That(
            e.Message,
            Is.EqualTo(
                "The value cannot be an empty string or composed entirely of whitespace. (Parameter 'position')"
            )
        );
    }

    [Test]
    public void ValidatePositionTooSmall()
    {
        ArgumentException e = Assert.Throws<ArgumentException>(
            () => ChessBoard.ValidatePosition("A")
        );
        Assert.That(e.Message, Is.EqualTo("Position 'A' must contain exactly two characters."));
    }

    [Test]
    public void ValidatePositionValid()
    {
        Assert.DoesNotThrow(() => ChessBoard.ValidatePosition("A2"));
    }

    [Test]
    public void ValidatePositionTooLarge()
    {
        ArgumentException e = Assert.Throws<ArgumentException>(
            () => ChessBoard.ValidatePosition("AA2")
        );
        Assert.That(e.Message, Is.EqualTo("Position 'AA2' must contain exactly two characters."));
    }

    [Test]
    public void ValidatePositionRowOutOfBoundsLow()
    {
        ArgumentException e = Assert.Throws<ArgumentException>(
            () => ChessBoard.ValidatePosition(".2")
        );
        Assert.That(e.Message, Is.EqualTo("Invalid row identifier '.' in position '.2'."));
    }

    [Test]
    public void ValidatePositionRowOutOfBoundsHigh()
    {
        ArgumentException e = Assert.Throws<ArgumentException>(
            () => ChessBoard.ValidatePosition("Q2")
        );
        Assert.That(e.Message, Is.EqualTo("Invalid row identifier 'Q' in position 'Q2'."));
    }

    [Test]
    public void ValidatePositionColumnOutOfBoundsLow()
    {
        ArgumentException e = Assert.Throws<ArgumentException>(
            () => ChessBoard.ValidatePosition("A0")
        );
        Assert.That(e.Message, Is.EqualTo("Invalid column identifier '0' in position 'A0'."));
    }

    [Test]
    public void ValidatePositionColumnOutOfBoundsHigh()
    {
        ArgumentException e = Assert.Throws<ArgumentException>(
            () => ChessBoard.ValidatePosition("A9")
        );
        Assert.That(e.Message, Is.EqualTo("Invalid column identifier '9' in position 'A9'."));
    }
}
