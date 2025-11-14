namespace WebApi.Endpoints.IngestAppointment;

public record AppointmentToBeIngested
{
    public string ClientName { get; init; }
    public AppointmentTime AppointmentTime { get; init; }
    public ServiceDuration? ServiceDurationInMinutes { get; init; }
    
    public AppointmentToBeIngested(string clientName, AppointmentTime appointmentTime, ServiceDuration? serviceDurationInMinutes)
    {
        ClientName = clientName;
        AppointmentTime = appointmentTime;
        ServiceDurationInMinutes = serviceDurationInMinutes ?? ServiceDuration.Default;
    }
}