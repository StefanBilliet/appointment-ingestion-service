using Vogen;

namespace WebApi.Features.Appointments.Ingestion.Domain;

[ValueObject<int>]
public partial struct ServiceDuration
{
    public static readonly ServiceDuration Default = From(30);
    
    public static Validation Validate(int value) =>
        value <= 0 ? Validation.Invalid("Service duration must be positive.") : Validation.Ok;
}
