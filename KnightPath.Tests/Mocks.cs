using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Azure.Core.Serialization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;

namespace KnightPath.Tests;

public sealed class MockHttpRequestData : HttpRequestData
{
    private readonly FunctionContext context;

    public MockHttpRequestData(
        FunctionContext context,
        string body)
            : base(context)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(body);
        Body = new MemoryStream(bytes);
        this.context = context;

        Cookies = new List<IHttpCookie>().AsReadOnly();

        Identities = new List<ClaimsIdentity>();
    }

    public override Stream Body { get; }

    public override HttpHeadersCollection Headers { get; } = [];

    public override IReadOnlyCollection<IHttpCookie> Cookies { get; }

    public override Uri Url { get; }

    public override IEnumerable<ClaimsIdentity> Identities { get; }

    public override string Method { get; }

    public override HttpResponseData CreateResponse()
    {
        MockHttpResponseData response = new(this.context);

        return response;
    }

    public void AddHeaderKeyVal(string key, string value)
    {
        Headers.Add(key, value);
    }

    public void AddQuery(string key, string value)
    {
        Query.Add(key, value);
    }
}


public sealed class MockHttpResponseData(FunctionContext context) : HttpResponseData(context)
{
    public override HttpStatusCode StatusCode { get; set; }

    public override HttpHeadersCollection Headers { get; set; } = [];

    public override Stream Body { get; set; } = new MemoryStream();

    public override HttpCookies Cookies { get; }
}

public class MockHttpRequestDataBuilder
{
    private readonly IServiceCollection requestServiceCollection;
    private IInvocationFeatures invocationFeatures;
    private IServiceProvider requestContextInstanceServices;
    private FunctionContext functionContext;

    private string rawJsonBody;

    public MockHttpRequestDataBuilder()
    {
        this.requestServiceCollection = new ServiceCollection();
        this.requestServiceCollection.AddOptions();
    }

    public MockHttpRequestDataBuilder WithDefaultJsonSerializer()
    {
        this.requestServiceCollection
            .Configure<WorkerOptions>(workerOptions =>
            {
                workerOptions.Serializer =
                    new JsonObjectSerializer(
                        new JsonSerializerOptions
                        {
                            AllowTrailingCommas = true,
                        });
            });

        return this;
    }

    public MockHttpRequestDataBuilder WithCustomJsonSerializerSettings(
        Func<JsonObjectSerializer> jsonObjectSerializerOptions)
    {
        this.requestServiceCollection.Configure<WorkerOptions>(
            workerOptions => workerOptions.Serializer = jsonObjectSerializerOptions());
        return this;
    }

    public MockHttpRequestDataBuilder WithRequestContextInstanceServices(
        IServiceProvider requestContextInstanceServices)
    {
        this.requestContextInstanceServices = requestContextInstanceServices;
        return this;
    }

    public MockHttpRequestDataBuilder WithInvocationFeatures(
        IInvocationFeatures invocationFeatures)
    {
        this.invocationFeatures = invocationFeatures;
        return this;
    }

    public MockHttpRequestDataBuilder WithFakeFunctionContext()
    {
        this.requestContextInstanceServices ??= this.requestServiceCollection.BuildServiceProvider();

        this.functionContext =
            new FakeFunctionContext(this.invocationFeatures)
            {
                InstanceServices = this.requestContextInstanceServices,
            };

        return this;
    }

    public MockHttpRequestDataBuilder WithRawJsonBody(string rawJsonBody)
    {
        this.rawJsonBody = rawJsonBody;
        return this;
    }

    public MockHttpRequestData Build()
        => new(this.functionContext, this.rawJsonBody);
}

public class FakeFunctionContext(IInvocationFeatures features, IDictionary<object, object> items = null) : FunctionContext
{
    public override string InvocationId { get; }

    public override string FunctionId { get; }

    public override TraceContext TraceContext { get; }

    public override BindingContext BindingContext { get; }

    public override RetryContext RetryContext { get; }

    public override IServiceProvider InstanceServices { get; set; }

    public override FunctionDefinition FunctionDefinition { get; }

    public override IDictionary<object, object> Items { get; set; } = items;

    public override IInvocationFeatures Features { get; } = features;
}