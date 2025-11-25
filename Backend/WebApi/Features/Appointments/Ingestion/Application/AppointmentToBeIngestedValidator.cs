using FluentValidation;
using WebApi.Features.Appointments.Ingestion.Contracts;

namespace WebApi.Features.Appointments.Ingestion.Application;

public class AppointmentToBeIngestedValidator : AbstractValidator<AppointmentToBeIngested>
{
    public AppointmentToBeIngestedValidator(TimeProvider timeProvider)
    {
        RuleFor(appointment => appointment.AppointmentTime)
            .Must((_, appointmentTime, context) =>
            {
                var now = timeProvider.GetUtcNow();
                context.MessageFormatter.AppendArgument("CurrentTime", now);
                return appointmentTime.Value >= now.AddMinutes(5);
            })
            .WithMessage("Appointment time must be in the future.");
    }
}
