using AutoFixture;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Time.Testing;
using WebApi.Features.Appointments.Ingestion.Application;
using WebApi.Features.Appointments.Ingestion.Contracts;
using WebApi.Features.Appointments.Ingestion.Domain;

namespace Tests.Unit.WebApi.Features.Appointments.Ingestion.Application;

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
    [InlineData(-30)]
    [InlineData(0)]
    public void Appointment_time_not_in_the_future_fails(int minutesInTheFuture)
    {
        var appointmentToBeIngested = _fixture.Build<AppointmentToBeIngested>()
            .With(appointment => appointment.AppointmentTime, AppointmentTime.From(_fakeTimeProvider.GetUtcNow().AddMinutes(minutesInTheFuture)))
            .Create();
        
        var result = _sut.TestValidate(appointmentToBeIngested);
        
        result.ShouldHaveValidationErrorFor(appointment => appointment.AppointmentTime).WithErrorMessage("Appointment time must be in the future.");
    }
    
    [Theory]
    [InlineData(30)]
    [InlineData(60)]
    public void Appointment_time_in_the_future_passes(int minutesInTheFuture)
    {
        var appointmentToBeIngested = _fixture.Build<AppointmentToBeIngested>()
            .With(appointment => appointment.AppointmentTime, AppointmentTime.From(_fakeTimeProvider.GetUtcNow().AddMinutes(minutesInTheFuture)))
            .Create();
        
        var result = _sut.TestValidate(appointmentToBeIngested);
        
        result.ShouldNotHaveValidationErrorFor(appointment => appointment.AppointmentTime);
    }
}
