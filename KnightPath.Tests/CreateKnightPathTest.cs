using System.Net;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging.Abstractions;
using static KnightPath.Tests.TestHelpers;

namespace KnightPath.Tests;

[TestFixture]
public class CreateKnightPathTest
{
    [Test]
    public async Task CreateKnightPathSuccessTest()
    {
        MockHttpRequestData mockHttpRequest = new MockHttpRequestDataBuilder()
            .WithDefaultJsonSerializer()
            .WithFakeFunctionContext()
            .WithRawJsonBody("{\"Source\": \"A1\", \"Target\": \"D5\"}")
            .Build();

        CreateKnightPath function = new(new NullLogger<CreateKnightPath>());
        MultiResponse response = await function.RunAsync(mockHttpRequest).ConfigureAwait(false);
        string streamText = await ReadBody(response.HttpResponse.Body).ConfigureAwait(false);

        Assert.Multiple(() =>
        {
            Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Message, Is.Not.Null);
            Assert.That(Guid.TryParse(streamText, out Guid val));
        });

        Assert.Multiple(() =>
        {
            Assert.That(response.Message.Source, Is.EqualTo("A1"));
            Assert.That(response.Message.Target, Is.EqualTo("D5"));
            Assert.That(Guid.TryParse(response.Message.TrackingId, out Guid val));
        });
    }

    [Test]
    public async Task CreateKnightPathSuccessCaseInsensitiveTest()
    {
        MockHttpRequestData mockHttpRequest = new MockHttpRequestDataBuilder()
            .WithDefaultJsonSerializer()
            .WithFakeFunctionContext()
            .WithRawJsonBody("{\"source\": \"A1\", \"target\": \"D5\"}")
            .Build();

        CreateKnightPath function = new(new NullLogger<CreateKnightPath>());
        MultiResponse response = await function.RunAsync(mockHttpRequest).ConfigureAwait(false);
        string streamText = await ReadBody(response.HttpResponse.Body).ConfigureAwait(false);

        Assert.Multiple(() =>
        {
            Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Message, Is.Not.Null);
            Assert.That(Guid.TryParse(streamText, out Guid val));
        });

        Assert.Multiple(() =>
        {
            Assert.That(response.Message.Source, Is.EqualTo("A1"));
            Assert.That(response.Message.Target, Is.EqualTo("D5"));
            Assert.That(Guid.TryParse(response.Message.TrackingId, out Guid val));
        });
    }

    [Test]
    public async Task CreateKnightPathMissingParamsTest()
    {
        MockHttpRequestData mockHttpRequest = new MockHttpRequestDataBuilder()
            .WithDefaultJsonSerializer()
            .WithFakeFunctionContext()
            .WithRawJsonBody("{\"Meow\": \"A1\", \"Woof\": \"D5\"}")
            .Build();

        CreateKnightPath function = new(new NullLogger<CreateKnightPath>());
        MultiResponse response = await function.RunAsync(mockHttpRequest).ConfigureAwait(false);
        string streamText = await ReadBody(response.HttpResponse.Body).ConfigureAwait(false);

        Assert.Multiple(() =>
        {
            Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(streamText, Is.EqualTo("Error processing request body!"));
            Assert.That(response.Message, Is.Null);
        });
    }

    [Test]
    public async Task CreateKnightPathEmptyParamTest()
    {
        MockHttpRequestData mockHttpRequest = new MockHttpRequestDataBuilder()
            .WithDefaultJsonSerializer()
            .WithFakeFunctionContext()
            .WithRawJsonBody("{\"Source\": \"A1\", \"Target\": \"\"}")
            .Build();

        CreateKnightPath function = new(new NullLogger<CreateKnightPath>());
        MultiResponse response = await function.RunAsync(mockHttpRequest).ConfigureAwait(false);
        string streamText = await ReadBody(response.HttpResponse.Body).ConfigureAwait(false);

        Assert.Multiple(() =>
        {
            Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(streamText, Is.EqualTo("Error processing request body!"));
            Assert.That(response.Message, Is.Null);
        });
    }

    [Test]
    public async Task CreateKnightPathInvalidJsonTest()
    {
        MockHttpRequestData mockHttpRequest = new MockHttpRequestDataBuilder()
            .WithDefaultJsonSerializer()
            .WithFakeFunctionContext()
            .WithRawJsonBody("{slakfjldkjafoiwjefoiwqjfeoij}")
            .Build();

        CreateKnightPath function = new(new NullLogger<CreateKnightPath>());
        MultiResponse response = await function.RunAsync(mockHttpRequest).ConfigureAwait(false);
        string streamText = await ReadBody(response.HttpResponse.Body).ConfigureAwait(false);

        Assert.Multiple(() =>
        {
            Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(streamText, Is.EqualTo("Error processing request body!"));
            Assert.That(response.Message, Is.Null);
        });
    }

    [Test]
    public async Task CreateKnightPathEmptyBodyTest()
    {
        MockHttpRequestData mockHttpRequest = new MockHttpRequestDataBuilder()
            .WithDefaultJsonSerializer()
            .WithFakeFunctionContext()
            .WithRawJsonBody("")
            .Build();

        CreateKnightPath function = new(new NullLogger<CreateKnightPath>());
        MultiResponse response = await function.RunAsync(mockHttpRequest).ConfigureAwait(false);
        string streamText = await ReadBody(response.HttpResponse.Body).ConfigureAwait(false);

        Assert.Multiple(() =>
        {
            Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(streamText, Is.EqualTo("Error processing request body!"));
            Assert.That(response.Message, Is.Null);
        });
    }
}
