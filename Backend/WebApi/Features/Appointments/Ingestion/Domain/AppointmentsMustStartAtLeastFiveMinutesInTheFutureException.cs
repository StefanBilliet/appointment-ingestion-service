namespace WebApi.Features.Appointments.Ingestion.Domain;

public sealed class AppointmentsMustStartAtLeastFiveMinutesInTheFutureException : Exception
{
    public AppointmentsMustStartAtLeastFiveMinutesInTheFutureException() : base("Appointments must start at least 5 minutes in the future.")
    {
    }

    public AppointmentsMustStartAtLeastFiveMinutesInTheFutureException(string message)
        : base(message)
    {
    }

    public AppointmentsMustStartAtLeastFiveMinutesInTheFutureException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}