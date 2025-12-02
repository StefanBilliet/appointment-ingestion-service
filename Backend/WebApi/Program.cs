using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Features.Appointments.Get.Data;
using WebApi.Features.Appointments.GetById.Data;
using WebApi.Features.Appointments.Ingestion.Application;
using WebApi.Features.Appointments.Ingestion.Domain;
using WebApi.Features.Appointments.Ingestion.Contracts;
using WebApi.Features.Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        if (context.Exception is AppointmentsMustStartAtLeastFiveMinutesInTheFutureException domainException)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.ProblemDetails = new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                [nameof(AppointmentToBeIngested.AppointmentTime)] = ["Appointment time must be in the future."]
            })
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Appointment not allowed",
                Detail = domainException.Message
            };
        }
    };
});
builder.Services.AddDbContext<AppointmentIngestionDbContext>(options => options.UseInMemoryDatabase("AppointmentIngestion"));
builder.Services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<AppointmentIngestionDbContext>());
builder.Services.AddScoped<IIngestAppointmentService, IngestAppointmentService>();
builder.Services.AddScoped<IGetIngestedAppointmentByIdDataService, GetIngestedAppointmentByIdDataService>();
builder.Services.AddScoped<IGetIngestedAppointmentListItemsDataService, GetIngestedAppointmentListItemsDataService>();

var openApiFilePath = Path.Combine(AppContext.BaseDirectory, "openapi", "appointments.openapi.yaml");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/appointments.openapi.yaml", "Appointment Ingestion API");
        options.RoutePrefix = "docs";
    });

    var envUrls = (Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? string.Empty)
        .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    var configuredUrls = envUrls.Length > 0
        ? envUrls
        : (app.Urls.Count != 0
            ? app.Urls.ToArray()
            : ["http://localhost:5000", "https://localhost:5001"]);

    var preferredUrl = configuredUrls.FirstOrDefault(url => url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                       ?? configuredUrls.First();
    var trimmed = preferredUrl.TrimEnd('/');
    var docsUrl = $"{trimmed}/docs";
    var specUrl = $"{trimmed}/openapi/appointments.openapi.yaml";
    app.Logger.LogInformation("Swagger UI available at {DocsUrl} and OpenAPI spec at {SpecUrl}", docsUrl, specUrl);
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        var status = exception switch
        {
            AppointmentsMustStartAtLeastFiveMinutesInTheFutureException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";

        var problemDetailsService = context.RequestServices.GetRequiredService<IProblemDetailsService>();
        await problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = context,
            Exception = exception,
            ProblemDetails = { Status = status }
        });
    });
});
app.UseHttpsRedirection();

app.MapControllers();
app.MapGet("/openapi/appointments.openapi.yaml", () =>
    File.Exists(openApiFilePath)
        ? Results.File(openApiFilePath, "application/yaml")
        : Results.NotFound());

app.Run();

namespace WebApi
{
    public partial class Program;
}