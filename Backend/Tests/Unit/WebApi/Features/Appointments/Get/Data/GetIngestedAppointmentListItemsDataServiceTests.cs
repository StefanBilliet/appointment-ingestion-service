using Tests.Builders;
using Tests.Infrastructure;
using WebApi.Features.Appointments.Get.Data;
using WebApi.Features.Appointments.GetById.Data;
using WebApi.Features.Shared.Infrastructure;

namespace Tests.Unit.WebApi.Features.Appointments.Get.Data;

public class GetIngestedAppointmentListItemsDataServiceTests : IClassFixture<DataTestFixture>
{
    private readonly AppointmentIngestionDbContext _db;
    private readonly GetIngestedAppointmentListItemsDataService _sut;

    public GetIngestedAppointmentListItemsDataServiceTests(DataTestFixture fixture)
    {
        _db = fixture.CreateDbContext();
        _sut = new GetIngestedAppointmentListItemsDataService(_db);
    }

    [Fact]
    public async Task GIVEN_no_appointment_with_id_WHEN_Get_THEN_return_empty_collection()
    {
        Assert.Empty(await _sut.Get(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task GIVEN_appointment_with_id_WHEN_Get_THEN_return_ingested_appointment()
    {
        var appointment = new AppointmentBuilder().Build();
        await _db.AddAsync(appointment, TestContext.Current.CancellationToken);
        await _db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var ingestedAppointments = await _sut.Get(TestContext.Current.CancellationToken);

        var ingestedAppointmentListItem = Assert.Single(ingestedAppointments);
        Assert.Equal(appointment.Id, ingestedAppointmentListItem.Id);
        Assert.Equal(appointment.ClientName, ingestedAppointmentListItem.ClientName);
        Assert.Equal(appointment.AppointmentTime, ingestedAppointmentListItem.AppointmentTime);
        Assert.Equal(appointment.ServiceDuration, ingestedAppointmentListItem.ServiceDuration);
    }
}