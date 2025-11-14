namespace WebApi.Endpoints.IngestAppointment;

public record AppointmentToBeIngested
{
    public string ClientName { get; init; }
    public DateTimeOffset AppointmentTime { get; init; }
    public ServiceDuration? ServiceDurationInMinutes { get; init; }
    
    public AppointmentToBeIngested(string ClientName, DateTimeOffset AppointmentTime, ServiceDuration? ServiceDurationInMinutes)
    {
        this.ClientName = ClientName;
        this.AppointmentTime = AppointmentTime;
        this.ServiceDurationInMinutes = ServiceDurationInMinutes ?? ServiceDuration.Default;
    }
}