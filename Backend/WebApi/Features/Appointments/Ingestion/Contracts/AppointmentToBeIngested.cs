using WebApi.Features.Appointments.Ingestion.Domain;

namespace WebApi.Features.Appointments.Ingestion.Contracts;

public record AppointmentToBeIngested
{
    public required string ClientName { get; init; }
    public AppointmentTime AppointmentTime { get; init; }
    public ServiceDuration ServiceDuration { get; init; } = ServiceDuration.Default;
}
