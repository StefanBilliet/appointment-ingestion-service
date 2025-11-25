using Microsoft.AspNetCore.Mvc;
using WebApi.Features.Appointments.Ingestion.Application;
using WebApi.Features.Appointments.Ingestion.Contracts;

namespace WebApi.Features.Appointments.Ingestion.Presentation;

[ApiController]
[Route("api")]
public class IngestAppointmentEndpoint : ControllerBase
{
    private readonly IIngestAppointmentService _ingestAppointmentService;

    public IngestAppointmentEndpoint(IIngestAppointmentService ingestAppointmentService)
    {
        _ingestAppointmentService = ingestAppointmentService;
    }

    [HttpPost("appointments/ingest")]
    public async Task<ActionResult<AppointmentIngestionConfirmation>> IngestAppointment(
        [FromBody] AppointmentToBeIngested appointmentToBeIngested,
        CancellationToken cancellationToken)
    {
        var confirmation = await _ingestAppointmentService.IngestAppointment(appointmentToBeIngested, cancellationToken);
        return Ok(confirmation);
    }
}
