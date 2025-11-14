using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints;

[ApiController]
[Route("api")]
public class IngestAppointmentEndpoint : ControllerBase
{
    [HttpPost("appointments/ingest")]
    public Task<ActionResult<AppointmentIngestionConfirmation>> IngestAppointment([FromBody] AppointmentToBeIngested appointmentsToBeIngested)
    {
        throw new NotImplementedException();
    }
}

public record IngestedAppointment(int Id, string ClientName, DateTimeOffset AppointmentTime, int? ServiceDurationInMinutes);

public record AppointmentToBeIngested(string ClientName, DateTimeOffset AppointmentTime, int? ServiceDurationInMinutes = 30);

public record AppointmentIngestionConfirmation(int Id)
{
    public string Message => "Appointment created successfully.";
};