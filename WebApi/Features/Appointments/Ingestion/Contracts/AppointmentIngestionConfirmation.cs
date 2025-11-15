using WebApi.Features.Appointments.Ingestion.Domain;

namespace WebApi.Features.Appointments.Ingestion.Contracts;

public record AppointmentIngestionConfirmation(int Id)
{
    public string Message => "Appointment created successfully.";
}

public record IngestedAppointment(int Id, string ClientName, AppointmentTime AppointmentTime, ServiceDuration? ServiceDurationInMinutes);
