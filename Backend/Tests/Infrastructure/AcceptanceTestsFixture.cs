using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Features.Shared.Infrastructure;

namespace Tests.Infrastructure;

public sealed class AcceptanceTestsFixture : IAsyncLifetime
{
    public WebApiApplicationFactory Factory { get; }

    public IFlurlClient Client { get; }

    public AcceptanceTestsFixture()
    {
        var databaseName = $"AcceptanceTests_{Guid.NewGuid():N}";
        Factory = new WebApiApplicationFactory(databaseName);
        Client = new FlurlClient(Factory.CreateClient());
    }

    public AsyncServiceScope CreateScope() => Factory.Services.CreateAsyncScope();

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        Client.Dispose();
        await Factory.DisposeAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        await using var scope = Factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppointmentIngestionDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }
}
