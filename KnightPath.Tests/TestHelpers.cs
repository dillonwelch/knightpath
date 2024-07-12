namespace KnightPath.Tests;

public static class TestHelpers
{
    public static async Task<string> ReadBody(Stream body)
    {
        ArgumentNullException.ThrowIfNull(body);

        body.Seek(0, SeekOrigin.Begin);
        StreamReader reader = new(body);
        string streamText = await reader.ReadToEndAsync().ConfigureAwait(false);
        reader.Dispose();

        return streamText;
    }
}
