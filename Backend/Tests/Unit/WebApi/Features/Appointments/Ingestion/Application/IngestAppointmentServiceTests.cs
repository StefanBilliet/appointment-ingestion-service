using FakeItEasy;
using WebApi.Features.Appointments.Ingestion.Application;
using WebApi.Features.Appointments.Ingestion.Contracts;
using WebApi.Features.Appointments.Ingestion.Domain;
using WebApi.Features.Shared.Infrastructure;

namespace Tests.Unit.WebApi.Features.Appointments.Ingestion.Application;

public class IngestAppointmentServiceTests
{
    private readonly IngestAppointmentService _sut;
    private readonly IUnitOfWork _unitOfWork;

    public IngestAppointmentServiceTests()
    {
        _unitOfWork = A.Fake<IUnitOfWork>();
        _sut = new IngestAppointmentService(_unitOfWork);
    }

    [Fact]
    public async Task WHEN_IngestAppointment_THEN_return_confirmation()
    {
        Appointment? persistedAppointment = null;
        A.CallTo(() => _unitOfWork.AddAsync(A<Appointment>._, A<CancellationToken>._)).Invokes(fakedCall => persistedAppointment = fakedCall.GetArgument<Appointment>(0));
        var appointmentToBeIngested = new AppointmentToBeIngested("John Doe", AppointmentTime.From(new DateTimeOffset(2020,1,1,1,0,0, TimeSpan.Zero)), ServiceDuration.From(45));
        
        var confirmation = await _sut.IngestAppointment(appointmentToBeIngested, TestContext.Current.CancellationToken);
        
        Assert.NotNull(persistedAppointment);
        Assert.Equal(persistedAppointment.Id, confirmation.Id);
        Assert.Equal(persistedAppointment.AppointmentTime, appointmentToBeIngested.AppointmentTime);
        Assert.Equal(persistedAppointment.ServiceDuration, appointmentToBeIngested.ServiceDurationInMinutes);
        Assert.Equal(persistedAppointment.ClientName, appointmentToBeIngested.ClientName);
        A.CallTo(() => _unitOfWork.SaveChangesAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
    }
}