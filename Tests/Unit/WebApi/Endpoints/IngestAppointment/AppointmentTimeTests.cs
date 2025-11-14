using Vogen;
using WebApi.Endpoints.IngestAppointment;

namespace Tests.Unit.WebApi.Endpoints.IngestAppointment;

public class AppointmentTimeTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(29)]
    [InlineData(31)]
    public void GIVEN_appointment_time_that_is_not_aligned_to_half_an_hour_WHEN_creating_appointment_THEN_throws(int minutes)
    {
        Assert.Throws<ValueObjectValidationException>(() => AppointmentTime.From(new DateTimeOffset(2020,1,1,1,minutes,0, TimeSpan.Zero)));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(30)]
    public void GIVEN_appointment_time_that_is_aligned_to_half_an_hour_WHEN_creating_appointment_THEN_does_not_throw(int minutes)
    {
        _ = AppointmentTime.From(new DateTimeOffset(2020,1,1,1,minutes,0, TimeSpan.Zero));
    }
}
