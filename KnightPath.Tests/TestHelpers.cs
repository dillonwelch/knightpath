namespace KnightPath.Tests;

public static class TestHelpers
{
    public static async Task<string> ReadBody(Stream body)
    {
        ArgumentNullException.ThrowIfNull(body);

        body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(body);
        var streamText = await reader.ReadToEndAsync().ConfigureAwait(false);
        reader.Dispose();

        return streamText;
    }
}