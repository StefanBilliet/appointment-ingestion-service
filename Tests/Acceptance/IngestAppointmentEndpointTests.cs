using Flurl.Http;
using Tests.Infrastructure;
using WebApi.Endpoints.IngestAppointment;

namespace Tests.Acceptance;

public sealed class IngestAppointmentEndpointTests : IClassFixture<AcceptanceTestsFixture>
{
    private readonly IFlurlClient _client;

    public IngestAppointmentEndpointTests(AcceptanceTestsFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact(Explicit = true)]
    public async Task WHEN_IngestAppointment_THEN_return_confirmation()
    {
        var appointmentToBeIngested = new AppointmentToBeIngested("John Doe", AppointmentTime.From(new DateTimeOffset(2020,1,1,1,0,0, TimeSpan.Zero)), ServiceDuration.From(45));
        
        var confirmation = await _client
            .Request("/api/appointments/ingest")
            .PostJsonAsync(appointmentToBeIngested, cancellationToken: TestContext.Current.CancellationToken)
            .ReceiveJson<AppointmentIngestionConfirmation>();

        await AssertAppointmentsExist(appointmentToBeIngested, confirmation);
    }

    private async Task AssertAppointmentsExist(AppointmentToBeIngested appointmentToBeIngested,
        AppointmentIngestionConfirmation confirmation)
    {
        var ingestedAppointment = await _client
            .Request($"/api/appointments/{confirmation.Id}")
            .GetJsonAsync<IngestedAppointment>();

        Assert.Equal(confirmation.Id, ingestedAppointment.Id);
        Assert.Equal(appointmentToBeIngested.ClientName, ingestedAppointment.ClientName);
        Assert.Equal(appointmentToBeIngested.AppointmentTime, ingestedAppointment.AppointmentTime);
        Assert.Equal(appointmentToBeIngested.ServiceDurationInMinutes, ingestedAppointment.ServiceDurationInMinutes);
    }
}