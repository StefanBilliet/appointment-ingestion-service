using AutoFixture.Xunit3;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using WebApi.Features.Appointments.Get.Contracts;
using WebApi.Features.Appointments.Get.Data;
using WebApi.Features.Appointments.Get.Presentation;

namespace Tests.Unit.WebApi.Features.Appointments.Get.Presentation;

public class GetIngestedAppointmentListItemsEndpointTests
{
    private readonly GetIngestedAppointmentListItemsEndpoint _sut;
    private readonly IGetIngestedAppointmentListItemsDataService _getIngestedAppointmentListItemsDataService;

    public GetIngestedAppointmentListItemsEndpointTests()
    {
        _getIngestedAppointmentListItemsDataService = A.Fake<IGetIngestedAppointmentListItemsDataService>();
        _sut = new GetIngestedAppointmentListItemsEndpoint(_getIngestedAppointmentListItemsDataService);
    }

    [Fact]
    public async Task GIVEN_no_appointment_with_id_WHEN_GetAppointmentById_THEN_return_not_found()
    {
        A.CallTo(() => _getIngestedAppointmentListItemsDataService.Get(TestContext.Current.CancellationToken)).Returns([]);

        var response = await _sut.Get(TestContext.Current.CancellationToken);

        var okObjectResult = Assert.IsType<OkObjectResult>(response.Result);
        var listItems = Assert.IsType<IReadOnlyCollection<IngestedAppointmentListItem>>(okObjectResult.Value, exactMatch: false);
        Assert.Empty(listItems);
    }

    [Theory, AutoData]
    public async Task GIVEN_appointment_with_id_WHEN_GetAppointmentById_THEN_return_ok_with_result_in_body(IReadOnlyCollection<IngestedAppointmentListItem> existingAppointments)
    {
        A.CallTo(() => _getIngestedAppointmentListItemsDataService.Get(TestContext.Current.CancellationToken)).Returns(existingAppointments);

        var response = await _sut.Get(TestContext.Current.CancellationToken);

        var okObjectResult = Assert.IsType<OkObjectResult>(response.Result);
        var listItems = Assert.IsType<IReadOnlyCollection<IngestedAppointmentListItem>>(okObjectResult.Value, exactMatch: false);
        Assert.Equal(existingAppointments, listItems);
    }
}