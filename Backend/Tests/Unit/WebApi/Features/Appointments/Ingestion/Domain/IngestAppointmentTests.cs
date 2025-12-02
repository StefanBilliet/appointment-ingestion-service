using SystemClock;
using WebApi.Features.Appointments.Ingestion.Domain;

namespace Tests.Unit.WebApi.Features.Appointments.Ingestion.Domain;

public class IngestAppointmentTests
{
    [Theory]
    [InlineData(5)]
    [InlineData(35)]
    [InlineData(65)]
    public void WHEN_ingesting_appointment_at_least_5_minutes_in_the_future_THEN_does_not_throw(int minutesInTheFuture)
    {
        Clock.Set(new DateTimeOffset(2020, 1, 1, 0, 55, 0, TimeSpan.Zero));
        
        _ = Appointment.Ingest("test", AppointmentTime.From(Clock.Now.AddMinutes(minutesInTheFuture)), ServiceDuration.From(45));
    }
    
    [Fact]
    public void WHEN_ingesting_appointment_not_at_least_5_minutes_in_the_future_THEN_throws()
    {
        Clock.Set(new DateTimeOffset(2020, 1, 1, 0, 56, 0, TimeSpan.Zero));

        Assert.Throws<AppointmentsMustStartAtLeastFiveMinutesInTheFutureException>(() => Appointment.Ingest("test", AppointmentTime.From(Clock.Now.AddMinutes(4)), ServiceDuration.From(45)));
    }
}