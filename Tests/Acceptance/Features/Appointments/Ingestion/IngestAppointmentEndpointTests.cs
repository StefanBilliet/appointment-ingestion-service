using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tests.Infrastructure;
using WebApi.Features.Appointments.Ingestion.Contracts;
using WebApi.Features.Appointments.Ingestion.Domain;

namespace Tests.Acceptance.Features.Appointments.Ingestion;

public sealed class IngestAppointmentEndpointTests : IClassFixture<AcceptanceTestsFixture>
{
    private readonly IFlurlClient _client;

    public IngestAppointmentEndpointTests(AcceptanceTestsFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task GIVEN_appointment_in_the_past_WHEN_IngestAppointment_THEN_return_bad_request_with_problem_details()
    {
        var appointmentToBeIngested = new AppointmentToBeIngested("John Doe", AppointmentTime.From(new DateTimeOffset(2020,1,1,1,0,0, TimeSpan.Zero)), ServiceDuration.From(45));

        var confirmation = await _client
            .AllowAnyHttpStatus()
            .Request("/api/appointments/ingest")
            .PostJsonAsync(appointmentToBeIngested, cancellationToken: TestContext.Current.CancellationToken)
         .ReceiveJson<ValidationProblemDetails>();

        Assert.Equal(StatusCodes.Status400BadRequest, confirmation.Status);
        Assert.Contains(confirmation.Errors,
            error => error.Key == nameof(appointmentToBeIngested.AppointmentTime) && error.Value.Contains("Appointment time must be in the future."));
    }
    
    [Fact(Explicit = true)]
    public async Task GIVEN_appointment_in_the_future_WHEN_IngestAppointment_THEN_return_confirmation()
    {
        var tomorrow = DateTimeOffset.Now.AddDays(1);
        var appointmentToBeIngested = new AppointmentToBeIngested("John Doe", AppointmentTime.From(new DateTimeOffset(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 0, 0, tomorrow.Offset)), ServiceDuration.From(45));

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
