using WebApi.Features.Appointments.Ingestion.Contracts;
using WebApi.Features.Appointments.Ingestion.Domain;

namespace Tests.Unit.WebApi.Features.Appointments.Ingestion.Contracts;

public class AppointmentToBeIngestedTests
{
    [Fact]
    public void GIVEN_no_service_duration_WHEN_creating_appointment_THEN_default_service_duration_is_used()
    {
        var appointment = new AppointmentToBeIngested
        {
            ClientName = "John Doe",
            AppointmentTime = AppointmentTime.From(new DateTimeOffset(2020,1,1,1,0,0, TimeSpan.Zero))
        };
        
        Assert.Equal(ServiceDuration.Default, appointment.ServiceDuration);
    }
    
    [Fact]
    public void GIVEN_service_duration_WHEN_creating_appointment_THEN_do_not_use_default_service_duration()
    {
        var appointment = new AppointmentToBeIngested
        {
            ClientName = "John Doe",
            AppointmentTime = AppointmentTime.From(new DateTimeOffset(2020,1,1,1,0,0, TimeSpan.Zero)),
            ServiceDuration = ServiceDuration.From(45)
        };
        
        Assert.Equal(ServiceDuration.From(45), appointment.ServiceDuration);
    }
}
