using Microsoft.EntityFrameworkCore;
using WebApi.Features.Appointments.Ingestion.Domain;

namespace WebApi.Features.Shared.Infrastructure;

public class AppointmentIngestionDbContext : DbContext, IUnitOfWork
{
    public AppointmentIngestionDbContext(DbContextOptions<AppointmentIngestionDbContext> options)
        : base(options)
    {
    }

    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppointmentIngestionDbContext).Assembly);
    }
}
