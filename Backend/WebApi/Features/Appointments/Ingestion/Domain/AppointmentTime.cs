using Vogen;

namespace WebApi.Features.Appointments.Ingestion.Domain;

[ValueObject<DateTimeOffset>]
public partial struct AppointmentTime
{
    public static Validation Validate(DateTimeOffset value) =>
        value.Minute % 30 != 0 ? Validation.Invalid("Appointment must start on the hour or half-hour.") : Validation.Ok;
}
