using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using Tests.Infrastructure;
using WebApi.Features.Appointments.GetById.Contracts;
using WebApi.Features.Appointments.Ingestion.Domain;
using WebApi.Features.Shared.Infrastructure;

namespace Tests.Acceptance.Features.Appointments.GetById;

public sealed class GetIngestedAppointmentListItemsEndpointTests : IClassFixture<AcceptanceTestsFixture>
{
    private readonly AcceptanceTestsFixture _fixture;
    private readonly IFlurlClient _client;

    public GetIngestedAppointmentListItemsEndpointTests(AcceptanceTestsFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.Client;
    }
    
    [Fact]
    public async Task GIVEN_existing_appointment_WHEN_get_by_id_THEN_returns_appointment()
    {
        await _fixture.ResetDatabaseAsync();
        var now = DateTimeOffset.UtcNow;
        var appointment = Appointment.Ingest(
            "Alice Johnson",
            AppointmentTime.From(new DateTimeOffset(now.Year, now.Month, now.Day, now.Hour, 0, 0, TimeSpan.Zero).AddHours(2)),
            ServiceDuration.From(45));
        await using var scope = _fixture.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppointmentIngestionDbContext>();
        await db.Appointments.AddAsync(appointment, TestContext.Current.CancellationToken);
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var retrieved = await _client
            .Request($"/api/appointments/{appointment.Id}")
            .GetJsonAsync<IngestedAppointment>(cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(appointment.Id, retrieved.Id);
        Assert.Equal(appointment.ClientName, retrieved.ClientName);
        Assert.Equal(appointment.AppointmentTime, retrieved.AppointmentTime);
        Assert.Equal(appointment.ServiceDuration, retrieved.ServiceDurationInMinutes);
    }
}
