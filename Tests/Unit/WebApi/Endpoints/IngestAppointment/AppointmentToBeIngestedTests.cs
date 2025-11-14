using WebApi.Endpoints.IngestAppointment;

namespace Tests.Unit.WebApi.Endpoints.IngestAppointment;

public class AppointmentToBeIngestedTests
{
    [Fact]
    public void GIVEN_no_service_duration_WHEN_creating_appointment_THEN_default_service_duration_is_used()
    {
        var appointment = new AppointmentToBeIngested("John Doe", DateTimeOffset.Now, null);
        
        Assert.Equal(ServiceDuration.Default, appointment.ServiceDurationInMinutes);
    }
    
    [Fact]
    public void GIVEN_service_duration_WHEN_creating_appointment_THEN_do_not_use_default_service_duration()
    {
        var appointment = new AppointmentToBeIngested("John Doe", DateTimeOffset.Now, ServiceDuration.From(45));
        
        Assert.Equal(ServiceDuration.From(45), appointment.ServiceDurationInMinutes);
    }
}