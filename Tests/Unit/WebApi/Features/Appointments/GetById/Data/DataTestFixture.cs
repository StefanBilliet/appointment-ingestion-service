using Microsoft.EntityFrameworkCore;
using WebApi.Features.Shared.Infrastructure;

namespace Tests.Unit.WebApi.Features.Appointments.GetById.Data;

public sealed class DataTestFixture
{
    public AppointmentIngestionDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppointmentIngestionDbContext>()
            .UseInMemoryDatabase($"GetByIdDataServiceTests_{Guid.NewGuid()}")
            .Options;

        var context = new AppointmentIngestionDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}
