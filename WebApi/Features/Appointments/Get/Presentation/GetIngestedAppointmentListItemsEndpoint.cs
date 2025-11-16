using Microsoft.AspNetCore.Mvc;
using WebApi.Features.Appointments.Get.Contracts;
using WebApi.Features.Appointments.Get.Data;

namespace WebApi.Features.Appointments.Get.Presentation;

[ApiController]
[Route("api")]
public class GetIngestedAppointmentListItemsEndpoint : ControllerBase
{
    private readonly IGetIngestedAppointmentListItemsDataService _getIngestedAppointmentListItemsDataService;

    public GetIngestedAppointmentListItemsEndpoint(IGetIngestedAppointmentListItemsDataService getIngestedAppointmentListItemsDataService)
    {
        _getIngestedAppointmentListItemsDataService = getIngestedAppointmentListItemsDataService;
    }

    [HttpGet("appointments")]
    public async Task<ActionResult<IReadOnlyCollection<IngestedAppointmentListItem>>> Get(CancellationToken cancellationToken)
    {
        return Ok(await _getIngestedAppointmentListItemsDataService.Get(cancellationToken));
    }
}
