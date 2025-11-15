using WebApi.Features.Appointments.GetById.Contracts;

namespace WebApi.Features.Appointments.GetById.Data;

public interface IGetIngestedAppointmentByIdDataService
{
    Task<IngestedAppointment?> Get(int id, CancellationToken cancellationToken);
}

public class GetIngestedAppointmentByIdDataService : IGetIngestedAppointmentByIdDataService
{
    public Task<IngestedAppointment?> Get(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}