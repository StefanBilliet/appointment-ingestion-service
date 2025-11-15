using Microsoft.EntityFrameworkCore;
using WebApi.Features.Appointments.GetById.Contracts;
using WebApi.Features.Shared.Infrastructure;

namespace WebApi.Features.Appointments.GetById.Data;

public interface IGetIngestedAppointmentByIdDataService
{
    Task<IngestedAppointment?> Get(int id, CancellationToken cancellationToken);
}

public class GetIngestedAppointmentByIdDataService : IGetIngestedAppointmentByIdDataService
{
    private readonly AppointmentIngestionDbContext _db;

    public GetIngestedAppointmentByIdDataService(AppointmentIngestionDbContext db)
    {
        _db = db;
    }

    public Task<IngestedAppointment?> Get(int id, CancellationToken cancellationToken)
    {
        return _db.Appointments.Where(appointment => appointment.Id == id)
            .Select(appointment => new IngestedAppointment(appointment.Id, appointment.ClientName, appointment.AppointmentTime, appointment.ServiceDuration))
            .SingleOrDefaultAsync(cancellationToken: cancellationToken);
    }
}