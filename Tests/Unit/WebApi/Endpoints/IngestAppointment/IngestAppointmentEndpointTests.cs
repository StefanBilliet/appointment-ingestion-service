using AutoFixture.Xunit3;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using WebApi.Endpoints;
using WebApi.Endpoints.IngestAppointment;

namespace Tests.Unit.WebApi.Endpoints.IngestAppointment;

public class IngestAppointmentEndpointTests
{
    private readonly IngestAppointmentEndpoint _sut;
    private readonly IIngestAppointService _ingestAppointService;

    public IngestAppointmentEndpointTests()
    {
        _ingestAppointService = A.Fake<IIngestAppointService>();
        _sut = new IngestAppointmentEndpoint(_ingestAppointService);
    }

    [Theory, AutoData]
    public async Task WHEN_IngestAppointment_THEN_return_confirmation(AppointmentToBeIngested appointmentToBeIngested, AppointmentIngestionConfirmation appointmentIngestionConfirmation)
    {
        A.CallTo(() => _ingestAppointService.IngestAppointment(appointmentToBeIngested, A<CancellationToken>._)).Returns(appointmentIngestionConfirmation);
        
        var result = await _sut.IngestAppointment(appointmentToBeIngested, TestContext.Current.CancellationToken);
        
        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(appointmentIngestionConfirmation, okObjectResult.Value);
    }
}