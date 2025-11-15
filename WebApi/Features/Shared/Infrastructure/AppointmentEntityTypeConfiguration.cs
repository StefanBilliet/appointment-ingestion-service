using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Features.Appointments.Ingestion.Domain;

namespace WebApi.Features.Shared.Infrastructure;

public sealed class AppointmentEntityTypeConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(appointment => appointment.Id);

        builder.Property(appointment => appointment.Id)
            .ValueGeneratedOnAdd();

        builder.Property(appointment => appointment.ClientName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(appointment => appointment.AppointmentTime)
            .IsRequired()
            .HasConversion(
                appointmentTime => appointmentTime.Value,
                dateTimeOffset => AppointmentTime.From(dateTimeOffset));

        builder.Property(appointment => appointment.ServiceDuration)
            .HasConversion(
                duration => duration.HasValue ? duration.Value.Value : default(int?),
                value => value.HasValue ? ServiceDuration.From(value.Value) : null);
    }
}
