using AutoFixture.Xunit3;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using WebApi.Features.Appointments.GetById.Contracts;
using WebApi.Features.Appointments.GetById.Data;
using WebApi.Features.Appointments.GetById.Presentation;

namespace Tests.Unit.WebApi.Features.Appointments.GetById.Presentation;

public class GetAppointmentByIdEndpointTests
{
    private readonly GetAppointmentByIdEndpoint _sut;
    private readonly IGetIngestedAppointmentByIdDataService _getIngestedAppointmentByIdDataService;

    public GetAppointmentByIdEndpointTests()
    {
        _getIngestedAppointmentByIdDataService = A.Fake<IGetIngestedAppointmentByIdDataService>();
        _sut = new GetAppointmentByIdEndpoint(_getIngestedAppointmentByIdDataService);
    }

    [Fact]
    public async Task GIVEN_no_appointment_with_id_WHEN_GetAppointmentById_THEN_return_not_found()
    {
        A.CallTo(() => _getIngestedAppointmentByIdDataService.Get(A<int>._, TestContext.Current.CancellationToken)).Returns<IngestedAppointment?>(null);
        
        var response = await _sut.GetAppointmentById(0, TestContext.Current.CancellationToken);
        
        Assert.IsType<NotFoundObjectResult>(response.Result);
    }
    
    [Theory, AutoData]
    public async Task GIVEN_appointment_with_id_WHEN_GetAppointmentById_THEN_return_ok_with_result_in_body(IngestedAppointment existingAppointment)
    {
        A.CallTo(() => _getIngestedAppointmentByIdDataService.Get(existingAppointment.Id, TestContext.Current.CancellationToken)).Returns(existingAppointment);
        
        var response = await _sut.GetAppointmentById(existingAppointment.Id, TestContext.Current.CancellationToken);
        
        var okObjectResult = Assert.IsType<OkObjectResult>(response.Result);
        var ingestedAppointment = Assert.IsType<IngestedAppointment>(okObjectResult.Value);      
        Assert.Equal(existingAppointment, ingestedAppointment);
    }
}