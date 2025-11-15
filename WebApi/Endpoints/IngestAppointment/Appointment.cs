namespace WebApi.Endpoints.IngestAppointment;

public class Appointment
{
    public int Id { get; private set; }
    public string ClientName { get; }
    public AppointmentTime AppointmentTime { get; }
    public ServiceDuration? ServiceDuration { get; }

    private Appointment() { } // for EF

    private Appointment(string clientName, AppointmentTime appointmentTime, ServiceDuration? serviceDuration)
    {
        ClientName = clientName;
        AppointmentTime = appointmentTime;
        ServiceDuration = serviceDuration;
    }

    public static Appointment Ingest(string clientName, AppointmentTime appointmentTime, ServiceDuration? duration)
        => new(clientName, appointmentTime, duration);
}