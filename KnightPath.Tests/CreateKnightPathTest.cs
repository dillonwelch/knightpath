// using Microsoft.Extensions.Logging.Abstractions;
using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Moq; // TODO: Remove?

namespace KnightPath.Tests;
  
[TestFixture]
public class CreateKnightPathTest
{
    // NOTE: I would love to be testing the response body content but I can't figure out how :(
    [Test]
    public async Task CreateKnightPathSuccessTest()
    {
        MockHttpRequestData mockHttpRequest =
          new MockHttpRequestDataBuilder()
            .WithDefaultJsonSerializer()
            .WithFakeFunctionContext()
            .WithRawJsonBody("{\"Source\": \"A1\", \"Target\": \"D5\"}")
            .Build();

        var logger = new NullLogger<CreateKnightPath>();
        CreateKnightPath function = new(logger);
        var response = await function.RunAsync(mockHttpRequest).ConfigureAwait(false);

        Assert.That(response.HttpResponse, Is.InstanceOf(typeof(HttpResponseData)));
        Assert.Multiple(() =>
        {
            Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Message, Is.Not.Null);
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
        MockHttpRequestData mockHttpRequest =
          new MockHttpRequestDataBuilder()
            .WithDefaultJsonSerializer()
            .WithFakeFunctionContext()
            .WithRawJsonBody("{\"Meow\": \"A1\", \"Woof\": \"D5\"}")
            .Build();

        var logger = new NullLogger<CreateKnightPath>();
        CreateKnightPath function = new(logger);
        var response = await function.RunAsync(mockHttpRequest).ConfigureAwait(false);

        Assert.That(response.HttpResponse, Is.InstanceOf(typeof(HttpResponseData)));
        Assert.Multiple(() =>
        {
            Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Message, Is.Null);
        });
    }

    [Test]
    public async Task CreateKnightPathEmptyParamTest()
    {
        MockHttpRequestData mockHttpRequest =
          new MockHttpRequestDataBuilder()
            .WithDefaultJsonSerializer()
            .WithFakeFunctionContext()
            .WithRawJsonBody("{\"Source\": \"A1\", \"Target\": \"\"}")
            .Build();

        var logger = new NullLogger<CreateKnightPath>();
        CreateKnightPath function = new(logger);
        var response = await function.RunAsync(mockHttpRequest).ConfigureAwait(false);

        Assert.That(response.HttpResponse, Is.InstanceOf(typeof(HttpResponseData)));
        Assert.Multiple(() =>
        {
            Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Message, Is.Null);
        });
    }

    [Test]
    public async Task CreateKnightPathInvalidJsonTest()
    {
        MockHttpRequestData mockHttpRequest =
          new MockHttpRequestDataBuilder()
            .WithDefaultJsonSerializer()
            .WithFakeFunctionContext()
            .WithRawJsonBody("{slakfjldkjafoiwjefoiwqjfeoij}")
            .Build();

        var logger = new NullLogger<CreateKnightPath>();
        CreateKnightPath function = new(logger);
        var response = await function.RunAsync(mockHttpRequest).ConfigureAwait(false);

        Assert.That(response.HttpResponse, Is.InstanceOf(typeof(HttpResponseData)));
        Assert.Multiple(() =>
        {
            Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Message, Is.Null);
        });

    }

    [Test]
    public async Task CreateKnightPathEmptyBodyTest()
    {
        MockHttpRequestData mockHttpRequest =
          new MockHttpRequestDataBuilder()
            .WithDefaultJsonSerializer()
            .WithFakeFunctionContext()
            .WithRawJsonBody("")
            .Build();

        var logger = new NullLogger<CreateKnightPath>();
        CreateKnightPath function = new(logger);
        var response = await function.RunAsync(mockHttpRequest).ConfigureAwait(false);

        Assert.That(response.HttpResponse, Is.InstanceOf(typeof(HttpResponseData)));
        Assert.Multiple(() =>
        {
            Assert.That(response.HttpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Message, Is.Null);
        });
    }
}

