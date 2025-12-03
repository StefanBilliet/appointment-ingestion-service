using WebApi.Features.Appointments.Ingestion.Domain;

namespace WebApi.Features.Appointments.Get.Contracts;

public record IngestedAppointmentListItem(int Id, string ClientName, AppointmentTime AppointmentTime, ServiceDuration ServiceDuration);