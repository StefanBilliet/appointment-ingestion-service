using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints;

[ApiController]
[Route("api")]
public class IngestAppointmentEndpoint : ControllerBase
{
    private readonly IIngestAppointService _ingestAppointService;

    public IngestAppointmentEndpoint(IIngestAppointService ingestAppointService)
    {
        _ingestAppointService = ingestAppointService;
    }

    [HttpPost("appointments/ingest")]
    public async Task<ActionResult<AppointmentIngestionConfirmation>> IngestAppointment([FromBody] AppointmentToBeIngested appointmentsToBeIngested,
        CancellationToken currentCancellationToken)
    {
        var confirmation = await _ingestAppointService.IngestAppointment(appointmentsToBeIngested, currentCancellationToken);
        return Ok(confirmation);
    }
}

public record IngestedAppointment(int Id, string ClientName, DateTimeOffset AppointmentTime, int? ServiceDurationInMinutes);

public record AppointmentToBeIngested(string ClientName, DateTimeOffset AppointmentTime, int? ServiceDurationInMinutes = 30);

public record AppointmentIngestionConfirmation(int Id)
{
    public string Message => "Appointment created successfully.";
};

public interface IIngestAppointService
{
    Task<AppointmentIngestionConfirmation> IngestAppointment(AppointmentToBeIngested appointmentToBeIngested, CancellationToken cancellationToken);
}