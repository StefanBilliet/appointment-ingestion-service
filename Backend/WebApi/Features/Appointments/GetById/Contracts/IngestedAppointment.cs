using WebApi.Features.Appointments.Ingestion.Domain;

namespace WebApi.Features.Appointments.GetById.Contracts;

public record IngestedAppointment(int Id, string ClientName, AppointmentTime AppointmentTime, ServiceDuration? ServiceDurationInMinutes);