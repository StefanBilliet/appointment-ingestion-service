using AutoFixture;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Time.Testing;
using WebApi.Endpoints;
using WebApi.Endpoints.IngestAppointment;

namespace Tests.Unit.WebApi.Endpoints.IngestAppointment;

public class AppointmentToBeIngestedValidatorTests
{
    private readonly AppointmentToBeIngestedValidator _sut;
    private readonly Fixture _fixture;
    private readonly FakeTimeProvider _fakeTimeProvider;

    public AppointmentToBeIngestedValidatorTests()
    {
        _fakeTimeProvider = new FakeTimeProvider();
        _sut = new AppointmentToBeIngestedValidator(_fakeTimeProvider);
        _fixture = new Fixture();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(3)]
    [InlineData(4)]
    public void Appointment_time_not_at_least_5_minutes_in_the_future_fails(int minutesInTheFuture)
    {
        var appointmentToBeIngested = _fixture.Build<AppointmentToBeIngested>()
            .With(appointment => appointment.AppointmentTime, _fakeTimeProvider.GetUtcNow().AddMinutes(minutesInTheFuture))
            .Create();
        
        var result = _sut.TestValidate(appointmentToBeIngested);
        
        result.ShouldHaveValidationErrorFor(appointment => appointment.AppointmentTime).WithErrorMessage($"Appointment time must be at least 5 minutes in the future. (Current time: {_fakeTimeProvider.GetUtcNow()})");
    }
    
    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(20)]
    public void Appointment_time_at_least_5_minutes_in_the_future_passes(int minutesInTheFuture)
    {
        var appointmentToBeIngested = _fixture.Build<AppointmentToBeIngested>()
            .With(appointment => appointment.AppointmentTime, _fakeTimeProvider.GetUtcNow().AddMinutes(minutesInTheFuture))
            .Create();
        
        var result = _sut.TestValidate(appointmentToBeIngested);
        
        result.ShouldNotHaveValidationErrorFor(appointment => appointment.AppointmentTime);
    }
}