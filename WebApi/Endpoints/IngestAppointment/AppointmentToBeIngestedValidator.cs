using FluentValidation;

namespace WebApi.Endpoints.IngestAppointment;

public class AppointmentToBeIngestedValidator : AbstractValidator<AppointmentToBeIngested>
{
    public AppointmentToBeIngestedValidator(TimeProvider timeProvider)
    {
        RuleFor(appointment => appointment.AppointmentTime)
            .Must((_, appointmentTime, context) =>
            {
                var now = timeProvider.GetUtcNow();
                context.MessageFormatter.AppendArgument("CurrentTime", now);
                return appointmentTime >= now.AddMinutes(5);
            })
            .WithMessage("Appointment time must be at least 5 minutes in the future. (Current time: {CurrentTime})");
    }
}