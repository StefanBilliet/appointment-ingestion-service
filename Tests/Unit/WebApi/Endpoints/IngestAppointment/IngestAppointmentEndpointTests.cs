using AutoFixture.Xunit3;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using WebApi.Endpoints.IngestAppointment;

namespace Tests.Unit.WebApi.Endpoints.IngestAppointment;

public class IngestAppointmentEndpointTests
{
    private readonly IngestAppointmentEndpoint _sut;
    private readonly IIngestAppointmentService _ingestAppointService;

    public IngestAppointmentEndpointTests()
    {
        _ingestAppointService = A.Fake<IIngestAppointmentService>();
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