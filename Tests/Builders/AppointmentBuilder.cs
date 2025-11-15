using System.Reflection;
using WebApi.Features.Appointments.Ingestion.Domain;

namespace Tests.Builders;

public sealed class AppointmentBuilder
{
    private int _id;
    private string _clientName = "Test Client";
    private AppointmentTime _appointmentTime = AppointmentTime.From(new DateTimeOffset(2020, 1, 1, 1, 0, 0, TimeSpan.Zero));
    private ServiceDuration? _serviceDuration = ServiceDuration.Default;

    public AppointmentBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public AppointmentBuilder WithClientName(string clientName)
    {
        _clientName = clientName;
        return this;
    }

    public AppointmentBuilder WithAppointmentTime(AppointmentTime appointmentTime)
    {
        _appointmentTime = appointmentTime;
        return this;
    }

    public AppointmentBuilder WithServiceDuration(ServiceDuration? duration)
    {
        _serviceDuration = duration;
        return this;
    }

    public Appointment Build()
    {
        var appointment = (Appointment)Activator.CreateInstance(
            typeof(Appointment),
            BindingFlags.Instance | BindingFlags.NonPublic,
            binder: null,
            args: [],
            culture: null)!;

        SetProperty(appointment, nameof(Appointment.Id), _id);
        SetProperty(appointment, nameof(Appointment.ClientName), _clientName);
        SetProperty(appointment, nameof(Appointment.AppointmentTime), _appointmentTime);
        SetProperty(appointment, nameof(Appointment.ServiceDuration), _serviceDuration);

        return appointment;
    }

    private static void SetProperty(Appointment appointment, string propertyName, object? value)
    {
        typeof(Appointment).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
            .SetValue(appointment, value);
    }
}
