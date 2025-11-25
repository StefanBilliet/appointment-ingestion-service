using Microsoft.EntityFrameworkCore;
using WebApi.Features.Appointments.Get.Contracts;
using WebApi.Features.Shared.Infrastructure;

namespace WebApi.Features.Appointments.Get.Data;

public interface IGetIngestedAppointmentListItemsDataService
{
    Task<IReadOnlyCollection<IngestedAppointmentListItem>> Get(CancellationToken cancellationToken);
}

public class GetIngestedAppointmentListItemsDataService : IGetIngestedAppointmentListItemsDataService
{
    private readonly AppointmentIngestionDbContext _db;

    public GetIngestedAppointmentListItemsDataService(AppointmentIngestionDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyCollection<IngestedAppointmentListItem>> Get(CancellationToken cancellationToken)
    {
        return await _db.Appointments
            .Select(appointment =>
                new IngestedAppointmentListItem(appointment.Id, appointment.ClientName, appointment.AppointmentTime, appointment.ServiceDuration))
            .ToArrayAsync(cancellationToken: cancellationToken);
    }
}