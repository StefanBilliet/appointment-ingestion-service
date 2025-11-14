using AutoFixture.Xunit3;
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

    [Theory(Explicit = true), AutoData]
    public async Task WHEN_IngestAppointment_THEN_return_confirmation(AppointmentToBeIngested appointmentToBeIngested)
    {
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