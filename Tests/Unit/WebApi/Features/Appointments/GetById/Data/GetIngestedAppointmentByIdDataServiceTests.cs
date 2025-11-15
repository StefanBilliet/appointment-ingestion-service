using Tests.Builders;
using WebApi.Features.Appointments.GetById.Data;
using WebApi.Features.Shared.Infrastructure;

namespace Tests.Unit.WebApi.Features.Appointments.GetById.Data;

public class GetIngestedAppointmentByIdDataServiceTests : IClassFixture<DataTestFixture>
{
    private readonly AppointmentIngestionDbContext _db;
    private readonly GetIngestedAppointmentByIdDataService _sut;

    public GetIngestedAppointmentByIdDataServiceTests(DataTestFixture fixture)
    {
        _db = fixture.CreateDbContext();
        _sut = new GetIngestedAppointmentByIdDataService(_db);
    }

    [Fact]
    public async Task GIVEN_no_appointment_with_id_WHEN_Get_THEN_return_null()
    {
        Assert.Null(await _sut.Get(0, TestContext.Current.CancellationToken));
    }
    
    [Fact]
    public async Task GIVEN_appointment_with_id_WHEN_Get_THEN_return_ingested_appointment()
    {
        var appointment = new AppointmentBuilder().Build();
        await _db.AddAsync(appointment, TestContext.Current.CancellationToken);
        await _db.SaveChangesAsync(TestContext.Current.CancellationToken);
        
        var ingestedAppointment = await _sut.Get(appointment.Id, TestContext.Current.CancellationToken);
        
        Assert.NotNull(ingestedAppointment);
        Assert.Equal(appointment.Id, ingestedAppointment.Id);
        Assert.Equal(appointment.ClientName, ingestedAppointment.ClientName);
        Assert.Equal(appointment.AppointmentTime, ingestedAppointment.AppointmentTime);
        Assert.Equal(appointment.ServiceDuration, ingestedAppointment.ServiceDurationInMinutes);
    }
}
