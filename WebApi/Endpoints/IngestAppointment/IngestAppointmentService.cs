namespace WebApi.Endpoints.IngestAppointment;

public interface IIngestAppointmentService
{
    Task<AppointmentIngestionConfirmation> IngestAppointment(AppointmentToBeIngested appointmentToBeIngested, CancellationToken currentCancellationToken);
}

public class IngestAppointmentService : IIngestAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public IngestAppointmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AppointmentIngestionConfirmation> IngestAppointment(AppointmentToBeIngested appointmentToBeIngested,
        CancellationToken currentCancellationToken)
    {
        var ingestedAppointment = Appointment.Ingest(appointmentToBeIngested.ClientName, appointmentToBeIngested.AppointmentTime, appointmentToBeIngested.ServiceDurationInMinutes);
        await _unitOfWork.AddAsync(ingestedAppointment, currentCancellationToken);
        await _unitOfWork.SaveChangesAsync(currentCancellationToken);
        return new AppointmentIngestionConfirmation(ingestedAppointment.Id);
    }
}