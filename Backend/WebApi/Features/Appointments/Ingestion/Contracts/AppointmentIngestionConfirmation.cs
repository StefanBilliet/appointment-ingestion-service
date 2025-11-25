namespace WebApi.Features.Appointments.Ingestion.Contracts;

public record AppointmentIngestionConfirmation(int Id)
{
    public string Message => "Appointment created successfully.";
}
