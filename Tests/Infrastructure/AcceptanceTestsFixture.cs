using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Infrastructure;

public sealed class AcceptanceTestsFixture : IAsyncLifetime
{
    public WebApiApplicationFactory Factory { get; }

    public IFlurlClient Client { get; }

    public AcceptanceTestsFixture()
    {
        Factory = new WebApiApplicationFactory();
        Client = new FlurlClient(Factory.CreateClient());
    }

    public AsyncServiceScope CreateScope() => Factory.Services.CreateAsyncScope();

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        Client.Dispose();
        await Factory.DisposeAsync();
    }
}
