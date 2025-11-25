using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi;
using WebApi.Features.Shared.Infrastructure;

namespace Tests.Infrastructure;

public sealed class WebApiApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName;

    public WebApiApplicationFactory(string databaseName)
    {
        _databaseName = databaseName;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            var descriptor = services.FirstOrDefault(serviceDescriptor =>
                serviceDescriptor.ServiceType == typeof(DbContextOptions<AppointmentIngestionDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AppointmentIngestionDbContext>(options => options.UseInMemoryDatabase(_databaseName));
        });
    }
}
