namespace WebApi.Features.Appointments.Ingestion.Domain;

public class Appointment
{
    public int Id { get; private set; }
    public string ClientName { get; private set; }
    public AppointmentTime AppointmentTime { get; private set; }
    public ServiceDuration? ServiceDuration { get; private set; }

    private Appointment()
    {
        ClientName = null!;
        // for EF
    }

    private Appointment(string clientName, AppointmentTime appointmentTime, ServiceDuration? serviceDuration)
    {
        ClientName = clientName;
        AppointmentTime = appointmentTime;
        ServiceDuration = serviceDuration;
    }

    public static Appointment Ingest(string clientName, AppointmentTime appointmentTime, ServiceDuration? duration)
        => new(clientName, appointmentTime, duration);
}
