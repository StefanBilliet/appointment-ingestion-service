using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using Tests.Infrastructure;
using WebApi.Features.Appointments.GetById.Contracts;
using WebApi.Features.Appointments.Ingestion.Domain;
using WebApi.Features.Shared.Infrastructure;

namespace Tests.Acceptance.Features.Appointments.GetById;

public sealed class GetAppointmentByIdEndpointTests : IClassFixture<AcceptanceTestsFixture>
{
    private readonly IFlurlClient _client;
    private readonly AppointmentIngestionDbContext _db;

    public GetAppointmentByIdEndpointTests(AcceptanceTestsFixture fixture)
    {
        _db = fixture.Factory.Services.GetRequiredService<AppointmentIngestionDbContext>();
        _client = fixture.Client;
    }

    [Fact(Explicit = true)]
    public async Task GIVEN_existing_appointment_WHEN_request_by_id_THEN_returns_appointment()
    {
        var now = DateTimeOffset.UtcNow;
        var appointment = Appointment.Ingest(
            "Alice Johnson",
            AppointmentTime.From(new DateTimeOffset(now.Year, now.Month, now.Day, now.Hour, 0, 0, TimeSpan.Zero).AddHours(2)),
            ServiceDuration.From(45));
        await _db.Appointments.AddAsync(appointment, TestContext.Current.CancellationToken);
        await _db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var retrieved = await _client
            .Request($"/api/appointments/{appointment.Id}")
            .GetJsonAsync<IngestedAppointment>(cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(appointment.Id, retrieved.Id);
        Assert.Equal(appointment.ClientName, retrieved.ClientName);
        Assert.Equal(appointment.AppointmentTime, retrieved.AppointmentTime);
        Assert.Equal(appointment.ServiceDuration, retrieved.ServiceDurationInMinutes);
    }
}
